namespace isometric_1.Helpers {
    using System;
    using SDL2;

    public static class SdlDrawing {
        public static void RenderTexture (IntPtr texture, IntPtr renderer, int x, int y) {
            var dst = new SDL.SDL_Rect ();

            dst.x = x;
            dst.y = y;

            uint format;
            int access;

            SDL.SDL_QueryTexture (texture, out format, out access, out dst.w, out dst.h);
            SDL.SDL_RenderCopy (renderer, texture, IntPtr.Zero, ref dst);
        }

        public static void RenderTexture (IntPtr texture, IntPtr renderer, int x, int y, int w, int h) {
            var dst = new SDL.SDL_Rect () {
                x = x,
                y = y,
                w = w,
                h = h,
            };

            SDL.SDL_RenderCopy (renderer, texture, IntPtr.Zero, ref dst);
        }

        public static void RenderTexture (IntPtr texture, IntPtr renderer, int x, int y, SDL.SDL_Rect clip) {
            var dst = new SDL.SDL_Rect () {
                x = x,
                y = y,
                w = clip.w,
                h = clip.h
            };

            SDL.SDL_RenderCopy (renderer, texture, ref clip, ref dst);
        }

        public static void RenderTexture (IntPtr texture, IntPtr renderer, SDL.SDL_Rect dst) {
            SDL.SDL_RenderCopy (renderer, texture, IntPtr.Zero, ref dst);
        }

        public static void RenderTexture (IntPtr texture, IntPtr renderer, SDL.SDL_Rect dst, SDL.SDL_Rect clip) {
            SDL.SDL_RenderCopy (renderer, texture, ref clip, ref dst);
        }

        public static IntPtr CreateTextureFromText (string text, IntPtr font, SDL.SDL_Color color, IntPtr renderer) {
            var surface = SDL_ttf.TTF_RenderUTF8_Solid (font, text, color);
            var texture = SDL.SDL_CreateTextureFromSurface (renderer, surface);

            return texture;
        }
    }
}