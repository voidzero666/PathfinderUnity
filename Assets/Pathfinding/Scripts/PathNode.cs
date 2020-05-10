
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {

    private Grid<PathNode> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public bool hasBeenWalkedOn = false;
    public PathNode cameFromNode;

    public PathNode(Grid<PathNode> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = generateWalkable(x, y);
    }

    // A very simple implementation to generate random maps? We can extend this later.
    public bool generateWalkable(int x, int y) {
        if (x != 0 && y != 0) {
            return Random.Range(0, 100) > 10;
        }
        return true;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
        if (hasBeenWalkedOn) {
            fCost *= 2;
        }
    }

    public void SetHasBeenWalked() {
        this.hasBeenWalkedOn = true;
    }

    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString() {
        return x + "," + y;
    }

}
