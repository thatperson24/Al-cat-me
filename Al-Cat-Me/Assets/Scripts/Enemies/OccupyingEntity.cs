using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using AStarBase;
using PathFinder;

public class OccupyingEntity
{
    public MapTile CurrentTile;

    /// <summary>
    /// Calculates relatively optimal path to get within range of attacking the player
    /// </summary>
    private void CalculatePathToPlayer(MapTile[][] map, Point destination)
    {
        PriorityQueue<AStar<Point, Cost>.Node> openList = new PriorityQueue<AStar<Point, Cost>.Node>();
        Dictionary<Point, Cost> closedList = new Dictionary<Point, Cost>();

        PathSolver solver = new PathSolver();
        solver.Graph(
            gameMap: map,
            source: this.CurrentTile.MyPosition,
            destination: destination,
            proximityThreshold: 0, // TODO: set threshold to "Range" of enemy's attack
            openList,
            closedList
        );
        Point position = solver.solution.Value.position;
        Debug.Log($"Initial position: {position}");
        Cost cost = solver.solution.Value.cost;
        do
        {
            position = solver.ToPosition(cost.parentIndex);
            Debug.Log($"Next position: {position}");
            cost = closedList[position];
            // TODO: Move the entity to the position
        } while (cost.parentIndex >= 0);
    }
}