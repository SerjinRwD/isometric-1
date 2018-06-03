namespace isometric_1.Builders {
    using System.Collections.Generic;
    using System;

    using isometric_1.Contract;
    using isometric_1.ManagedSdl;
    using isometric_1.Scene;
    using isometric_1.Types;

    public class Demo1MapBuilder : AbstractMapBuilder {
        public Demo1MapBuilder (MapTilePrototypeLibrary library) : base (library) { }

        public override MapBuildResult Build (Size2d mapSize) {
            var tiles = new MapTile[mapSize.width, mapSize.height];
            var markers = new List<Marker> ();
            var rnd = new Random ();
            var putPlayer = false;
            var lights1Count = 4;
            var lights2Count = 2;
            var treesCount = ((mapSize.width * mapSize.height) / 100) * 10;
            int i, j, n;

            GlobalLight = new Lighting (SdlColorFactory.FromRGBA (255, 255, 255, 10));

            for (i = 0; i < mapSize.width; i++) {
                for (j = 0; j < mapSize.height; j++) {
                    n = rnd.Next (1, 100);

                    if (n < 11) {
                        tiles[i, j] = Library.HashedTiles["field"].Create (new MapPoint (i, j)); // -w-tree-1
                    } else if (n < 12) {
                        tiles[i, j] = Library.HashedTiles["field"].Create (new MapPoint (i, j)); // -w-corpse-1
                    } else if (n < 13) {
                        tiles[i, j] = Library.HashedTiles["field"].Create (new MapPoint (i, j, (i + j) % 5));
                    } else {
                        var rx = Math.Max (0, i - 1);
                        var ry = Math.Max (0, j - 1);

                        if (tiles[rx, j] != null && tiles[rx, j].MapCoords.level > 0) {
                            tiles[i, j] = Library.HashedTiles["ramp-w-1"].Create (new MapPoint (i, j, tiles[rx, j].MapCoords.level - 1));
                        } else if (tiles[i, ry] != null && tiles[i, ry].MapCoords.level > 0) {
                            tiles[i, j] = Library.HashedTiles["ramp-n-1"].Create (new MapPoint (i, j, tiles[i, ry].MapCoords.level - 1));
                        } else {
                            tiles[i, j] = Library.HashedTiles["field"].Create (new MapPoint (i, j));

                            if (!putPlayer) {
                                markers.Add (Library.HashedMarkers["player-1"].Create (new MapPoint (i, j)));
                                putPlayer = true;
                            }
                        }
                    }
                }
            }
 
            while(lights1Count-- > 0) {
                i = rnd.Next (0, mapSize.width);
                j = rnd.Next (0, mapSize.height);

                var tile = tiles[i, j];

                markers.Add (Library.HashedMarkers["light-1"].Create (new MapPoint (i, j, tile.MapCoords.level)));

                tile.TileType = MapTileType.Wall;
            }

            while(lights2Count-- > 0) {
                i = rnd.Next (0, mapSize.width);
                j = rnd.Next (0, mapSize.height);

                var tile = tiles[i, j];

                markers.Add (Library.HashedMarkers["light-2"].Create (new MapPoint (i, j, tile.MapCoords.level)));

                tile.TileType = MapTileType.Wall;
            }

            while(treesCount-- > 0) {
                i = rnd.Next (0, mapSize.width);
                j = rnd.Next (0, mapSize.height);

                var tile = tiles[i, j];

                markers.Add (Library.HashedMarkers["tree-1"].Create (new MapPoint (i, j, tile.MapCoords.level)));

                tile.TileType = MapTileType.Wall;
            }

            return new MapBuildResult (tiles, markers);
        }
    }
}