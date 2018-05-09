namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System;

    using isometric_1.Contract;
    using isometric_1.Helpers;
    using isometric_1.Types;

    public enum MapTileOrientation {
        XY,
        ZY
    }
    public enum MapTileType {
        Floor,
        Wall,
        Ramp
    }

    public class MapTile {
        public Point3d IsometricPosition { get; private set; }
        public Point2d MapPosition { get; private set; }
        public int Level { get; set; }
        public MapTileType Type { get; set; }
        public MapTileOrientation Orientation { get; set; }
        public AbstractActor Visitor { get; set; }
        public bool IsEmpty { get => Type != MapTileType.Wall && Visitor == null; }
        public bool IsSelected { get; set; }
        public int FloorId { get; private set; } = ImageTile.NOT_SET;
        public int BlockId { get; private set; } = ImageTile.NOT_SET;
        public int[] DecorationIds { get; private set; }
        public object Tag { get; set; }

        /// <summary>
        /// Конструктор для ячейки изометрической карты
        /// </summary>
        /// <param name="mapPosition"></param>
        /// <param name="cellSize"></param>
        /// <param name="type"></param>
        /// <param name="orientation"></param>
        /// <param name="floorId"></param>
        /// <param name="blockId"></param>
        /// <param name="decorationIds"></param>
        public MapTile (
            Point2d mapPosition,
            int level,
            Size3d cellSize,
            MapTileType type,
            MapTileOrientation orientation = MapTileOrientation.ZY,
            int floorId = ImageTile.NOT_SET,
            int blockId = ImageTile.NOT_SET,
            int[] decorationIds = null) {

            MapPosition = mapPosition;
            Level = level;
            IsometricPosition = Point3d.CalcIsometric (mapPosition.x, Level * cellSize.height, mapPosition.y, cellSize);
            Type = type;
            Orientation = orientation;
            IsSelected = false;

            FloorId = floorId;
            BlockId = blockId;
            DecorationIds = decorationIds;
        }

        /// <summary>
        /// Конструктор для ячейки упрощённой двумерной карты
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        /// <param name="cellSize"></param>
        public MapTile (int mx, int my) {
            MapPosition = new Point2d (mx, my);
        }
    }
}