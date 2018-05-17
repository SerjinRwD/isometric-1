namespace isometric_1.Builders {
    using System;

    using isometric_1.Contract;
    using isometric_1.Scene;
    using isometric_1.Types;

    public class PyramidMapBuilder : AbstractMapBuilder {
        public PyramidMapBuilder (MapTilePrototypeLibrary library) : base (library) { }

        public override MapBuildResult Build (Size2d mapSize) {
            var tiles = new MapTile[mapSize.width, mapSize.height];

            for (var i = 0; i < mapSize.width; i++) {
                for (var j = 0; j < mapSize.height; j++) {
                    tiles[i, j] = Library.HashedTiles["field"].Create (new Point2d (i, j), (i + j) % 3);
                }
            }

            return new MapBuildResult (tiles, null);
        }
    }
}