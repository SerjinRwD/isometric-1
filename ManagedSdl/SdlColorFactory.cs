namespace isometric_1.ManagedSdl {
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System;
    using SDL2;

    public static class SdlColorFactory {
        private static readonly Regex HeximalColorRGBCodeMask = new Regex (@"^#[0-9A-Fa-f]{6}$");

        public static SDL.SDL_Color FromRGB (string code) {
            if (!HeximalColorRGBCodeMask.IsMatch (code)) {
                throw new ArgumentException (
                    @"Некорректный код RGB цвета. Ожидалась строка вида #ffffff.",
                    nameof (code));
            }

            var rStr = code.Substring (1, 2);
            var gStr = code.Substring (3, 2);
            var bStr = code.Substring (5, 2);

            var c = new SDL.SDL_Color ();

            c.r = byte.Parse (rStr, NumberStyles.AllowHexSpecifier);
            c.g = byte.Parse (gStr, NumberStyles.AllowHexSpecifier);
            c.b = byte.Parse (bStr, NumberStyles.AllowHexSpecifier);
            c.a = 255;

            return c;
        }

        public static SDL.SDL_Color FromRGB (byte r, byte g, byte b) {
            return FromRGBA(r, g, b, 255);
        }
        public static SDL.SDL_Color FromRGBA (byte r, byte g, byte b, byte a) {

            var c = new SDL.SDL_Color ();

            c.r = r;
            c.g = g;
            c.b = b;
            c.a = a;

            return c;
        }
    }
}