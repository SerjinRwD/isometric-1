namespace isometric_1.Scene {
    using isometric_1.Contract;
    using isometric_1.ManagedSdl;
    using isometric_1.Types;

    public enum ActorGenericState {
        Waiting,
        Moving
    }

    public abstract class AbstractActor : AbstractSdlEventListener, IUpdateable, IRenderable {
        public string Name { get; protected set; }
        public SceneContext Scene { get; protected set; }
        public Point2d PrevMapPosition { get; protected set; }
        public Point2d MapPosition { get; protected set; }
        public Point2d Waypoint { get; protected set; }
        public Point3d Position { get; protected set; }
        public ActorGenericState GenericState { get; protected set; }
        public int TileId { get; protected set; }

        public virtual void Render (SdlRenderer renderer, Viewport viewport) {
            var texture = Scene.TileSet.Texture;
            var imageTile = Scene.TileSet.Tiles[TileId];
            var isoPosition = Point3d.CalcIsometric (Position);
            int resultIsoX = isoPosition.x - viewport.Position.x + (viewport.Size.width >> 1) - imageTile.OriginX;
            int resultIsoY = isoPosition.z - isoPosition.y - viewport.Position.y - imageTile.OriginY;

            renderer.RenderTexture (
                texture,
                resultIsoX, resultIsoY,
                imageTile.GetClipRect ());
        }

        public virtual void Update () {
            PrevMapPosition = MapPosition;
            Scene.Map.Tiles[PrevMapPosition.x, PrevMapPosition.y].Visitor = null;

            MapPosition = new Point2d (Position.x / Scene.Map.TileSize.width, Position.z / Scene.Map.TileSize.length);
            Scene.Map.Tiles[MapPosition.x, MapPosition.y].Visitor = this;
        }

        public AbstractActor (SceneContext scene, Point2d mapPosition, int tileId) {
            Scene = scene;
            MapPosition = mapPosition;
            Position = new Point3d (mapPosition.x, Scene.Map.Tiles[mapPosition.x, mapPosition.y].Level, mapPosition.y, Scene.Map.TileSize);
            TileId = tileId;
        }
    }
}