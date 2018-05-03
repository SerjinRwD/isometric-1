namespace isometric_1.Scene {
    using SDL2;

    public class Tile {
        public int Id { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int OriginX { get; set; }
        public int OriginY { get; set; }

        public SDL.SDL_Rect GetClipRect () {
            var rect = new SDL.SDL_Rect ();

            rect.x = OffsetX;
            rect.y = OffsetY;
            rect.w = Width;
            rect.h = Height;

            return rect;
        }
    }
}