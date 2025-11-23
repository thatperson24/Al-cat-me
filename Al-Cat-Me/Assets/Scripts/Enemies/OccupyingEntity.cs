using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using AStarBase;
using PathFinder;

public static class OccupyingEntity
{
    private const double PROXIMITY_THRESHOLD = 4;

    /// <summary>
    /// Calculates relatively optimal path to get within range of attacking the player
    /// </summary>
    public static List<Point> CalculatePathToPlayer(MapTile[][] map, MapTile currentTile, Point destination)
    {
        PriorityQueue<AStar<Point, Cost>.Node> openList = new PriorityQueue<AStar<Point, Cost>.Node>();
        Dictionary<Point, Cost> closedList = new Dictionary<Point, Cost>();

        PathSolver solver = new PathSolver();
        solver.Graph(
            gameMap: map,
            source: currentTile.MyPosition,
            destination: destination,
            proximityThreshold: PROXIMITY_THRESHOLD, // TODO: set threshold to dynamic "Range" of enemy's attack
            openList,
            closedList
        );

        // TODO: should actually check that `solver.solution.HasValue` before assuming we found a path
        Point position = solver.solution.Value.position;
        List<Point> result = new();
        result.Add(position);

        Cost cost = solver.solution.Value.cost;
        do
        {
            position = solver.ToPosition(cost.parentIndex);
            result.Add(position);
            cost = closedList[position];
        } while (cost.parentIndex >= 0);

        Debug.Log($"Found path: {string.Join(" -> ", result)}");
        return result;
    }
}