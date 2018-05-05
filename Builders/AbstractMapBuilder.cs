namespace isometric_1.Builders {
    using System;
    using isometric_1.Contract;
    using isometric_1.Scene;
    using isometric_1.Types;

    public abstract class AbstractMapBuilder : IMapBuilder {
        private ImageTileSet _tileSet;
        private Size3d _tileSize;
        private MapTilePrototypeLibrary _library;

        public ImageTileSet TileSet { get => _tileSet; }
        public Size3d TileSize { get => _tileSize; }
        public MapTilePrototypeLibrary Library { get => _library; }
        public abstract MapTile[, ] Build (Size2d mapSize);

        public AbstractMapBuilder (MapTilePrototypeLibrary library) {
            _tileSet = library.TileSet;
            _tileSize = library.TileSize;
            _library = library;
        }
    }
}