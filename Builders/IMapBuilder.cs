namespace isometric_1.Builders {
    using System;
    using isometric_1.Scene;
    using isometric_1.Types;

    public interface IMapBuilder {
        ImageTileSet TileSet { get; }
        Size3d TileSize { get; }
        MapTilePrototypeLibrary Library { get; }
        MapBuildResult Build (Size2d mapSize);
    }
}