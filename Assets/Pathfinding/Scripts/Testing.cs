

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start() {
        pathfinding = new Pathfinding(20, 10);
        pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        pathfindingVisual.SetGrid(pathfinding.GetGrid());
        grid = pathfinding.GetGrid();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {

            // Get random grid X and Y
            int nextX = Random.Range(0, 20);
            int nextY = Random.Range(0, 10);

            // Get the world position of x and y
            Vector3 mouseWorldPosition = grid.GetWorldPosition(nextX, nextY);

            // Go there
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(lastX, lastY, x, y);

            // Update character
            characterPathfinding.SetTargetPosition(mouseWorldPosition);

            // Keep track of previous tile
            if (path != null) {
                lastX = x;
                lastY = y;
            }
         }

        if (Input.GetMouseButtonDown(1)) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }

}
