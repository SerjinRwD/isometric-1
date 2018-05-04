namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System;
    using isometric_1.Contract;
    using isometric_1.Helpers;
    using isometric_1.Types;

    public class MapTile {
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

        public MapTile (SceneContext context,
            int column, int row, int height, bool isBlocked,
            int floorId = ImageTileSet.NOT_SET, int blockId = ImageTileSet.NOT_SET, int[] decorationIds = null) {

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
}