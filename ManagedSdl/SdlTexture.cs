namespace isometric_1.ManagedSdl {
    using System;
    using SDL2;

    public class SdlTexture : IDisposable {
        public IntPtr Pointer { get; private set; }

        public byte AlphaMod
        {
            get
            {
                byte a;

                if (SDL.SDL_GetTextureAlphaMod(Pointer, out a) < 0) {
                    throw new SdlException(nameof(ColorMod));
                }

                return a;
            }

            set => SDL.SDL_SetTextureAlphaMod(Pointer, value);
        }

        public (byte, byte, byte) ColorMod
        {
            get
            {
                byte r, g, b;

                if (SDL.SDL_GetTextureColorMod(Pointer, out r, out g, out b) < 0) {
                    throw new SdlException(nameof(ColorMod));
                }

                return (r, g, b);
            }

            set => SDL.SDL_SetTextureColorMod(Pointer, value.Item1, value.Item2, value.Item3);
        }

        public SDL.SDL_BlendMode BlendMode
        {
            get
            {
                SDL.SDL_BlendMode blend;
                
                if (SDL.SDL_GetTextureBlendMode(Pointer, out blend) < 0) {
                    throw new SdlException(nameof(BlendMode));
                }

                return blend;
            }

            set => SDL.SDL_SetTextureBlendMode(Pointer, value);
        }

        private SdlTexture () { }

        public void Dispose () {
            Dispose (true);
            //GC.SuppressFinalize (this);
        }

        protected virtual void Dispose (bool disposing) {
            if (disposing) {
                if (Pointer != IntPtr.Zero) {
                    SDL.SDL_DestroyTexture (Pointer);
                }
            }
        }

        public static SdlTexture LoadFromBitmap (string path, SdlRenderer renderer) {
            var surfacePtr = SDL.SDL_LoadBMP (path);

            if (surfacePtr == IntPtr.Zero) {
                throw new SdlException (nameof (SDL.SDL_LoadBMP));
            }

            IntPtr format;

            // ммм, душок Си
            unsafe {
                format = ((SDL.SDL_Surface * ) (surfacePtr.ToPointer ())) -> format;
            }

            SDL.SDL_SetColorKey (surfacePtr, (int) SDL.SDL_bool.SDL_TRUE, SDL.SDL_MapRGB (format, 255, 0, 255));

            var texturePtr = SDL.SDL_CreateTextureFromSurface (renderer.Pointer, surfacePtr);
            SDL.SDL_FreeSurface (surfacePtr);

            if (texturePtr == IntPtr.Zero) {
                throw new SdlException (nameof (SDL.SDL_CreateTextureFromSurface));
            }

            return new SdlTexture {
                Pointer = texturePtr
            };
        }

        public static SdlTexture LoadFromImage (string path, SdlRenderer renderer) {
            var texturePtr = SDL_image.IMG_LoadTexture (renderer.Pointer, path);

            if (texturePtr == IntPtr.Zero) {
                throw new SdlException (nameof (SDL_image.IMG_LoadTexture));
            }

            return new SdlTexture {
                Pointer = texturePtr
            };
        }

        public static SdlTexture CreateFromText (string text, SdlRenderer renderer, SdlFont font, SDL.SDL_Color color) {

            var surfacePtr = SDL_ttf.TTF_RenderUTF8_Solid (font.Pointer, text, color);

            if (surfacePtr == IntPtr.Zero) {
                throw new SdlException (nameof (SDL_ttf.TTF_RenderUTF8_Solid));
            }

            var texturePtr = SDL.SDL_CreateTextureFromSurface (renderer.Pointer, surfacePtr);

            if (texturePtr == IntPtr.Zero) {
                throw new SdlException (nameof (SDL.SDL_CreateTextureFromSurface));
            }

            SDL.SDL_FreeSurface (surfacePtr);

            return new SdlTexture {
                Pointer = texturePtr
            };
        }
    }
}