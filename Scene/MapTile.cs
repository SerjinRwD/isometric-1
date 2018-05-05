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
        public Point3d MapPosition { get; private set; }
        public MapTileType Type { get; private set; }
        public MapTileOrientation Orientation { get; private set; }
        public AbstractActor Visitor { get; set; }
        public bool IsEmpty { get => Type != MapTileType.Wall && Visitor == null; }
        public bool IsSelected { get; set; }
        public int FloorId { get; private set; }
        public int BlockId { get; private set; }
        public int[] DecorationIds { get; private set; }
        public object Tag { get; set; }

        public MapTile (
            Point3d mapPosition,
            Size3d cellSize,
            MapTileType type,
            MapTileOrientation orientation = MapTileOrientation.ZY,
            int floorId = ImageTile.NOT_SET,
            int blockId = ImageTile.NOT_SET,
            int[] decorationIds = null) {

            MapPosition = mapPosition;
            IsometricPosition = Point3d.CalcIsometric (mapPosition, cellSize);
            Type = type;
            Orientation = orientation;
            IsSelected = false;

            FloorId = floorId;
            BlockId = blockId;
            DecorationIds = decorationIds;
        }
    }
}