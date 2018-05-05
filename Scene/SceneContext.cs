namespace isometric_1.Scene {
    using isometric_1.Contract;
    using isometric_1.Types;

    public sealed class SceneContext {
        public Viewport Viewport { get; private set; }
        public ImageTileSet TileSet { get; private set; }
        public Map Map { get; private set; }

        public SceneContext (
            Size2d mapSize,
            Size2d viewportSize,
            ImageTileSet tileSet,
            IMapBuilder builder) {

            TileSet = tileSet;
            Map = new Map (mapSize, builder);
            Viewport = new Viewport(this, 0, 0, viewportSize);
        }
    }
}