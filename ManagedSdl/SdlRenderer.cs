namespace isometric_1.ManagedSdl {
    using System;

    using isometric_1.Types;

    using SDL2;

    public class SdlRenderer : IDisposable {
        public IntPtr Pointer { get; private set; }

        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        protected virtual void Dispose (bool disposing) {
            if (disposing) {
                if (Pointer != IntPtr.Zero) {
                    SDL.SDL_DestroyRenderer (Pointer);
                }
            }
        }

        public static SdlRenderer Create (SdlWindow window, int index, SDL.SDL_RendererFlags flags) {
            var rendererPtr = SDL.SDL_CreateRenderer (window.Pointer, index, flags);

            return new SdlRenderer {
                Pointer = rendererPtr
            };
        }

        public void Clear () {
            SDL.SDL_RenderClear (Pointer);
        }

        public void Present () {
            SDL.SDL_RenderPresent (Pointer);
        }

        public void DrawLine (int x1, int y1, int x2, int y2) {
            SDL.SDL_RenderDrawLine (Pointer, x1, y1, x2, y2);
        }
        public void DrawLine (Point2d p1, Point2d p2) {
            SDL.SDL_RenderDrawLine (Pointer, p1.x, p1.y, p2.x, p2.y);
        }

        internal void DrawRect (int x, int y, int w, int h, bool fill) {
            SDL.SDL_Rect rect;
            rect.x = x;
            rect.y = y;
            rect.w = w;
            rect.h = h;

            if (fill) {
                SDL.SDL_RenderFillRect(Pointer, ref rect);
            } else {
                SDL.SDL_RenderDrawRect (Pointer, ref rect);
            }
        }

        public void RenderTexture (SdlTexture texture, int x, int y) {
            var dst = new SDL.SDL_Rect ();

            dst.x = x;
            dst.y = y;

            uint format;
            int access;

            SDL.SDL_QueryTexture (texture.Pointer, out format, out access, out dst.w, out dst.h);
            SDL.SDL_RenderCopy (Pointer, texture.Pointer, IntPtr.Zero, ref dst);
        }
        public void RenderTexture (SdlTexture texture, int x, int y, int w, int h) {
            var dst = new SDL.SDL_Rect () {
                x = x,
                y = y,
                w = w,
                h = h,
            };

            SDL.SDL_RenderCopy (Pointer, texture.Pointer, IntPtr.Zero, ref dst);
        }
        public void RenderTexture (SdlTexture texture, int x, int y, SDL.SDL_Rect clip) {
            var dst = new SDL.SDL_Rect () {
                x = x,
                y = y,
                w = clip.w,
                h = clip.h
            };

            SDL.SDL_RenderCopy (Pointer, texture.Pointer, ref clip, ref dst);
        }
        public void RenderTexture (SdlTexture texture, SDL.SDL_Rect dst) {
            SDL.SDL_RenderCopy (Pointer, texture.Pointer, IntPtr.Zero, ref dst);
        }
        public void RenderTexture (SdlTexture texture, SDL.SDL_Rect dst, SDL.SDL_Rect clip) {
            SDL.SDL_RenderCopy (Pointer, texture.Pointer, ref clip, ref dst);
        }
        public void SetDrawColor (byte r, byte g, byte b, byte a) {
            SDL.SDL_SetRenderDrawColor (Pointer, r, g, b, a);
        }
        public void SetDrawColor (byte r, byte g, byte b) {
            SetDrawColor (r, g, b, 255);
        }
        public void SetDrawColor (SDL.SDL_Color color) {
            SDL.SDL_SetRenderDrawColor (Pointer, color.r, color.g, color.b, color.a);
        }

        public void DrawPoint (int x, int y) {
            SDL.SDL_RenderDrawPoint (Pointer, x, y);
        }
        public void DrawPoint (Point2d point) {
            SDL.SDL_RenderDrawPoint (Pointer, point.x, point.y);
        }
        public void DrawPoint (Point3d point) {
            DrawPoint (point.ToPoint2d ());
        }

        public void DrawText(string text, int x, int y, SdlFont font) {
            byte r, g, b, a;
            SDL.SDL_GetRenderDrawColor(Pointer, out r, out g, out b, out a);

            using(var t = SdlTexture.CreateFromText(text, this, font, SdlColorFactory.FromRGBA(r, g, b, a))) {
                RenderTexture(t, x, y);
            }
        }
    }
}