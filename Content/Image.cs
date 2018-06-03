namespace isometric_1.Content {
    using System.Xml.Serialization;

    using isometric_1.ManagedSdl;

    using SDL2;

    public class Image {

        [XmlIgnore]
        public SdlTexture Texture { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
        
        [XmlAttribute]
        public string BitmapFile { get; set; }

        [XmlAttribute]
        public int OffsetX { get; set; }

        [XmlAttribute]
        public int OffsetY { get; set; }

        [XmlAttribute]
        public int RegistrationX { get; set; }

        [XmlAttribute]
        public int RegistrationY { get; set; }

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
        
        public static int Comparison (Image x, Image y) {
            if (x == null && y == null) {
                return 0;
            }

            if (x == null) {
                return 1;
            }

            if (y == null) {
                return -1;
            }

            return x.Name.CompareTo (y.Name);
        }
    }
}