namespace isometric_1.Types {
    public struct Point3d {
        public int x;
        public int y;
        public int z;

        public Point3d (int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Point3d operator +(Point3d p1, Point3d p2) {
            return new Point3d(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
        }

        public static Point3d operator +(Point3d p1, (int, int, int) p2) {
            return new Point3d(p1.x + p2.Item1, p1.y + p2.Item2, p1.z + p2.Item3);
        }

        public static Point3d CalcIsometric (int x, int y, int z) {
            return new Point3d ((x - z) >> 1, y, ((x + z) >> 2)); // down-right diagonal
                               // ((x + z) >> 1, y, ((x - z) >> 2)); // up-right diagonal
        }

        public static Point3d CalcIsometric(Point3d point, Size3d scale) {
            return CalcIsometric(
                point.x * scale.width,
                point.y * scale.height,
                point.z * scale.length);
        }

        public Point2d ToPoint2d() {
            return new Point2d(x, z + y);
        }

        public override string ToString() {
            return $"({x}; {y}; {z})";
        }
    }
}