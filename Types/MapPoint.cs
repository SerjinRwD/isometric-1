namespace isometric_1.Types {
    public struct MapPoint {
        public static readonly MapPoint Zero = new MapPoint(0 , 0, 0);

        public int row;
        public int column;
        public int level;

        public MapPoint(int column, int row) {
            this.row = row;
            this.column = column;
            this.level = 0;
        }

        public MapPoint(int column, int row, int level) {
            this.row = row;
            this.column = column;
            this.level = level;
        }

        public static bool operator == (MapPoint p1, MapPoint p2) {
            return p1.column == p2.column && p1.row == p2.row && p1.level == p2.level;
        }

        public static bool operator != (MapPoint p1, MapPoint p2) {
            return p1.column != p2.column || p1.row != p2.row || p1.level != p2.level;
        }

        public override bool Equals (object obj) {

            if (obj == null || !obj.GetType ().Equals (typeof (MapPoint))) {
                return false;
            }

            var other = (MapPoint) obj;

            return column == other.column && row == other.row && level == other.level;
        }

        public override int GetHashCode () {
            return base.GetHashCode ();
        }

        public void Deconstruct (out int column, out int row) {
            column = this.column;
            row = this.row;
        }

        public void Deconstruct (out int column, out int row, out int level) {
            column = this.column;
            row = this.row;
            level = this.level;
        }

        public Point2d ToPoint2d (Size3d scale) {
            return new Point2d (column * scale.width, row * scale.length - level * scale.height);
        }

        public Point2d ToPoint2d (int scale) {
            return new Point2d (column * scale, row * scale - level * scale);
        }

        public Point3d ToPoint3d (Size3d scale) {
            return new Point3d (column, level, row, scale);
        }

        public static MapPoint FromPoint3d(Point3d point, Size3d scale) {
            return new MapPoint(point.x / scale.width, point.z / scale.length);
        }
    }
}