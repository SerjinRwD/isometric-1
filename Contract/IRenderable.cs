namespace isometric_1.Contract {
    using System;
    using isometric_1.Scene;
    using ManagedSdl;

    public interface IRenderable {
        void Render (SdlRenderer renderer, Viewport viewport);
    }
}