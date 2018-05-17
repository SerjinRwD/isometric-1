namespace isometric_1.Scene {
    using isometric_1.Contract;
    using isometric_1.ManagedSdl;
    using isometric_1.Types;

    public enum ActorGenericState {
        Waiting,
        CheckPath,
        MovingToWaypoint
    }

    public abstract class AbstractActor : AbstractSdlEventListener, IUpdateable, IRenderable {
        public ImageTile Image { get; protected set; }
        public Point3d RegistrationPoint { get; protected set; }
        public Point3d IsometricPosition { get; protected set; }

        public string Name { get; protected set; }
        public Point2d PrevMapPosition { get; protected set; }
        public Point2d MapPosition { get; protected set; }
        public Point2d Destination { get; protected set; }
        public Point3d Position { get; protected set; }
        public ActorGenericState GenericState { get; protected set; }
        public int TileId { get; protected set; }

        public virtual void Render (SdlRenderer renderer, Viewport viewport) {

            int resultIsoX = IsometricPosition.x - viewport.Position.x + (viewport.Size.width >> 1) - Image.OriginX;
            int resultIsoY = IsometricPosition.z - IsometricPosition.y - viewport.Position.y - Image.OriginY;

            renderer.RenderTexture (
                Image.Texture,
                resultIsoX, resultIsoY,
                Image.GetClipRect ());
        }

        public virtual void Update () {
            PrevMapPosition = MapPosition;
            SceneContext.Current.Map.Tiles[PrevMapPosition.x, PrevMapPosition.y].Visitor = null;

            MapPosition = new Point2d (Position.x / SceneContext.Current.Map.TileSize.width, Position.z / SceneContext.Current.Map.TileSize.length);
            SceneContext.Current.Map.Tiles[MapPosition.x, MapPosition.y].Visitor = this;

            var tileSize = SceneContext.Current.Map.TileSize;
        }

        public AbstractActor (Point2d mapPosition, ImageTile image) {
            MapPosition = mapPosition;
            Position = new Point3d (mapPosition.x, SceneContext.Current.Map.Tiles[mapPosition.x, mapPosition.y].Level, mapPosition.y, SceneContext.Current.Map.TileSize);
            Image = image;

            var tileSize = SceneContext.Current.Map.TileSize;
        }
    }
}