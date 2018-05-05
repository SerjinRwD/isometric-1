namespace isometric_1 {
    using System;

    using isometric_1.Builders;
    using isometric_1.Helpers;
    using isometric_1.ManagedSdl;
    using isometric_1.Scene;
    using isometric_1.Types;

    using SDL2;

    class Program {
        private static SdlRenderer _renderer;
        private static SdlWindow _window;
        private static bool _quit;
        private static SDL.SDL_Rect _display;

        static void Main (string[] args) {

            if (args != null && args.Length > 0) {
                if (args[0] == "make-tileset-template") {
                    var result = MakeTileSetTemplate ();
                    Console.WriteLine ($"Tileset template created: \"{result}\"");

                    return;
                } else if (args[0] == "make-library-template") {
                    var result = MakeLibraryTemplate ();
                    Console.WriteLine ($"Library template created: \"{result}\"");

                    return;
                }
            }

            try {
                Init ();

                Run ();
            } catch (Exception ex) {
                Console.WriteLine ($"Exception were thrown. Message: {ex.Message}. Stack Trace: {ex.StackTrace}");
            } finally {
                Quit ();
            }
        }

        private static string MakeTileSetTemplate () {

            var tileset = new ImageTileSetMetadata {
                Name = "templateName",
                Description = "templateDescription",
                BitmapFile = "path/to/your/bitmap",
                Tiles = new ImageTile[] {
                new ImageTile ()
                }
            };

            var outputPath = System.IO.Path.Combine (Resources.GetCatalogPath ("tilesets"), "template.xml");
            tileset.Save (outputPath);

            return outputPath;
        }

        private static string MakeLibraryTemplate () {
            var library = new MapTilePrototypeLibrary {
                Name = "Template of Library",
                ImageTileSetFile = "path/to/your/tileset",
                TileSize = new Size3d (54, 54, 27),
                Tiles = new MapTilePrototype[] {
                    new MapTilePrototype {
                        Name = "prototype1",
                        Type = MapTileType.Floor,
                        Orientation = MapTileOrientation.XY,
                        FloorId = 0,
                        BlockId = ImageTile.NOT_SET,
                        DecorationIds = new int[] { 1, 2 }
                    },
                    new MapTilePrototype {
                        Name = "prototype2",
                        Type = MapTileType.Wall,
                        Orientation = MapTileOrientation.XY,
                        FloorId = ImageTile.NOT_SET,
                        BlockId = 1,
                        DecorationIds = null
                    }
                }
            };

            var outputPath = System.IO.Path.Combine (Resources.GetCatalogPath ("libraries"), "template.xml");
            library.Save (outputPath);

            return outputPath;
        }

        private static void Init () {
            if (SDL.SDL_Init (SDL.SDL_INIT_VIDEO) != 0) {
                throw new SdlException (nameof (SDL.SDL_Init));
            }

            if (SDL_ttf.TTF_Init () != 0) {
                throw new SdlException (nameof (SDL_ttf.TTF_Init));
            }

            SDL.SDL_GetDisplayBounds (0, out _display);

            _window = SdlWindow.Create ("Isometrics 1", 0, 0, _display.w, _display.h, SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP); // .SDL_WINDOW_SHOWN); //

            _renderer = SdlRenderer.Create (
                _window, -1,
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        }

        private static void Run () {

            var emitter = new SdlEventEmitter ();

            var font = SdlFont.LoadFromTTF (Resources.GetFilePath (@"fonts", "DejaVuSansMono.ttf"), 8);

            var tileset = ImageTileSet.Load (Resources.GetFilePath ("tilesets", "demo.xml"), _renderer);
            var library = MapTilePrototypeLibrary.Load (Resources.GetFilePath ("libraries", "demo.xml"), _renderer);

            var context = new SceneContext (
                new Size2d (128, 128), new Size2d (_display.w, _display.h), tileset, new Demo1MapBuilder (library)); // new Demo1MapBuilder ()); // 

            emitter.Quit += (s, a) => _quit = true;
            emitter.MouseMotion += context.Viewport.OnMouseMotion;
            emitter.KeyUp += context.Viewport.OnKeyUp;
            emitter.KeyDown += context.Viewport.OnKeyDown;
            emitter.KeyDown += (s, a) => {
                if (a.Keycode == SDL.SDL_Keycode.SDLK_ESCAPE) _quit = true;
            };

            while (!_quit) {
                // event handling
                emitter.Poll ();

                context.Viewport.Update ();

                // rendering
                _renderer.SetDrawColor (0, 0, 0, 255);
                _renderer.Clear ();

                context.Map.Render (_renderer, context.Viewport);

                _renderer.Present ();
            }
        }

        private static void Quit () {
            SDL_ttf.TTF_Quit ();
            SDL.SDL_Quit ();
        }
    }
}