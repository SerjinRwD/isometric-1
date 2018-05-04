namespace isometric_1.ManagedSdl {
    using System;
    using SDL2;

    public class SdlFont : IDisposable {
        public IntPtr Pointer { get; private set; }
        public int Size { get; private set; }
        public string FileName { get; private set; }

        private SdlFont () { }

        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        protected virtual void Dispose (bool disposing) {
            if (disposing) {
                if (Pointer != IntPtr.Zero) {
                    SDL_ttf.TTF_CloseFont (Pointer);
                }
            }
        }

        public static SdlFont LoadFromTTF (string path, int size) {
            var fontPtr = SDL_ttf.TTF_OpenFont (path, size);

            return new SdlFont {
                Pointer = fontPtr,
                Size = size,
                FileName = path,
            };
        }
    }
}