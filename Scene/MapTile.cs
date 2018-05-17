namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using isometric_1.Contract;
    using isometric_1.Helpers;
    using isometric_1.ManagedSdl;
    using isometric_1.Types;

    public enum MapTileType {
        Floor,
        Wall,
        Ramp
    }

    public class MapTile : IRenderable {

        #region Данные для поисковика
        private static int _lastIndex;
        private int _id;
        public int Id { get => _id; }
        public int f;
        public int g;
        public bool closed;
        #endregion

        public ImageTile Image { get; protected set; }
        public Point3d RegistrationPoint { get; protected set; }
        public Point3d IsometricPosition { get; protected set; }
        
        public ImageTile WallSouth { get; protected set; }
        public ImageTile WallNorth { get; protected set; }

        public Dictionary<Direction, int> Egdes { get; private set; }
        public Dictionary<Direction, MapTile> Neighbors { get; private set; }
        public Point2d MapPosition { get; private set; }
        public int Level { get; set; }
        public MapTileType TileType { get; set; }
        public Direction Orientation { get; set; }
        public AbstractActor Visitor { get; set; }
        public bool IsEmpty { get => TileType != MapTileType.Wall && Visitor == null; }
        public bool IsSelected { get; set; }
        public object Tag { get; set; }

        private MapTile () {
            _id = _lastIndex++;
        }

        /// <summary>
        /// Конструктор для ячейки изометрической карты
        /// </summary>
        /// <param name="mapPosition"></param>
        /// <param name="tileSize"></param>
        /// <param name="type"></param>
        /// <param name="orientation"></param>
        /// <param name="floorId"></param>
        /// <param name="blockId"></param>
        /// <param name="decorationIds"></param>
        public MapTile (
            Point2d mapPosition,
            int level,
            Size3d tileSize,
            MapTileType type,
            ImageTile image,
            ImageTile southImage,
            ImageTile northImage,
            Direction orientation = Direction.W) : this () {

            Image = image;
            WallSouth = southImage;
            WallNorth = northImage;
            var position = new Point2d (mapPosition.x * tileSize.width + Image.RegistrationX, mapPosition.y * tileSize.length + Image.RegistrationY - Level * tileSize.height);

            MapPosition = mapPosition;
            Level = level;
            IsometricPosition = Compute.Isometric (mapPosition.x, Level, mapPosition.y, tileSize);
            RegistrationPoint = Compute.Isometric (position);
            TileType = type;
            Orientation = orientation;
            IsSelected = false;
        }

        /// <summary>
        /// Конструктор для ячейки упрощённой двумерной карты
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        /// <param name="cellSize"></param>
        public MapTile (int mx, int my) : this () {
            MapPosition = new Point2d (mx, my);
        }

        public int GetYForActor (AbstractActor actor) {
            var tileSize = SceneContext.Current.Map.TileSize;
            var position = new Point3d (MapPosition.x, Level, MapPosition.y, tileSize) + (tileSize.width, 0, tileSize.length);

            switch (TileType) {

                case MapTileType.Floor:
                    return position.y;

                case MapTileType.Ramp:
                    switch (Orientation) {
                        case Direction.N:
                            var rz = actor.Position.z - position.z;
                            return position.y - (rz >> 1);

                        case Direction.W:
                            var rx = actor.Position.x - position.x;
                            return position.y - (rx >> 1);

                        default:
                            return 0;
                    }
                case MapTileType.Wall:
                    return position.y;

                default:
                    return 0;
            }
        }

        public void Reset () {
            f = g = 0;
            closed = false;
        }

        public bool IsMath (Point2d point) {
            return MapPosition.x == point.x && MapPosition.y == point.y;
        }

        public void RecalculateNeighbors () {
            Neighbors = new Dictionary<Direction, MapTile> ();
            Egdes = new Dictionary<Direction, int> ();

            (int x, int y) = MapPosition;
            int rx, ry;

            for (var i = -1; i < 2; i++) {
                for (var j = -1; j < 2; j++) {
                    if (i == 0 && j == 0) {
                        continue;
                    }

                    rx = x + i;
                    ry = y + j;

                    if (rx >= 0 && rx < SceneContext.Current.Map.MapSize.width && ry >= 0 && ry < SceneContext.Current.Map.MapSize.height) {
                        var key = Compute.DirectionOfVector (i, j);
                        var tile = SceneContext.Current.Map.Tiles[rx, ry];

                        Neighbors.Add (key, tile);
                        Egdes.Add (key, CalculateEdge (tile));
                    }
                }
            }
        }

        public int CalculateEdge (MapTile neighbor) {

            if (neighbor.TileType == MapTileType.Wall) {
                return 255;
            }

            return 1 + Math.Abs (Level - neighbor.Level) * 2;
        }

        public void RecalculateEdges () {
            foreach (var key in Neighbors.Keys) {
                Egdes[key] = CalculateEdge (Neighbors[key]);
            }
        }

        public void ForceNeighborsRecalculateEdges () {
            foreach (var key in Neighbors.Keys) {
                Neighbors[key].RecalculateEdges ();
            }
        }

        public void Render (SdlRenderer renderer, Viewport viewport) {
            ImageTile imageTile;
            var tileSet = SceneContext.Current.Map.TileSet;
            var tileSize = SceneContext.Current.Map.TileSize;

            var texture = tileSet.Texture;
            int resultIsoX = IsometricPosition.x - viewport.Position.x + (viewport.Size.width >> 1);
            int resultIsoY = IsometricPosition.z - viewport.Position.y;

            if (WallSouth != null && Level > 0) {
                for (var i = 0; i < Level; i++) {
                    renderer.RenderTexture (
                        texture,
                        resultIsoX - WallSouth.OriginX, resultIsoY - (i + 1) * tileSize.height - WallSouth.OriginY,
                        WallSouth.GetClipRect ());
                }
            }

            if (WallNorth != null && Level > 0) {
                for (var i = 0; i < Level; i++) {
                    renderer.RenderTexture (
                        texture,
                        resultIsoX - WallNorth.OriginX, resultIsoY - (i + 1) * tileSize.height - WallNorth.OriginY,
                        WallNorth.GetClipRect ());
                }
            }

            if (Image != null) {

                renderer.RenderTexture (
                    Image.Texture,
                    resultIsoX - Image.OriginX, resultIsoY - IsometricPosition.y - Image.OriginY,
                    Image.GetClipRect ());
            }

            if (IsSelected) {
                var id = TileType == MapTileType.Ramp
                    ? Orientation == Direction.N
                        ? 2
                        : 3
                    : 1;
                imageTile = SceneContext.Current.TileSet.Tiles[id];
                
                renderer.RenderTexture (
                    SceneContext.Current.TileSet.Texture,
                    resultIsoX - imageTile.OriginX, resultIsoY - IsometricPosition.y - imageTile.OriginY,
                    imageTile.GetClipRect ());
            }
        }
    }
}