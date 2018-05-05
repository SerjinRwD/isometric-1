namespace isometric_1.Scene {
    using System.Xml.Serialization;
    using SDL2;

    public class ImageTile {
        public const int NOT_SET = -1;

        [XmlAttribute]
        public int OrderId { get; set; }

        [XmlAttribute]
        public int OffsetX { get; set; }

        [XmlAttribute]
        public int OffsetY { get; set; }

        [XmlAttribute]
        public int Width { get; set; }

        [XmlAttribute]
        public int Height { get; set; }

        [XmlAttribute]
        public int OriginX { get; set; }

        [XmlAttribute]
        public int OriginY { get; set; }

        public SDL.SDL_Rect GetClipRect () {
            SDL.SDL_Rect rect;

            rect.x = OffsetX;
            rect.y = OffsetY;
            rect.w = Width;
            rect.h = Height;

            return rect;
        }
        public static int Comparison (ImageTile x, ImageTile y) {
            if (x == null && y == null) {
                return 0;
            }

            if (x == null) {
                return 1;
            }

            if (y == null) {
                return -1;
            }

            return x.OrderId.CompareTo (y.OrderId);
        }
    }
}