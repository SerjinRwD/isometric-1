namespace isometric_1.ManagedSdl {
    using System;
    using SDL2;

    public interface ISdlEventListener {
        void OnKeyDown (object sender, SdlKeyboardEventArgs args);
        void OnKeyUp (object sender, SdlKeyboardEventArgs args);
        void OnMouseMotion (object sender, SdlMouseMotionEventArgs args);
    }
}