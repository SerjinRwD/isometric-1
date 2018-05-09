namespace isometric_1.Builders {
    using System.Collections.Generic;

    using isometric_1.Scene;

    public class MapBuildResult {
        public MapTile[, ] Tiles { get; private set; }
        public List<Marker> Markers { get; private set; }

        public MapBuildResult (MapTile[, ] tiles, List<Marker> markers) {
            Tiles = tiles;
            Markers = markers;
        }
    }
}