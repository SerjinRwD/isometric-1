namespace isometric_1.SdlProgram {
    using System;
    using isometric_1.ManagedSdl;

    using SDL2;

    public abstract class AbstractSdlProgram : ISdlProgram {
        private SdlRenderer _renderer;
        private SdlWindow _window;

        public SdlRenderer Renderer { get => _renderer; }
        public SdlWindow Window { get => _window; }

        public virtual void Execute () {
            try {
                Init ();

                Run ();
            } catch (Exception ex) {
                Console.WriteLine ($"Exception were thrown. Message: {ex.Message}. Stack Trace: {ex.StackTrace}");
            } finally {
                Quit ();
            }
        }

        public virtual void Init () {
            if (SDL.SDL_Init (SDL.SDL_INIT_VIDEO) != 0) {
                throw new SdlException (nameof (SDL.SDL_Init));
            }

            if (SDL_ttf.TTF_Init () != 0) {
                throw new SdlException (nameof (SDL_ttf.TTF_Init));
            }

            SDL.SDL_Rect display;

            SDL.SDL_GetDisplayBounds (0, out display);

            _window = SdlWindow.Create ("Isometrics 1", 0, 0, display.w, display.h, SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP); // .SDL_WINDOW_SHOWN); //

            _renderer = SdlRenderer.Create (
                _window, -1,
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        }
        public abstract void Run ();
        public virtual void Quit () {
            SDL_ttf.TTF_Quit ();
            SDL.SDL_Quit ();
        }
    }
}