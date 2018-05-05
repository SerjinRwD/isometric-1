namespace isometric_1.Scene {
    using isometric_1.Contract;
    using isometric_1.ManagedSdl;
    using isometric_1.Types;

    public enum ActorGenericState {
        Waiting,
        Moving
    }

    public abstract class AbstractActor : IUpdateable, IRenderable, ISdlEventListener {
        public string Name { get; private set; }
        public SceneContext Scene { get; private set; }
        public Point3d MapPosition { get; private set; }
        public ActorGenericState GenericState { get; private set; }
        public int TileId { get; private set; }

        public virtual void OnKeyDown (object sender, SdlKeyboardEventArgs args) { }

        public virtual void OnKeyUp (object sender, SdlKeyboardEventArgs args) { }

        public virtual void OnMouseMotion (object sender, SdlMouseMotionEventArgs args) { }

        public virtual void Render (SdlRenderer renderer, Viewport viewport) {
            var texture = Scene.TileSet.Texture;
            var imageTile = Scene.TileSet.Tiles[TileId];
            var isoPosition = Point3d.CalcIsometric(MapPosition, Scene.Map.TileSize);
            int resultIsoX = isoPosition.x - viewport.Position.x + (viewport.Size.width >> 1) - imageTile.OriginX;
            int resultIsoY = isoPosition.z - isoPosition.y - viewport.Position.y - imageTile.OriginY;

            renderer.RenderTexture (
                texture,
                resultIsoX, resultIsoY,
                imageTile.GetClipRect ());
        }

        public virtual void Update () { }
    }
}