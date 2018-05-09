namespace isometric_1.PathFinder
{
    using System.Collections.Generic;
    using isometric_1.Scene;
    using isometric_1.Types;

    public interface IPathFinder
    {
        Stack<Point2d> Find(Point2d start, Point2d end);
    }
}