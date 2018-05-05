namespace isometric_1.Contract {
    using System;
    using isometric_1.Scene;
    using isometric_1.Types;

    public interface IMapBuilder {
        ImageTileSet TileSet { get; }
        Size3d TileSize { get; }
        MapTilePrototypeLibrary Library { get; }
        MapTile[, ] Build (Size2d mapSize);
    }
}