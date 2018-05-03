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

        public void Set (int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point3d Add (int dx, int dy, int dz) {
            x += dx;
            y += dy;
            z += dz;

            return this;
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

        public override string ToString() {
            return $"({x}; {y}; {z})";
        }
    }
}