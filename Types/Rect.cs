namespace isometric_1.Types {
    using SDL2;
    public class Rect {
        Point2d Position { get; set; }
        Size2d Size { get; set; }

        public SDL.SDL_Rect ToSDLRect() {
            SDL.SDL_Rect r;

            (r.x, r.y) = Position;
            (r.w, r.h) = Size;
            
            return r;
        }

        public Rect () {}

        public Rect(Point2d position, Size2d size) {
            position = Position;
            Size = size;
        }

        public bool HasIntersection (Rect other) {
            var rectMe = ToSDLRect();
            var rectOther = other.ToSDLRect();

            return SDL.SDL_HasIntersection(ref rectMe, ref rectOther) == SDL.SDL_bool.SDL_TRUE;
        }
    }
}