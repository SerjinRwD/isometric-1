namespace isometric_1.Contract {
    using System;
    using ManagedSdl;

    public interface IRenderable {
        void Render (SdlRenderer renderer);
    }
}