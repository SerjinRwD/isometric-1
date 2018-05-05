namespace isometric_1.Builders {
    using System;

    using isometric_1.Contract;
    using isometric_1.Scene;
    using isometric_1.Types;

    public class Demo1MapBuilder : AbstractMapBuilder {
        public Demo1MapBuilder (MapTilePrototypeLibrary library) : base (library) { }

        public override MapTile[, ] Build (Size2d mapSize) {
            var tiles = new MapTile[mapSize.width, mapSize.height];
            var rnd = new Random();

            for (var i = 0; i < mapSize.width; i++) {
                for (var j = 0; j < mapSize.height; j++) {
                    var n = rnd.Next(1, 100);

                    if(n < 51) {
                        tiles[i, j] = Library.HashedTiles["field"].Create (new Point3d (i, 0, j));
                    } else if(n < 71) {
                        tiles[i, j] = Library.HashedTiles["field-w-tree-1"].Create (new Point3d (i, 0, j));
                    } else if(n < 72) {
                        tiles[i, j] = Library.HashedTiles["field-w-corpse-1"].Create (new Point3d (i, 0, j));
                    } else {
                        tiles[i, j] = Library.HashedTiles["field"].Create (new Point3d (i, 0, j));
                    }
                }
            }

            return tiles;
        }
    }
}