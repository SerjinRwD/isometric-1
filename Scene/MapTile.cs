namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using isometric_1.Contract;
    using isometric_1.Helpers;
    using isometric_1.ManagedSdl;
    using isometric_1.Types;
    using SDL2;

    public enum MapTileType {
        Floor,
        Wall,
        Slope
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
        public MapPoint MapCoords { get; private set; }
        public MapTileType TileType { get; set; }
        public Direction Orientation { get; set; }
        public AbstractActor Visitor { get; set; }
        public bool IsEmpty { get => TileType != MapTileType.Wall && Visitor == null; }
        public bool IsSelected { get; set; }
        public object Tag { get; set; }

        #region Освещенность
        public MapTileLight Light { get; set; }
        #endregion

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
            MapPoint mapPosition,
            Size3d tileSize,
            MapTileType type,
            ImageTile image,
            ImageTile southImage,
            ImageTile northImage,
            Direction orientation = Direction.W) : this () {

            Image = image;
            WallSouth = southImage;
            WallNorth = northImage;
            var position = mapPosition.ToPoint2d(tileSize) + (Image.RegistrationX, Image.RegistrationY);

            MapCoords = mapPosition;
            IsometricPosition = Compute.Isometric (mapPosition.ToPoint3d (tileSize));
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
            MapCoords = new MapPoint (mx, my);
        }

        public int GetYForActor (AbstractActor actor) {
            var tileSize = SceneContext.Current.Map.TileSize;

            switch (TileType) {

                case MapTileType.Floor:
                    return MapCoords.level * tileSize.height;

                case MapTileType.Slope:
                    var tile = this;//Neighbors[actor.Direction];
                    Point3d position;

                    switch (Orientation) {
                        case Direction.N:
                            position = actor.Direction == Direction.N
                                ? MapCoords.ToPoint3d(tileSize)
                                : MapCoords.ToPoint3d(tileSize) + (tileSize.width, 0, tileSize.length);

                            var rz = actor.Position.z - position.z;
                            return MapCoords.level * tileSize.height - (rz >> 1);

                        case Direction.W:
                            position = actor.Direction == Direction.W
                                ? MapCoords.ToPoint3d(tileSize) + (tileSize.width, 0, tileSize.length)
                                : MapCoords.ToPoint3d(tileSize);

                            var rx = actor.Position.x - position.x;
                            return MapCoords.level * tileSize.height - (rx >> 1);

                        default:
                            return 0;
                    }
                case MapTileType.Wall:
                    return MapCoords.level * tileSize.height;

                default:
                    return 0;
            }
        }

        public void Reset () {
            f = g = 0;
            closed = false;
        }

        public bool IsMath (MapPoint point) {
            return MapCoords.column == point.column && MapCoords.row == point.row;
        }

        public void RecalculateNeighbors () {
            Neighbors = new Dictionary<Direction, MapTile> ();
            Egdes = new Dictionary<Direction, int> ();

            (int x, int y) = MapCoords;
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

            return 1 + Math.Abs (MapCoords.level - neighbor.MapCoords.level) * 2;
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
            int resultIsoX = IsometricPosition.x - viewport.Position.x + (viewport.Size.width >> 1);
            int resultIsoY = IsometricPosition.z - viewport.Position.y;

            if(resultIsoX < 0 ||
               resultIsoY < 0 ||
               resultIsoX > viewport.Size.width || 
               resultIsoY > viewport.Size.height) {
                return;
            } 

            ImageTile imageTile;
            var tileSet = SceneContext.Current.Map.TileSet;
            var tileSize = SceneContext.Current.Map.TileSize;

            var texture = tileSet.Texture;

            texture.ColorMod = Light.modulatedColor; // (Light.intensity, Light.intensity, Light.intensity);(255, 0, 0); // 

            if (WallSouth != null && MapCoords.level > 0) {
                for (var i = 0; i < MapCoords.level; i++) {
                    renderer.RenderTexture (
                        texture,
                        resultIsoX - WallSouth.OriginX, resultIsoY - (i + 1) * tileSize.height - WallSouth.OriginY,
                        WallSouth.GetClipRect ());
                }
            }

            if (WallNorth != null && MapCoords.level > 0) {
                for (var i = 0; i < MapCoords.level; i++) {
                    renderer.RenderTexture (
                        texture,
                        resultIsoX - WallNorth.OriginX, resultIsoY - (i + 1) * tileSize.height - WallNorth.OriginY,
                        WallNorth.GetClipRect ());
                }
            }

            if (Image != null) {

                /* отрисовка тайла */
                renderer.RenderTexture (
                    Image.Texture,
                    resultIsoX - Image.OriginX, resultIsoY - IsometricPosition.y - Image.OriginY,
                    Image.GetClipRect ());

                SceneContext.Current.TileSet.Texture.AlphaMod = 76;

                /* отрисовка граней */
                // грани склона
                if(TileType == MapTileType.Slope)
                {
                    if(Neighbors.ContainsKey(Direction.W) && Neighbors[Direction.W].MapCoords.level <= MapCoords.level)
                    {
                        imageTile = SceneContext.Current.TileSet.Tiles[6];

                        renderer.RenderTexture (
                            SceneContext.Current.TileSet.Texture,
                            resultIsoX - imageTile.OriginX, resultIsoY - IsometricPosition.y - imageTile.OriginY,
                            imageTile.GetClipRect ());
                    }

                    if(Neighbors.ContainsKey(Direction.S) && Neighbors[Direction.S].MapCoords.level <= MapCoords.level)
                    {
                        imageTile = SceneContext.Current.TileSet.Tiles[7];

                        renderer.RenderTexture (
                            SceneContext.Current.TileSet.Texture,
                            resultIsoX - imageTile.OriginX, resultIsoY - IsometricPosition.y - imageTile.OriginY,
                            imageTile.GetClipRect ());
                    }
                }
                // грани плоскости
                else
                {
                    if(Neighbors.ContainsKey(Direction.W) && Neighbors[Direction.W].MapCoords.level < MapCoords.level)
                    {
                        imageTile = SceneContext.Current.TileSet.Tiles[4];

                        renderer.RenderTexture (
                            SceneContext.Current.TileSet.Texture,
                            resultIsoX - imageTile.OriginX, resultIsoY - IsometricPosition.y - imageTile.OriginY,
                            imageTile.GetClipRect ());
                    }

                    if(Neighbors.ContainsKey(Direction.S) && Neighbors[Direction.S].MapCoords.level < MapCoords.level)
                    {
                        imageTile = SceneContext.Current.TileSet.Tiles[5];

                        renderer.RenderTexture (
                            SceneContext.Current.TileSet.Texture,
                            resultIsoX - imageTile.OriginX, resultIsoY - IsometricPosition.y - imageTile.OriginY,
                            imageTile.GetClipRect ());
                    }
                }

                SceneContext.Current.TileSet.Texture.AlphaMod = 255;
            }

            texture.ColorMod = (255, 255, 255);

            if (IsSelected) {
                var id = TileType == MapTileType.Slope
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