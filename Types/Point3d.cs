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

        public Point3d (int x, int y, int z, Size3d scale) {
            this.x = x * scale.width;
            this.y = y * scale.height;
            this.z = z * scale.length;
        }

        public Point3d (Point3d point, Size3d scale) {
            this.x = point.x * scale.width;
            this.y = point.y * scale.height;
            this.z = point.z * scale.length;
        }

        public static Point3d operator + (Point3d p1, Point3d p2) {
            return new Point3d (p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
        }

        public static Point3d operator + (Point3d p1, (int, int, int) p2) {
            return new Point3d (p1.x + p2.Item1, p1.y + p2.Item2, p1.z + p2.Item3);
        }

        public Point2d ToPoint2d (bool doCountY = true) {
            return doCountY ?
                new Point2d (x, z - y) :
                new Point2d (x, z);
        }

        public void Deconstruct (out int x, out int y, out int z) {
            x = this.x;
            y = this.y;
            z = this.z;
        }

        public override string ToString () {
            return $"({x}; {y}; {z})";
        }
    }
}