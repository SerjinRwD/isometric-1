namespace isometric_1.Builders {
    using System;
    using isometric_1.Contract;
    using isometric_1.Scene;
    using isometric_1.Types;

    public class PyramidMapBuilder : IMapBuilder {
        public MapCell[, ] Build (Size2d mapSize, SceneContext context) {
            var _cells = new MapCell[mapSize.width, mapSize.height];

            var t1floorId = 0;
            var t1blockId = 0;
            var t1decorations = new int[] { 0 };

            for (var i = 0; i < mapSize.width; i++) {
                for (var j = 0; j < mapSize.height; j++) {
                    _cells[i, j] = new MapCell (context, i, j, (i + j) % 3, false, floorId : t1floorId, blockId : t1blockId, decorationIds : t1decorations);
                     // new MapCell (context, i, j, (i + j) % 11 == 0 ? 0 : 3, false, floorId : t1floorId, blockId : t1blockId, decorationIds : t1decorations);
                }
            }

            return _cells;
        }
    }
}