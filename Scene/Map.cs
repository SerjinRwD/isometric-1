namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System;

    using isometric_1.Builders;
    using isometric_1.Contract;
    using isometric_1.Finders;
    using isometric_1.Helpers;
    using isometric_1.Types;

    using ManagedSdl;

    public sealed class Map {
        public Size2d MapSize { get; private set; }
        public Size3d TileSize { get; private set; }
        public ImageTileSet TileSet { get; private set; }
        public MapTile[, ] Tiles { get; private set; }
        public Dictionary<Point2d, MapTile> Hash { get; private set; }
        public List<Marker> Markers { get; private set; }
        public Lighting GlobalLight { get; private set; }
        public List<Lighting> LocalLights { get; private set; }
        private int _precalculatedCellWidthHalf;
        private int _precalculatedCellLengthHalf;
        private int _precalculatedCellLengthQuarter;

        public void BypassTiles (Action<int, int> handler) {
            for (var i = 0; i < MapSize.width; i++) {
                for (var j = 0; j < MapSize.height; j++) {
                    handler (i, j);
                }
            }
        }

        public void BypassTiles (Action<MapTile[, ], int, int> handler) {
            for (var i = 0; i < MapSize.width; i++) {
                for (var j = 0; j < MapSize.height; j++) {
                    handler (Tiles, i, j);
                }
            }
        }

        public Map (Size2d mapSize) {
            MapSize = mapSize;

            Hash = new Dictionary<Point2d, MapTile> ();
            Tiles = new MapTile[MapSize.width, MapSize.height];

            BypassTiles ((i, j) => {
                Tiles[i, j] = new MapTile (i, j);
            });

            LocalLights = new List<Lighting> ();
        }

        public Map (Size2d mapSize, IMapBuilder builder) {

            var result = builder.Build (mapSize);

            Markers = result.Markers;
            MapSize = mapSize;
            Tiles = result.Tiles;
            TileSet = builder.TileSet;
            TileSize = builder.TileSize;
            GlobalLight = builder.GlobalLight;

            _precalculatedCellWidthHalf = TileSize.width >> 1;
            _precalculatedCellLengthHalf = TileSize.length >> 1;
            _precalculatedCellLengthQuarter = TileSize.length >> 2;

            LocalLights = new List<Lighting> ();
            //Hash = new Dictionary<Point2d, MapTile> ();
        }

        public void Init () {
            /*
            BypassTiles ((i, j) => {
                var cell = Tiles[i, j];
                var key = cell.IsometricPosition.ToPoint2d ();

                if (Hash.ContainsKey (key)) {
                    Hash[key] = cell;
                } else {
                    Hash.Add (key, cell);
                    //Console.WriteLine ($"hash: {key}");
                }
            });
            */

            BypassTiles ((i, j) => {
                Tiles[i, j].RecalculateNeighbors ();
            });
        }

        public MapTile TileAtScreenPos (Point2d screenPos) {
            /*
            var isoTileX = screenPos.x / _precalculatedCellWidthHalf * _precalculatedCellWidthHalf;
            var isoTileY = screenPos.y / _precalculatedCellLengthHalf * _precalculatedCellLengthHalf + (isoTileX % 2 == 0 ? 0 : _precalculatedCellLengthQuarter);

            var currentIsoTile = new Point2d (
                isoTileX,
                isoTileY);

            //Console.WriteLine ($"screenPos: {screenPos}, currentIsoTile: {currentIsoTile}, match: {Hash.ContainsKey (currentIsoTile)}");

            return Hash.ContainsKey (currentIsoTile) ? Hash[currentIsoTile] : null;
            */

            var mapX = ((screenPos.y / (TileSize.length >> 2)) + (screenPos.x / (TileSize.width >> 1))) >> 1;
            var mapY = ((screenPos.y / (TileSize.length >> 2)) - (screenPos.x / (TileSize.width >> 1))) >> 1;

            if (mapX < 0) { mapX = 0; }
            if (mapX >= MapSize.width) { mapX = MapSize.width - 1; }
            if (mapY < 0) { mapY = 0; }
            if (mapY >= MapSize.height) { mapY = MapSize.height - 1; }

            return Tiles[mapX, mapY];
        }

        public void RecalculateLightings () {
            BypassTiles ((i, j) => {
                var t = Tiles[i, j];

                t.Light = GlobalLight.ToMapTileLight ();

                foreach (var l in LocalLights) {

                    var result = ObstacleFinder.Check (this, Tiles[l.mapPosition.column, l.mapPosition.row], t);

                    if (result.IsGoalAchieved) {
                        var intensity = (byte)(l.intensity / (Compute.EuclideanDistance (new MapPoint (i, j), l.mapPosition) + 1));

                        t.Light = t.Light.Blend(l.color, intensity); // new MapTileLight (l.color /*t.Light.color*/, (byte) Math.Min (255, (t.Light.intensity + intensity))); // * (255 - t.light)));
                    }
                }
            });
        }
    }
}