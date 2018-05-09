namespace isometric_1.Scene {
    using isometric_1.Types;
    
    public class Marker {
        public string Type { get; private set; }
        public Point2d MapPosition { get; private set; }
        public Marker (Point2d mapPosition, string type) {
            Type = type;
            MapPosition = mapPosition;
        }
    }
}