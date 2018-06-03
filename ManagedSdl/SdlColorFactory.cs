namespace isometric_1.ManagedSdl {
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System;

    using isometric_1.Types;

    using SDL2;

    public enum BlendMode {
        None,
        Alpha,
        Additive,
        ColorModulate
    }

    public static class SdlColorFactory {
        private static readonly Regex HeximalColorRGBCodeMask = new Regex (@"^#[0-9A-Fa-f]{6}$");
        public static readonly SDL.SDL_Color White;

        static SdlColorFactory () {
            White = SdlColorFactory.FromRGB (255, 255, 255);
        }

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

        public static SDL.SDL_Color FromRGB ((byte, byte, byte) rgb) {
            return FromRGBA (rgb.Item1, rgb.Item2, rgb.Item3, 255);
        }

        public static SDL.SDL_Color FromRGB (byte r, byte g, byte b) {
            return FromRGBA (r, g, b, 255);
        }

        public static SDL.SDL_Color FromRGBA (byte r, byte g, byte b, byte a) {

            var c = new SDL.SDL_Color ();

            c.r = r;
            c.g = g;
            c.b = b;
            c.a = a;

            return c;
        }

        /// <summary>
        /// <para>Данный метод не используется как замена нативному блендингу в SDL2.</para>
        /// <para>Метод используется для расчёта интенсивности освещённости ячеек карты.</para>
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static SDL.SDL_Color Blend (SDL.SDL_Color src, SDL.SDL_Color dest, BlendMode mode = BlendMode.Alpha) {

            SDL.SDL_Color newColor;
            double srcAlpha, destAlpha;
            byte newAlpha;

            switch (mode) {
                case BlendMode.Alpha:

                    srcAlpha = src.a / 255.0D;
                    destAlpha = dest.a / 255.0D;

                    newAlpha = (byte) Math.Round (255.0D * (srcAlpha + destAlpha * (1.0D - srcAlpha)));
                    newColor = ((new Color (src) * srcAlpha) + (new Color (dest) * (1.0D - srcAlpha))).ToSDLColor ();

                    newColor.a = newAlpha;

                    return newColor;

                case BlendMode.Additive:

                    srcAlpha = src.a / 255.0D;

                    newColor = ((new Color (src) * srcAlpha) + new Color (dest)).ToSDLColor ();
                    newColor.a = dest.a;

                    return new SDL.SDL_Color ();

                case BlendMode.ColorModulate:
                    newColor = ((new Color (src)) * (new Color (dest))).ToSDLColor ();
                    newColor.a = dest.a;

                    return newColor;

                case BlendMode.None:
                    return dest;

                default:
                    throw new NotImplementedException ();
            }

        }

        public static SDL.SDL_Color ApplyAlphaToColor (SDL.SDL_Color color) {

            var newColor = new SDL.SDL_Color ();

            var r = (byte) Math.Round ((double) color.r / 255.0D * (double) color.a);
            var g = (byte) Math.Round ((double) color.g / 255.0D * (double) color.a);
            var b = (byte) Math.Round ((double) color.b / 255.0D * (double) color.a);

            newColor.r = r;
            newColor.g = g;
            newColor.b = b;
            newColor.a = color.a;

            return newColor;
        }
    }
}