namespace isometric_1.Types
{
    public struct Point2d
    {
        public int x;
        public int y;

        public Point2d (int x, int y) {
            this.x = x;
            this.y = y;
        }

        public static Point2d operator +(Point2d p1, Point2d p2) {
            return new Point2d(p1.x + p2.x, p1.y + p2.y);
        }

        public static Point2d operator +(Point2d p1, (int, int) p2) {
            return new Point2d(p1.x + p2.Item1, p1.y + p2.Item2);
        }

        public static Point2d operator -(Point2d p1, Point2d p2) {
            return new Point2d(p1.x - p2.x, p1.y - p2.y);
        }

        public static Point2d operator -(Point2d p1, (int, int) p2) {
            return new Point2d(p1.x - p2.Item1, p1.y - p2.Item2);
        }

        public static Point2d operator *(Point2d p1, (int, int) p2) {
            return new Point2d(p1.x * p2.Item1, p1.y * p2.Item2);
        }

        public void Deconstruct(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }

        public override string ToString() {
            return $"({x}; {y})";
        }
    }
}