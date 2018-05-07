namespace isometric_1.PathFinder {
    using System;
    using isometric_1.Types;

    public static class Heuristics {
        public static int ManhattanDistance (Point2d from, Point2d to) {
            return Math.Abs(to.x - from.x) + Math.Abs(to.y - from.y);
        }

        public static int ChebyshevDistance(Point2d from, Point2d to) {
            return Math.Max(Math.Abs(from.x - to.x), Math.Abs(from.y - to.y));
        }

        public static int EuclideanDistance(Point2d from, Point2d to) {
            var dx = from.x - to.x;
            var dy = from.y - to.y;

            return (int)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}