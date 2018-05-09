namespace isometric_1.Scene {
    using isometric_1.ManagedSdl;
    
    public abstract class AbstractSdlEventListener : ISdlEventListener {
        public virtual void OnKeyDown (object sender, SdlKeyboardEventArgs args) { }
        public virtual void OnKeyUp (object sender, SdlKeyboardEventArgs args) { }
        public virtual void OnMouseMotion (object sender, SdlMouseMotionEventArgs args) { }
        public virtual void OnMouseDown (object sender, SdlMouseButtonEventArgs args) { }
        public virtual void OnMouseUp (object sender, SdlMouseButtonEventArgs args) { }
    }
}