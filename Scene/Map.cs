namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System;

    using isometric_1.Builders;
    using isometric_1.Contract;
    using isometric_1.Helpers;
    using isometric_1.Types;

    using ManagedSdl;

    public sealed class Map : IRenderable {
        public Size2d MapSize { get; private set; }
        public Size3d TileSize { get; private set; }
        public ImageTileSet TileSet { get; private set; }
        public MapTile[, ] Tiles { get; private set; }
        public Dictionary<Point2d, MapTile> Hash { get; private set; }
        public List<Marker> Markers { get; private set; }
        private int _precalculatedCellWidthHalf;
        private int _precalculatedCellLengthHalf;
        private int _precalculatedCellLengthQuarter;

        public Map (Size2d mapSize) {
            MapSize = mapSize;

            Tiles = new MapTile[MapSize.width, MapSize.height];

            for (var i = 0; i < MapSize.width; i++) {
                for (var j = 0; j < MapSize.height; j++) {
                    Tiles[i, j] = new MapTile (i, j);
                }
            }
        }
        public Map (Size2d mapSize, IMapBuilder builder) {

            var result = builder.Build (mapSize);

            Markers = result.Markers;
            MapSize = mapSize;
            Tiles = result.Tiles;
            TileSet = builder.TileSet;
            TileSize = builder.TileSize;

            _precalculatedCellWidthHalf = TileSize.width >> 1;
            _precalculatedCellLengthHalf = TileSize.length >> 1;
            _precalculatedCellLengthQuarter = TileSize.length >> 2;

            Hash = new Dictionary<Point2d, MapTile> ();

            for (var i = 0; i < MapSize.width; i++) {
                for (var j = 0; j < MapSize.height; j++) {
                    var cell = Tiles[i, j];
                    var key = new Point2d (
                        cell.IsometricPosition.x,
                        (cell.IsometricPosition.z - cell.IsometricPosition.y));

                    if (Hash.ContainsKey (key)) {
                        Hash[key] = cell;
                    } else {
                        Hash.Add (key, cell);
                        //Console.WriteLine ($"hash: {key}");
                    }
                }
            }
        }

        public void Render (SdlRenderer renderer, Viewport viewport) {

            if (viewport == null) {
                throw new InvalidOperationException (@"Viewport is not set");
            }

            //*
            var fromX = 0;
            var fromY = 0;
            var toX = MapSize.width;
            var toY = MapSize.height;
            //*/

            /*
            var fromX = Math.Max (0, viewport.Position.MapX);
            var fromY = Math.Max (0, viewport.Position.MapY);
            var toX = Math.Min (MapWidth - 1, viewport.BottomRight.MapX);
            var toY = Math.Min (MapHeight - 1, viewport.BottomRight.MapY);
            //*/

            int i, j;

            for (i = fromX; i < toX; i++) {
                for (j = fromY; j < toY; j++) {
                    RenderMapCell (renderer, Tiles[i, j], viewport);
                }
            }
        }

        public void RenderMapCell (SdlRenderer renderer, MapTile mapTile, Viewport viewport) {
            ImageTile imageTile;
            var texture = TileSet.Texture;
            int resultIsoX = mapTile.IsometricPosition.x - viewport.Position.x + (viewport.Size.width >> 1);
            int resultIsoY = mapTile.IsometricPosition.z - viewport.Position.y;

            if (mapTile.BlockId != ImageTile.NOT_SET && mapTile.Level > 0) {
                imageTile = TileSet.Tiles[mapTile.BlockId];

                for (var i = 0; i < mapTile.Level; i++) {
                    renderer.RenderTexture (
                        texture,
                        resultIsoX - imageTile.OriginX, resultIsoY - (i + 1) * TileSize.height - imageTile.OriginY,
                        TileSet.Tiles[mapTile.BlockId].GetClipRect ());
                }

            }

            if (mapTile.FloorId != ImageTile.NOT_SET) {
                imageTile = TileSet.Tiles[mapTile.FloorId];

                renderer.RenderTexture (
                    texture,
                    resultIsoX - imageTile.OriginX, resultIsoY - mapTile.IsometricPosition.y - imageTile.OriginY,
                    imageTile.GetClipRect ());
            }

            if (mapTile.IsSelected) {
                imageTile = TileSet.Tiles[0];

                renderer.SetDrawColor (255, 255, 55);
                renderer.DrawPoint (resultIsoX, resultIsoY);

                /*
                renderer.RenderTexture (
                    texture,
                    resultIsoX - imageTile.OriginX, resultIsoY - mapTile.IsometricPosition.y - imageTile.OriginY,
                    imageTile.GetClipRect ());
                */
            }

            if (!(mapTile.DecorationIds == null || mapTile.DecorationIds.Length == 0)) {
                for (var tid = 0; tid < mapTile.DecorationIds.Length; tid++) {
                    imageTile = TileSet.Tiles[mapTile.DecorationIds[tid]];

                    renderer.RenderTexture (
                        texture,
                        resultIsoX - imageTile.OriginX, resultIsoY - mapTile.IsometricPosition.y - imageTile.OriginY,
                        imageTile.GetClipRect ());
                }
            }

            if (mapTile.Visitor != null) {
                mapTile.Visitor.Render (renderer, viewport);
            }

        }

        public MapTile TileAtScreenPos (Point2d screenPos) {
            var isoTileX = screenPos.x / _precalculatedCellWidthHalf * _precalculatedCellWidthHalf;
            var isoTileY = screenPos.y / _precalculatedCellLengthHalf * _precalculatedCellLengthHalf + (isoTileX % 2 == 0 ? 0 : _precalculatedCellLengthQuarter);

            var currentIsoTile = new Point2d (
                isoTileX,
                isoTileY);

            return Hash.ContainsKey (currentIsoTile) ? Hash[currentIsoTile] : null;
        }
    }
}