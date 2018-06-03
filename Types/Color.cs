namespace isometric_1.Types {
    using System;

    using SDL2;

    public struct Color {
        public double red;
        public double green;
        public double blue;

        public Color(SDL.SDL_Color c) {
            red = c.r / 255.0D;
            green = c.g / 255.0D;
            blue = c.b / 255.0D;
        }

        public Color (double r, double g, double b) {
            red = Math.Min (1.0D, r);
            green = Math.Min (1.0D, g);
            blue = Math.Min (1.0D, b);
        }

        public static Color operator * (Color c, double a) {
            return new Color (c.red * a, c.green * a, c.blue * a);
        }

        public static Color operator * (Color x, Color y) {
            return new Color (x.red * y.red, x.green * y.green, x.blue * y.blue);
        }

        public static Color operator + (Color x, Color y) {
            return new Color (x.red + y.red, x.green + y.green, x.blue + y.blue);
        }

        public SDL.SDL_Color ToSDLColor () {
            var sdlColor = new SDL.SDL_Color();

            sdlColor.r = (byte) Math.Ceiling(255.0D * red);
            sdlColor.g = (byte) Math.Ceiling(255.0D * green);
            sdlColor.b = (byte) Math.Ceiling(255.0D * blue);

            return sdlColor;
        }
    }
}