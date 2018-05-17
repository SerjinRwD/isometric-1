namespace isometric_1.Types {
    public struct Point2d {
        public int x;
        public int y;

        public Point2d (int x, int y) {
            this.x = x;
            this.y = y;
        }

        public Point2d (int x, int y, Size2d scale) {
            this.x = x * scale.width;
            this.y = y * scale.height;
        }

        public static Point2d operator + (Point2d p1, Point2d p2) {
            return new Point2d (p1.x + p2.x, p1.y + p2.y);
        }

        public static Point2d operator + (Point2d p1, (int, int) p2) {
            return new Point2d (p1.x + p2.Item1, p1.y + p2.Item2);
        }

        public static Point2d operator - (Point2d p1, Point2d p2) {
            return new Point2d (p1.x - p2.x, p1.y - p2.y);
        }

        public static Point2d operator - (Point2d p1, (int, int) p2) {
            return new Point2d (p1.x - p2.Item1, p1.y - p2.Item2);
        }

        public static Point2d operator * (Point2d p1, (int, int) p2) {
            return new Point2d (p1.x * p2.Item1, p1.y * p2.Item2);
        }

        public static bool operator == (Point2d p1, Point2d p2) {
            return p1.x == p2.x && p1.y == p2.y;
        }

        public static bool operator != (Point2d p1, Point2d p2) {
            return p1.x != p2.x || p1.y != p2.y;
        }

        public override bool Equals (object obj) {

            if (obj == null || !obj.GetType ().Equals (typeof (Point2d))) {
                return false;
            }

            var other = (Point2d) obj;

            return other.x == x && other.y == y;
        }

        public override int GetHashCode () {
            return base.GetHashCode ();
        }

        public void Deconstruct (out int x, out int y) {
            x = this.x;
            y = this.y;
        }

        public override string ToString () {
            return $"({x}; {y})";
        }
    }
}