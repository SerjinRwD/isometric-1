namespace isometric_1.Scene {
    using isometric_1.Content;
    using isometric_1.Contract;
    using isometric_1.ManagedSdl;
    using isometric_1.Types;

    public enum ActorGenericState {
        Waiting,
        CheckPath,
        MovingToWaypoint
    }

    public abstract class AbstractActor : AbstractSdlEventListener, IUpdateable, IRenderable {
        public Image Image { get; protected set; }
        public Point3d RegistrationPoint { get; protected set; }
        public Point3d IsometricPosition { get; protected set; }

        public string Name { get; protected set; }
        public MapPoint PrevMapPosition { get; protected set; }
        public MapPoint MapPosition { get; protected set; }
        public MapPoint Destination { get; protected set; }
        public Point3d Position { get; protected set; }
        public Direction Direction { get; protected set; }
        public ActorGenericState GenericState { get; protected set; }
        public int TileId { get; protected set; }

        public virtual void Render (SdlRenderer renderer, Viewport viewport) {

            int resultIsoX = IsometricPosition.x - viewport.Position.x + (viewport.Size.width >> 1) - Image.OriginX;
            int resultIsoY = IsometricPosition.z - IsometricPosition.y - viewport.Position.y - Image.OriginY;

            if(resultIsoX < 0 ||
               resultIsoY < 0 ||
               resultIsoX > viewport.Size.width || 
               resultIsoY > viewport.Size.height) {
                return;
            }

            Image.Texture.ColorMod = SceneContext.Current.Map.Tiles[MapPosition.column, MapPosition.row].ModulatedColor;

            renderer.RenderTexture (
                Image.Texture,
                resultIsoX, resultIsoY,
                Image.GetClipRect ());

            Image.Texture.ColorMod = SdlColorFactory.White;
        }

        public virtual void Update () {
            PrevMapPosition = MapPosition;
            SceneContext.Current.Map.Tiles[PrevMapPosition.column, PrevMapPosition.row].Visitor = null;

            MapPosition = MapPoint.FromPoint3d (Position, SceneContext.Current.Map.TileSize);
            SceneContext.Current.Map.Tiles[MapPosition.column, MapPosition.row].Visitor = this;

            var tileSize = SceneContext.Current.Map.TileSize;
        }

        public AbstractActor (MapPoint mapPosition, Image image) {
            MapPosition = mapPosition;
            Position = mapPosition.ToPoint3d (SceneContext.Current.Map.TileSize);
            Image = image;

            var tileSize = SceneContext.Current.Map.TileSize;
        }
    }
}