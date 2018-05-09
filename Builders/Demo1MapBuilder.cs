namespace isometric_1.Builders {
    using System.Collections.Generic;
    using System;

    using isometric_1.Contract;
    using isometric_1.Scene;
    using isometric_1.Types;

    public class Demo1MapBuilder : AbstractMapBuilder {
        public Demo1MapBuilder (MapTilePrototypeLibrary library) : base (library) { }

        public override MapBuildResult Build (Size2d mapSize) {
            var tiles = new MapTile[mapSize.width, mapSize.height];
            var markers = new List<Marker> ();
            var rnd = new Random ();
            var putPlayer = false;

            for (var i = 0; i < mapSize.width; i++) {
                for (var j = 0; j < mapSize.height; j++) {
                    var n = rnd.Next (1, 100);

                    if (n < 51) {
                        tiles[i, j] = Library.HashedTiles["field"].Create (new Point2d (i, j));

                        if (!putPlayer) {
                            markers.Add(new Marker(new Point2d(i, j), "player-1"));
                            putPlayer = true;
                        }
                    } else if (n < 71) {
                        tiles[i, j] = Library.HashedTiles["field-w-tree-1"].Create (new Point2d (i, j));
                    } else if (n < 72) {
                        tiles[i, j] = Library.HashedTiles["field-w-corpse-1"].Create (new Point2d (i, j));
                    } else {
                        tiles[i, j] = Library.HashedTiles["field"].Create (new Point2d (i, j));
                    }
                }
            }

            return new MapBuildResult (tiles, markers);
        }
    }
}