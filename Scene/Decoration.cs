namespace isometric_1.Scene {
    using isometric_1.Contract;
    using isometric_1.ManagedSdl;
    using isometric_1.Types;

    public class Decoration : IRenderable {
        public ImageTile Image { get; protected set; }
        public MapPoint MapPosition { get; protected set; }
        public Point3d RegistrationPoint { get; protected set; }
        public Point3d IsometricPosition { get; protected set; }

        public Decoration (MapPoint mapPosition, ImageTile image) {
            Image = image;
            var position = mapPosition.ToPoint3d(SceneContext.Current.Map.TileSize);
            
            MapPosition = mapPosition;
            IsometricPosition = Compute.Isometric (position);
            RegistrationPoint = Compute.Isometric (position + (Image.RegistrationX, 0, Image.RegistrationY));
        }

        public void Render (SdlRenderer renderer, Viewport viewport) {

            int resultIsoX = IsometricPosition.x - viewport.Position.x + (viewport.Size.width >> 1) - Image.OriginX;
            int resultIsoY = IsometricPosition.z - IsometricPosition.y - viewport.Position.y - Image.OriginY;

            if(resultIsoX < 0 ||
               resultIsoY < 0 ||
               resultIsoX > viewport.Size.width || 
               resultIsoY > viewport.Size.height) {
                return;
            }

            var light = SceneContext.Current.Map.Tiles[MapPosition.column, MapPosition.row].Light;

            Image.Texture.ColorMod = light.modulatedColor; // (light.intensity, light.intensity, light.intensity);

            renderer.RenderTexture (
                Image.Texture,
                resultIsoX, resultIsoY,
                Image.GetClipRect ());

            Image.Texture.ColorMod = (255, 255, 255);
        }
    }
}