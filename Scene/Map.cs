namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System;
    using isometric_1.Contract;
    using isometric_1.Helpers;
    using isometric_1.Types;
    using SDL2;

    public class MapCell {
        public Point3d IsometricPosition { get; private set; }
        public int Column { get; private set; }
        public int Row { get; private set; }
        public int Height { get; private set; }
        public bool IsBlocked { get; private set; }
        public bool IsSelected { get; set; }
        public int FloorId { get; private set; }
        public int BlockId { get; private set; }
        public int[] DecorationIds { get; private set; }
        public object Tag { get; set; }

        public MapCell (SceneContext context,
            int column, int row, int height, bool isBlocked,
            int floorId = TileSet.NOT_SET, int blockId = TileSet.NOT_SET, int[] decorationIds = null) {

            Column = column;
            Row = row;
            Height = height;
            IsometricPosition = Point3d.CalcIsometric (
                new Point3d (column, height, row),
                context.CellSize);
            IsBlocked = isBlocked;
            IsSelected = false;

            FloorId = floorId;
            BlockId = blockId;
            DecorationIds = decorationIds;
        }
    }

    public sealed class Map : IRenderable {
        public SceneContext Context { get; private set; }
        public Size2d MapSize { get; private set; }
        public MapCell[, ] Cells { get; private set; }
        public Dictionary<Point2d, MapCell> Hash { get; private set; }

        public Map (Size2d mapSize, SceneContext context, IMapBuilder builder) {
            Context = context;

            MapSize = mapSize;

            Cells = builder.Build (mapSize, context);

            Hash = new Dictionary<Point2d, MapCell> ();

            for (var i = 0; i < MapSize.width; i++) {
                for (var j = 0; j < MapSize.height; j++) {
                    var cell = Cells[i, j];
                    var key = new Point2d (
                        cell.IsometricPosition.x,
                        (cell.IsometricPosition.z - cell.IsometricPosition.y));

                    if (Hash.ContainsKey (key)) {
                        Hash[key] = cell;
                    } else {
                        Hash.Add (key, cell);
                        Console.WriteLine($"hash: {key}");
                    }
                }
            }
        }

        public void Render (IntPtr renderer) {

            if (Context.Viewport == null) {
                throw new InvalidOperationException (@"Viewport is not set");
            }

            //*
            var fromX = 0;
            var fromY = 0;
            var toX = MapSize.width;
            var toY = MapSize.height;
            //*/

            /*
            var fromX = Math.Max (0, Context.Viewport.Position.MapX);
            var fromY = Math.Max (0, Context.Viewport.Position.MapY);
            var toX = Math.Min (MapWidth - 1, Context.Viewport.BottomRight.MapX);
            var toY = Math.Min (MapHeight - 1, Context.Viewport.BottomRight.MapY);
            //*/

            int i, j;

            for (i = fromX; i < toX; i++) {
                for (j = fromY; j < toY; j++) {
                    RenderMapCell (renderer, Cells[i, j]);
                }
            }
        }

        public void RenderMapCell (IntPtr renderer, MapCell cell) {
            Tile tile;
            IntPtr texture = Context.TileSet.Texture;
            int resultIsoX = cell.IsometricPosition.x - Context.Viewport.Position.x + Context.DisplaySize.width / 2;
            int resultIsoY = cell.IsometricPosition.z - Context.Viewport.Position.y;

            if (cell.BlockId != TileSet.NOT_SET && cell.Height > 0) {
                tile = Context.TileSet.Blocks[cell.BlockId];

                for (var i = 0; i < cell.Height; i++) {
                    SdlDrawing.RenderTexture (
                        texture, renderer,
                        resultIsoX - tile.OriginX, resultIsoY - (i + 1) * Context.CellSize.height - tile.OriginY,
                        Context.TileSet.Blocks[cell.BlockId].GetClipRect ());
                }

            }

            if (cell.FloorId != TileSet.NOT_SET) {
                tile = Context.TileSet.Floors[cell.FloorId];

                SdlDrawing.RenderTexture (
                    texture, renderer,
                    resultIsoX - tile.OriginX, resultIsoY - cell.IsometricPosition.y - tile.OriginY,
                    tile.GetClipRect ());
            }

            if (cell.IsSelected) {
                tile = Context.TileSet.UserInterface[0];

                SdlDrawing.RenderTexture (
                    texture, renderer,
                    resultIsoX - tile.OriginX, resultIsoY - cell.IsometricPosition.y - tile.OriginY,
                    tile.GetClipRect ());
            }

            if (!(cell.DecorationIds == null || cell.DecorationIds.Length == 0)) {
                for (var tid = 0; tid < cell.DecorationIds.Length; tid++) {
                    tile = Context.TileSet.Decorations[cell.DecorationIds[tid]];

                    SdlDrawing.RenderTexture (
                        texture, renderer,
                        resultIsoX - tile.OriginX, resultIsoY - cell.IsometricPosition.y - tile.OriginY,
                        tile.GetClipRect ());
                }
            }

        }
    }
}