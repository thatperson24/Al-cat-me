using System;
using Point = System.Drawing.Point;

public static class DistanceUtils
{
    public static double PythagDistanceBetweenPoints(Point a, Point b)
    {
        return Math.Sqrt(
            Math.Pow(Math.Abs(a.X - b.X), 2)
                + Math.Pow(Math.Abs(a.Y - b.Y), 2)
         );
    }
}