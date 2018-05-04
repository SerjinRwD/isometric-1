namespace isometric_1.Scene {
    using isometric_1.Contract;
    using isometric_1.Types;

    public sealed class SceneContext {
        public Size3d CellSize { get; private set; }
        public Size2d DisplaySize { get; private set; }
        public Viewport Viewport { get; private set; }
        public ImageTileSet TileSet { get; private set; }
        public Map Map { get; private set; }

        public SceneContext (Size3d cellSize,
            Size2d mapSize,
            Size2d displaySize,
            ImageTileSet tileSet,
            IMapBuilder builder) {

            CellSize = cellSize;
            DisplaySize = displaySize;
            Viewport = new Viewport(this, 0, 0, 800, 800);
            TileSet = tileSet;
            Map = new Map (mapSize, this, builder);
        }
    }
}