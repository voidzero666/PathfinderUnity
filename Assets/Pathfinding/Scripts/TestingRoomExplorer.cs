

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using CodeMonkey;

public class TestingRoomExplorer : MonoBehaviour {
    [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual;
    [SerializeField] private CharacterPathfindingMovementHandler characterPathfinding;
    private Pathfinding pathfinding;
    private Grid<PathNode> grid;
    
    private int lastX = 0;
    private int lastY = 0; 

    private int GRID_X = 20;
    private int GRID_Y = 10;

    private int EXPLORED_X = 0;
    private int EXPLORED_Y = 0;

    private void Start() {
        pathfinding = new Pathfinding(GRID_X, GRID_Y);
        pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        pathfindingVisual.SetGrid(pathfinding.GetGrid());
        grid = pathfinding.GetGrid();
    }

    private void ExploreRoomX() {
        StatsHandler.Instance.log("Exploring room X...");

        Globals.isRunning = true;
        int nextX = lastX + 1; // 0 -> 1

        // Get the world position of x and y
        Vector3 mouseWorldPosition = grid.GetWorldPosition(nextX, lastY); // 1,0

        // Find path
        pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);  // 1, 0
        List<PathNode> path = pathfinding.FindPath(lastX, lastY, nextX, lastY); // 0, 0 -> 1, 0

        // Update character
        characterPathfinding.SetTargetPosition(mouseWorldPosition);

        // Keep track of previous tile
        if (path != null) {
            lastX = nextX;
            lastY = 0;
        } else {
            EXPLORED_X = lastX;
            Globals.xExplored = true;
            ReturnToBase();
        }

        StatsHandler.Instance.updateCycles();
    }
    
    private void ExploreRoomY() {
        StatsHandler.Instance.log("Exploring room Y...");

        Globals.isRunning = true;
        int nextY = lastY + 1;
        lastX = 0;

        // Get the world position of x and y
        Vector3 mouseWorldPosition = grid.GetWorldPosition(lastX, nextY);

        // Find path
        pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);  // 1, 0
        List<PathNode> path = pathfinding.FindPath(lastX, lastY, lastX, nextY); // 0, 0 -> 1, 0

        // Update character
        characterPathfinding.SetTargetPosition(mouseWorldPosition);

        // Keep track of previous tile
        if (path != null) {
            lastY = nextY;
            lastX = 0;
        } else {
            EXPLORED_Y = lastY;
            Globals.yExplored = true;
            // ReturnToBase();
        }

        StatsHandler.Instance.updateCycles();
    }

    private void OnFinished() {
        Globals.isRunning = true;

        // Get random grid X and Y  
        int nextX = Random.Range(1, EXPLORED_X + 1);
        int nextY = Random.Range(1, EXPLORED_Y + 1);

        StatsHandler.Instance.log("Zooming to " + nextX + "," + nextY);

        // Get the world position of x and y
        Vector3 mouseWorldPosition = grid.GetWorldPosition(nextX, nextY);

        // Find path
        pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
        List<PathNode> path = pathfinding.FindPath(lastX, lastY, x, y);

        // Update character
        characterPathfinding.SetTargetPosition(mouseWorldPosition);

        // Keep track of previous tile
        if (path != null) {
            lastX = x;
            lastY = y;
        }

        // Use StatsHandler to keep track of cycles
        StatsHandler.Instance.updateCycles();
    }

    private void Update() {
        
        if (!Globals.isRunning && !Globals.isWalking && !Globals.xExplored) {
            ExploreRoomX();
        }

        Globals.ignoreW = true;
        if (!Globals.isRunning && !Globals.isWalking && Globals.xExplored && !Globals.yExplored) {
            ExploreRoomY();
        }
        Globals.ignoreW = false;
        
        if (!Globals.isRunning && !Globals.isWalking && Globals.yExplored && Globals.xExplored) {
            OnFinished();
        }

        // DEBUG
        if (Input.GetMouseButtonDown(1)) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }

    private void ReturnToBase() {
        Globals.isRunning = true;

        // Get the world position of x and y
        Vector3 mouseWorldPosition = grid.GetWorldPosition(0, 1);

        // Find path
        pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);  // 1, 0
        List<PathNode> path = pathfinding.FindPath(lastX, lastY, x, y); // 0, 0 -> 1, 0

        // Update character
        characterPathfinding.SetTargetPosition(mouseWorldPosition);

        StatsHandler.Instance.updateCycles();

        lastY = 1; // We returnen naar de tile boven base
        lastX = 0; // Same
    }

}
