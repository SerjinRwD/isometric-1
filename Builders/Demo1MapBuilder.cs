namespace isometric_1.Builders {
    using System;
    using isometric_1.Contract;
    using isometric_1.Scene;
    using isometric_1.Types;

    public class Demo1MapBuilder : IMapBuilder {
        public MapCell[, ] Build (Size2d mapSize, SceneContext context) {
            var _cells = new MapCell[mapSize.width, mapSize.height];

            var t1floorId = 0;
            var t1decorations = new int[] { 0 };

            var t2floorId = 1;
            var t2decorations = new int[] { 1 };

            for (var i = 0; i < mapSize.width; i++) {
                for (var j = 0; j < mapSize.height; j++) {
                    _cells[i, j] = (i + j) % 2 == 0 ?
                        new MapCell (context, i, j, 0, false, floorId: t1floorId, decorationIds: t1decorations) :
                        new MapCell (context, i, j, 0, false, floorId: t2floorId, decorationIds: t2decorations);
                }
            }

            return _cells;
        }
    }
}