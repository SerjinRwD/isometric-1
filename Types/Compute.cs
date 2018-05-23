namespace isometric_1.Types {
    using System;

    using isometric_1.Scene;

    public static class Compute {

        /// <summary>
        /// <para>Матрица направлений.</para>
        /// <para>Берётся исходная матрица:</para>
        /// <para>(-1,  1) (0,  1) (1,  1)</para>
        /// <para>(-1,  0) (0,  0) (1,  0)</para>
        /// <para>(-1, -1) (0, -1) (1, -1)</para>
        /// <para>К каждому индексу прибавляется 1, затем матрица поворачивается на 90* по часовой стрелке:</para>
        /// <para>( 0,  0) (0,  1) (0,  2)</para>
        /// <para>( 1,  0) (1,  1) (1,  2)</para>
        /// <para>( 2,  0) (2,  1) (2,  2)</para>
        /// </summary>
        private static Direction[, ] _directionMatrix = new Direction[3, 3] {
            //
            { Direction.SW, Direction.W, Direction.NW },
            //
            { Direction.S, Direction.E, Direction.N },
            //
            { Direction.SE, Direction.E, Direction.NE }
        };

        private static Point3d[] _moveAdditions = new Point3d[8] {
            /// <summary>
            /// E
            /// </summary>
            new Point3d (2, 0, 0),
            /// <summary>
            /// NE
            /// </summary>
            new Point3d (1, 0, 1),
            /// <summary>
            /// N
            /// </summary>
            new Point3d (0, 0, 2),
            /// <summary>
            /// NW
            /// </summary>
            new Point3d (-1, 0, 1),
            /// <summary>
            /// W
            /// </summary>
            new Point3d (-2, 0, 0),
            /// <summary>
            /// SW
            /// </summary>
            new Point3d (-1, 0, -1),
            /// <summary>
            /// S
            /// </summary>
            new Point3d (0, 0, -2),
            /// <summary>
            /// SE
            /// </summary>
            new Point3d (1, 0, -1)
        };

        public static Point3d Isometric (int x, int y, int z) {
            return new Point3d ((x - z) >> 1, y, ((x + z) >> 2)); // down-right diagonal
            // ((x + z) >> 1, y, ((x - z) >> 2)); // up-right diagonal
        }

        public static Point3d Isometric (Point2d point) {
            return Isometric (point.x, 0, point.y);
        }

        public static Point3d Isometric (Point3d point) {
            return Isometric (point.x, point.y, point.z);
        }

        public static Point3d Isometric (Point3d point, Size3d scale) {
            return Isometric (
                point.x * scale.width,
                point.y * scale.height,
                point.z * scale.length);
        }

        public static Point3d Isometric (int x, int y, int z, Size3d scale) {
            return Isometric (new Point3d (x, y, z), scale);
        }

        public static int ManhattanDistance (Point2d from, Point2d to) {
            return Math.Abs (to.x - from.x) + Math.Abs (to.y - from.y);
        }
        
        public static int ManhattanDistance (MapPoint from, MapPoint to) {
            return Math.Abs (to.column - from.column) + Math.Abs (to.row - from.row);
        }

        public static int ChebyshevDistance (Point2d from, Point2d to) {
            return Math.Max (Math.Abs (from.x - to.x), Math.Abs (from.y - to.y));
        }

        public static int ChebyshevDistance (MapPoint from, MapPoint to) {
            return Math.Max (Math.Abs (from.column - to.column), Math.Abs (from.row - to.row));
        }

        public static int EuclideanDistance (Point2d from, Point2d to) {
            var dx = from.x - to.x;
            var dy = from.y - to.y;

            return (int) Math.Sqrt (dx * dx + dy * dy);
        }

        public static int EuclideanDistance (MapPoint from, MapPoint to) {
            var dx = from.column - to.column;
            var dy = from.row - to.row;

            return (int) Math.Sqrt (dx * dx + dy * dy);
        }

        public static Direction DirectionBetweenPoints (MapPoint from, MapPoint to) {
            var dx = Math.Sign (to.column - from.column) + 1;
            var dy = Math.Sign (to.row - from.row) + 1;

            return _directionMatrix[dx, dy];
        }

        public static Direction DirectionBetweenPoints (Point2d from, Point2d to) {
            var dx = Math.Sign (to.x - from.x) + 1;
            var dy = Math.Sign (to.y - from.y) + 1;

            return _directionMatrix[dx, dy];
        }

        public static Direction DirectionOfVector(int vx, int vy) {
            var dx = Math.Sign (vx) + 1;
            var dy = Math.Sign (vy) + 1;

            return _directionMatrix[dx, dy];
        }

        public static Point3d StepToDirection (Point3d pos, Direction d, int speed) {
            return pos + new Point3d (_moveAdditions[(int) d], new Size3d (speed, 0, speed));
        }
    }
}