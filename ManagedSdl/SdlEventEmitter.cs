namespace isometric_1.ManagedSdl {
    using System;

    using SDL2;

    public class SdlKeyboardEventArgs : EventArgs {
        public SDL.SDL_Keycode Keycode { get; private set; }

        public SdlKeyboardEventArgs (SDL.SDL_Keycode keycode) {
            Keycode = keycode;
        }
    }

    public class SdlMouseMotionEventArgs : EventArgs {
        public SDL.SDL_MouseMotionEvent MouseMotionEvent { get; private set; }

        public SdlMouseMotionEventArgs (SDL.SDL_MouseMotionEvent mouseMotionEvent) {
            MouseMotionEvent = mouseMotionEvent;
        }
    }

    public class SdlMouseButtonEventArgs : EventArgs {
        public SDL.SDL_MouseButtonEvent MouseButtonEvent { get; private set; }

        public SdlMouseButtonEventArgs (SDL.SDL_MouseButtonEvent mouseButtonEvent) {
            MouseButtonEvent = mouseButtonEvent;
        }
    }

    public delegate void SdlKeyboardEventHandler (object sender, SdlKeyboardEventArgs args);

    public delegate void SdlMouseMotionEventHandler (object sender, SdlMouseMotionEventArgs args);
    public delegate void SdlMouseButtonEventHandler (object sender, SdlMouseButtonEventArgs args);

    public class SdlEventEmitter {
        public event SdlKeyboardEventHandler KeyDown;
        public event SdlKeyboardEventHandler KeyUp;
        public event SdlMouseMotionEventHandler MouseMotion;
        public event SdlMouseButtonEventHandler MouseDown;
        public event SdlMouseButtonEventHandler MouseUp;
        public event EventHandler Quit;

        public void Poll () {
            SDL.SDL_Event e;

            while (SDL.SDL_PollEvent (out e) > 0) {

                switch (e.type) {

                    case SDL.SDL_EventType.SDL_QUIT:
                        Quit?.Invoke (this, null);
                        break;

                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        KeyDown?.Invoke (this, new SdlKeyboardEventArgs (e.key.keysym.sym));
                        break;

                    case SDL.SDL_EventType.SDL_KEYUP:
                        KeyUp?.Invoke (this, new SdlKeyboardEventArgs (e.key.keysym.sym));
                        break;

                    case SDL.SDL_EventType.SDL_MOUSEMOTION:
                        MouseMotion?.Invoke (this, new SdlMouseMotionEventArgs (e.motion));
                        break;

                    case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        MouseDown?.Invoke (this, new SdlMouseButtonEventArgs (e.button));
                        break;

                    case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                        MouseUp?.Invoke (this, new SdlMouseButtonEventArgs (e.button));
                        break;

                    default:
                        break;
                }
            }
        }
    }
}