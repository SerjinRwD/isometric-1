namespace isometric_1.PathFinder {
    using System;
    using isometric_1.Types;

    public static class Heuristics {
        public static int ManhattanDistance (Point2d from, Point2d to) {
            return Math.Abs(to.x - from.x) + Math.Abs(to.y - from.y);
        }
    }
}