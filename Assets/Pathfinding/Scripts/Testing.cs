

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using CodeMonkey;

public class Testing : MonoBehaviour {
    
    [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual;
    [SerializeField] private CharacterPathfindingMovementHandler characterPathfinding;
    private Pathfinding pathfinding;
    private Grid<PathNode> grid;
    
    private int lastX = 0;
    private int lastY = 0; 

    public static bool isRunning = false;
    public static bool isWalking = false;
    private int GRID_X = 20;
    private int GRID_Y = 10;

    private void Start() {
        pathfinding = new Pathfinding(GRID_X, GRID_Y);
        pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        pathfindingVisual.SetGrid(pathfinding.GetGrid());
        grid = pathfinding.GetGrid();
    }

    private void OnFinished() {
        isRunning = true;

        // Get random grid X and Y  
        int nextX = Random.Range(0, GRID_X);
        int nextY = Random.Range(0, GRID_Y);

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
        
        // Do routine if debugview isn't drawing AND Roomba isn't walking.
        if (!isRunning && !isWalking) {
            OnFinished();
        }

        // DEBUG
        if (Input.GetMouseButtonDown(1)) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }

}
