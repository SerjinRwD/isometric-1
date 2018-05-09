namespace isometric_1.ManagedSdl {
    using System;
    using SDL2;

    public interface ISdlEventListener {
        void OnKeyDown (object sender, SdlKeyboardEventArgs args);
        void OnKeyUp (object sender, SdlKeyboardEventArgs args);
        void OnMouseMotion (object sender, SdlMouseMotionEventArgs args);
        void OnMouseDown (object sender, SdlMouseButtonEventArgs args);
        void OnMouseUp (object sender, SdlMouseButtonEventArgs args);
    }
}