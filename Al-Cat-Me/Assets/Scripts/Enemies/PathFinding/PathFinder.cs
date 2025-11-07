using System;
using System.Collections.Generic;
using System.Drawing;
using AStarBase;

namespace PathFinder
{
    public struct Cost : IComparable<Cost>
    {
        public readonly int parentIndex;
        public readonly int distanceTravelled; /*g(x)*/
        public readonly int totalCost; /*f(x)*/
        public Cost(int parentIndex, int distanceTravelled, int totalCost)
        {
            this.parentIndex = parentIndex;
            this.distanceTravelled = distanceTravelled;
            this.totalCost = totalCost;
        }
        public int CompareTo(Cost other)
        {
            return this.totalCost.CompareTo(other.totalCost);
        }
    }

    /// <example>
    /// 
    /// const solver = new PathFinder();
    /// solver.Graph(map, source, destination, threshold, openList, closedList);
    /// Point position = solver.solution.Value.position;
    /// Cost cost = bitmapSolver.solution.Value.cost;
    /// do {
    ///     position = solver.ToPosition(cost.parentIndex);
    ///     cost = closedList[position];
    ///     // Move the entity to the position
    /// } while (cost.parentIndex >= 0);
    /// 
    /// // Do attacks
    /// 
    /// </example>
    public class PathFinder : AStar<Point, Cost>
    {
        // TODO: delete this since we only allow orthogonal movements
        // private const int baseOrthogonalCost = 5;
        // private const int baseDiagonalCost = 7;

        public Node? solution;
        private MapTile[][] gameMap;
        private int mapWidth;
        private int mapHeight;
        /** How close the enemy needs to get to user to be "close enough" to attack */
        private double proximityThreshold;
        private Point destination;
        private Dictionary<Point, Cost> closedList;

        // TODO: delete this?
        // public void PathFinder(MapTile[] gameMap)
        // {
        //     this.gameMap = gameMap;
        // }

        public void Graph(
            MapTile[][] gameMap,
            Point source,
            Point destination,
            int proximityThreshold,
            PriorityQueue<Node> openList,
            Dictionary<Point, Cost> closedList
        )
        {
            this.gameMap = gameMap;
            this.mapWidth = gameMap.Length;
            this.mapHeight = gameMap[0].Length; // TODO: update this if we have variable width rows
            this.proximityThreshold = proximityThreshold;
            this.closedList = closedList;
            this.destination = destination;
            // destination = new Point(mapWidth - 1, mapHeight - 1);
            this.Graph(
                new Node(
                    source,
                    new Cost(
                        parentIndex: -1,
                        distanceTravelled: 0,
                        totalCost: GetDistance(source, destination)
                    )
                ),
                openList,
                closedList
            );
        }

        public int ToIndex(Point position)
        {
            return position.Y * mapWidth + position.X;
        }

        /**
         * This method converts a "parent index" into an actual point to travel to.
         */
        public Point ToPosition(int index)
        {
            return new Point(index % mapWidth, index / mapWidth);
        }
        protected override void AddNeighbours(Node node, PriorityQueue<Node> openList)
        {
            int parentIndex = ToIndex(node.position);
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    if (x != 0 && y != 0) continue; // This prevents calculating any diagonal movements

                    Point newPos = new Point(node.position.X + x, node.position.Y + y);
                    if (IsPointWithinMapBounds(newPos))
                    {
                        MapTile targetTile = gameMap[newPos.Y][newPos.X];
                        if (MapTile.TileState.CanBeOccupied(targetTile.GetState()))
                        {
                            int distanceCost = node.cost.distanceTravelled + GetCostToTraverseTile(targetTile);
                            openList.Insert(
                                new Node(
                                    newPos,
                                    new Cost(
                                        parentIndex: parentIndex,
                                        distanceTravelled: distanceCost,
                                        totalCost: distanceCost + GetDistance(newPos, destination)
                                    )
                                )
                            );
                        }
                    }
                }
            }
        }

        private static int GetDistance(Point source, Point destination)
        {
            int dx = Math.Abs(destination.X - source.X);
            int dy = Math.Abs(destination.Y - source.Y);
            return dx + dy;

            // TODO: delete logic that handles diagonals + orthogonal movement

            // int diagonal = Math.Min(dx, dy);
            // int orthogonal = dx + dy - 2 * diagonal;
            // return diagonal * baseDiagonalCost + orthogonal * baseOrthogonalCost;
        }

        private static int GetCostToTraverseTile(MapTile tile)
        {
            return tile.GetState() switch
            {
                MapTile.TileState.TERRAIN => 9999, // TODO: this is kinda boof
                MapTile.TileState.MUDDY => 2,
                _ => 1,
            };

            // If we allowed diagonal movements, we would use something like this as a multiplier,
            // where x/y are those used in AddNeighbors()
            // ((x == 0 || y == 0) ? baseOrthogonalCost : baseDiagonalCost);
        }

        protected override bool IsDestination(Point position)
        {
            double trueDistance = Math.Sqrt(
                Math.Pow(Math.Abs(position.X - destination.X), 2)
                + Math.Pow(Math.Abs(position.Y - destination.Y), 2)
            );
            bool isSolved = (position == destination) || trueDistance < proximityThreshold;

            if (isSolved)
            {
                solution = new Node(position, closedList[position]);
            }
            return isSolved;
        }

        private bool IsPointWithinMapBounds(Point position)
        {
            return (position.X >= 0 && position.X < mapWidth)
                && (position.Y >= 0 && position.Y < mapHeight);
        }
    }
}