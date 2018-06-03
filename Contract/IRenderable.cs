namespace isometric_1.Contract {
    using System;

    using isometric_1.Content;
    using isometric_1.Scene;
    using isometric_1.Types;

    using ManagedSdl;

    public interface IRenderable {
        Image Image { get; }
        Point3d RegistrationPoint { get; }
        Point3d IsometricPosition { get; }
        void Render (SdlRenderer renderer, Viewport viewport);
    }
}