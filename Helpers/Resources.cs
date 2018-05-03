namespace isometric_1.Helpers {
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System;
    using SDL2;

    public static class Resources {
        private static readonly Regex HeximalColorRGBACodeMask = new Regex (@"^#[0-9A-Fa-f]{6,8}$");
        private static string _basePath;

        public static readonly List<IntPtr> Textures = new List<IntPtr> ();
        public static readonly Dictionary<string, IntPtr> Fonts = new Dictionary<string, IntPtr> ();

        public static string BasePath {
            get {
                if (string.IsNullOrWhiteSpace (_basePath)) {
                    _basePath = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
                }

                return _basePath;
            }
        }

        public static string GetFilePath (string fileName) {
            var path = Path.Combine (BasePath, "resources", fileName);

            if (!File.Exists (path)) {
                throw new FileNotFoundException (fileName);
            }

            return path;
        }

        public static IntPtr LoadFontFromTTF(string path, int size) {
            var font = SDL_ttf.TTF_OpenFont(path, size);

            Fonts.Add(path, font);

            return font;
        }

        public static IntPtr LoadTextureFromBitmap (string path, IntPtr renderer) {
            var texture = IntPtr.Zero;

            var image = SDL.SDL_LoadBMP (path);

            if (image != IntPtr.Zero) {

                IntPtr format;

                // ммм, душок Си
                unsafe {
                    format = ((SDL.SDL_Surface * ) (image.ToPointer ())) -> format;
                }

                SDL.SDL_SetColorKey (image, (int)SDL.SDL_bool.SDL_TRUE, SDL.SDL_MapRGB(format, 255, 0, 255));

                texture = SDL.SDL_CreateTextureFromSurface (renderer, image);
                SDL.SDL_FreeSurface (image);

                if (texture == IntPtr.Zero) {
                    SdlLogger.Error (nameof (SDL.SDL_CreateTextureFromSurface));
                } else {
                    Textures.Add (texture);
                }
            } else {
                SdlLogger.Error (nameof (SDL.SDL_LoadBMP));
            }

            return texture;
        }

        public static IntPtr LoadTextureFromImage (string path, IntPtr renderer) {
            var texture = SDL_image.IMG_LoadTexture (renderer, path);

            if (texture == IntPtr.Zero) {
                SdlLogger.Error (nameof (SDL_image.IMG_LoadTexture));
            } else {
                Textures.Add (texture);
            }

            return texture;
        }

        public static IntPtr CreateTextureFromText(string text, IntPtr renderer, IntPtr font, SDL.SDL_Color color) {

            var surface = SDL_ttf.TTF_RenderUTF8_Solid(font, text, color);
            
            if(surface == IntPtr.Zero)
            {
                throw new SdlException(nameof(SDL_ttf.TTF_RenderUTF8_Solid));
            }
            
            var texture = SDL.SDL_CreateTextureFromSurface(renderer, surface);

            if(texture == IntPtr.Zero)
            {
                throw new SdlException(nameof(SDL.SDL_CreateTextureFromSurface));
            }

            Textures.Add(texture);

            SDL.SDL_FreeSurface(surface);

            return texture;
        }

        public static void Release () {

            foreach (var k in Textures) {
                if (k != IntPtr.Zero) {
                    SDL.SDL_DestroyTexture (k);
                }
            }

            Textures.Clear ();

            foreach (var k in Fonts.Keys) {
                if (Fonts[k] != IntPtr.Zero) {
                    SDL_ttf.TTF_CloseFont (Fonts[k]);
                }
            }

            Fonts.Clear ();
        }

        public static SDL.SDL_Color ParseColorCode (string rgbaCode) {
            if (!HeximalColorRGBACodeMask.IsMatch (rgbaCode)) {
                throw new ArgumentException (
                    @"Некорректный код RGB цвета. Ожидалась строка вида #ffffffff.",
                    nameof (rgbaCode));
            }

            var rStr = rgbaCode.Substring (1, 2);
            var gStr = rgbaCode.Substring (3, 2);
            var bStr = rgbaCode.Substring (5, 2);
            var aStr = rgbaCode.Substring (7, 2);

            var c = new SDL.SDL_Color ();

            c.r = byte.Parse (rStr, NumberStyles.AllowHexSpecifier);
            c.g = byte.Parse (gStr, NumberStyles.AllowHexSpecifier);
            c.b = byte.Parse (bStr, NumberStyles.AllowHexSpecifier);
            c.a = byte.Parse (aStr, NumberStyles.AllowHexSpecifier);

            return c;
        }
    }
}