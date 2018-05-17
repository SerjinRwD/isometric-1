namespace isometric_1.Scene {
    using isometric_1.Contract;
    using isometric_1.ManagedSdl;
    using isometric_1.Types;

    public class Decoration : IRenderable {
        public ImageTile Image { get; protected set; }
        public Point3d RegistrationPoint { get; protected set; }
        public Point3d IsometricPosition { get; protected set; }

        public Decoration (Point3d position, ImageTile image) {
            Image = image;
            IsometricPosition = Compute.Isometric (position);
            RegistrationPoint = Compute.Isometric (position + (Image.RegistrationX, 0, Image.RegistrationY));
        }

        public void Render (SdlRenderer renderer, Viewport viewport) {
            int resultIsoX = IsometricPosition.x - viewport.Position.x + (viewport.Size.width >> 1) - Image.OriginX;
            int resultIsoY = IsometricPosition.z - IsometricPosition.y - viewport.Position.y - Image.OriginY;

            renderer.RenderTexture (
                Image.Texture,
                resultIsoX, resultIsoY,
                Image.GetClipRect ());
        }
    }
}