namespace isometric_1.ManagedSdl {
    using System;
    using isometric_1.Types;
    using SDL2;

    public class SdlWindow : IDisposable {
        public IntPtr Pointer { get; private set; }
        public Point2d Position { get; private set; }
        public Size2d Size { get; private set; }
        public string Title { get; private set; }
        public SDL.SDL_WindowFlags Flags { get; private set; }

        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        protected virtual void Dispose (bool disposing) {
            if (disposing) {
                if (Pointer != IntPtr.Zero) {
                    SDL.SDL_DestroyWindow (Pointer);
                }
            }
        }

        public static SdlWindow Create (string title, int x, int y, int w, int h, SDL.SDL_WindowFlags flags) {
            var windowPtr = SDL.SDL_CreateWindow (title, x, y, w, h, flags);

            if(windowPtr == IntPtr.Zero) {
                throw new SdlException(nameof(SDL.SDL_CreateWindow));
            }

            return new SdlWindow {
                Pointer = windowPtr,
                Title = title,
                Position = new Point2d(x, y),
                Size = new Size2d(w, h),
                Flags = flags
            };
        }
    }
}