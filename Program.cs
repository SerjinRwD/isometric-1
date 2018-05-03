namespace isometric_1 {
    using System;
    using isometric_1.Builders;
    using isometric_1.Helpers;
    using isometric_1.Scene;
    using isometric_1.Types;
    using SDL2;

    class Program {
        private static IntPtr _renderer;
        private static IntPtr _window;
        private static string _fontPath = Resources.GetFilePath ("fonts/DejaVuSansMono.ttf");
        private static IntPtr _font;
        private static bool _quit;
        private static SDL.SDL_Rect _display;

        static void Main (string[] args) {
            try {
                Init ();

                Run ();
            } catch (Exception ex) {
                Console.WriteLine ($"Exception were thrown. Message: {ex.Message}. Stack Trace: {ex.StackTrace}");
            } finally {
                Quit ();
            }
        }

        private static void Init () {
            if (SDL.SDL_Init (SDL.SDL_INIT_VIDEO) != 0) {
                throw new SdlException (nameof (SDL.SDL_Init));
            }

            if (SDL_ttf.TTF_Init () != 0) {
                throw new SdlException (nameof (SDL_ttf.TTF_Init));
            }

            SDL_image.IMG_Init (SDL_image.IMG_InitFlags.IMG_INIT_PNG);

            SDL.SDL_GetDisplayBounds (0, out _display);

            _window = SDL.SDL_CreateWindow ("Isometrics 1", 0, 0, _display.w, _display.h, SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP); // .SDL_WINDOW_SHOWN);

            if (_window == IntPtr.Zero) {
                throw new SdlException (nameof (SDL.SDL_CreateWindow));
            }

            _renderer = SDL.SDL_CreateRenderer (
                _window, -1,
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if (_renderer == IntPtr.Zero) {
                throw new SdlException (nameof (SDL.SDL_CreateRenderer));
            }

            _font = SDL_ttf.TTF_OpenFont (_fontPath, 14);
        }

        private static void Run () {

            var emitter = new SdlEventEmitter ();

            var font = Resources.LoadFontFromTTF (Resources.GetFilePath (@"fonts\DejaVuSansMono.ttf"), 8);

            var texture = Resources.LoadTextureFromBitmap (Resources.GetFilePath (@"images\demo.bmp"), _renderer);

            var floors = new Tile[] {
                new Tile {
                Width = 54,
                Height = 54,
                OriginX = 27,
                OriginY = 39
                },
                new Tile {
                OffsetY = 54,
                Width = 54,
                Height = 54,
                OriginX = 27,
                OriginY = 39,
                }
            };

            var blocks = new Tile[] {
                new Tile {
                OffsetX = 54,
                Width = 54,
                Height = 54,
                OriginX = 27,
                OriginY = 13
                }
            };

            var decorations = new Tile[] {
                new Tile {
                OffsetX = 108,
                Width = 54,
                Height = 54,
                OriginX = 27,
                OriginY = 39
                },
                new Tile {
                OffsetX = 108,
                OffsetY = 54,
                Width = 54,
                Height = 54,
                OriginX = 27,
                OriginY = 39
                }
            };

            var ui = new Tile[] {
                new Tile {
                OffsetX = 162,
                Width = 54,
                Height = 54,
                OriginX = 27,
                OriginY = 39
                },
            };

            var tileset = new TileSet (texture, floors, blocks, decorations, ui);
            var context = new SceneContext (
                new Size3d (54, 54, 27), new Size2d (3, 3), new Size2d (_display.w, _display.h), tileset, new PyramidMapBuilder()); // new Demo1MapBuilder ()); // 

            emitter.Quit += (s, a) => _quit = true;
            emitter.MouseMotion += context.Viewport.OnMouseMotion;
            emitter.KeyUp += context.Viewport.OnKeyUp;
            emitter.KeyDown += context.Viewport.OnKeyDown;
            emitter.KeyDown += (s, a) => {
                if (a.Keycode == SDL.SDL_Keycode.SDLK_ESCAPE) _quit = true;
            };

            var fontColor = Resources.ParseColorCode ("#ffffffff");

            for (var i = 0; i < context.Map.MapSize.width; i++) {
                for (var j = 0; j < context.Map.MapSize.height; j++) {
                    context.Map.Cells[i, j].Tag = Resources.CreateTextureFromText (context.Map.Cells[i, j].IsometricPosition.ToString (), _renderer, font, fontColor);
                }
            }

            while (!_quit) {
                // event handling
                emitter.Poll ();

                context.Viewport.Update ();

                // rendering
                SDL.SDL_SetRenderDrawColor (_renderer, 0, 0, 0, 255);
                SDL.SDL_RenderClear (_renderer);

                context.Map.Render (_renderer);

                /*
                for (var i = 0; i < context.Map.MapSize.width; i++) {
                    for (var j = 0; j < context.Map.MapSize.height; j++) {
                        var c = context.Map.Cells[i, j];
                        var t = (IntPtr) context.Map.Cells[i, j].Tag;

                        SDL.SDL_SetTextureColorMod (t, 10, 10, 10);
                        SDL.SDL_SetRenderDrawColor (_renderer, 10, 10, 10, 255);

                        SdlDrawing.RenderTexture (
                            t, _renderer, c.IsometricPosition.x  - context.Viewport.Position.x + context.DisplaySize.width / 2 - 1,
                            c.IsometricPosition.z - context.Viewport.Position.y + 2 );

                        SDL.SDL_RenderDrawPoint (
                            _renderer,
                            c.IsometricPosition.x - context.Viewport.Position.x + context.DisplaySize.width / 2,
                            c.IsometricPosition.z - context.Viewport.Position.y);

                        SDL.SDL_SetTextureColorMod (t, 255, 0, 255);
                        SDL.SDL_SetRenderDrawColor (_renderer, 255, 0, 255, 255);

                        SdlDrawing.RenderTexture (
                            t, _renderer, c.IsometricPosition.x  - context.Viewport.Position.x + context.DisplaySize.width / 2,
                            c.IsometricPosition.z - context.Viewport.Position.y);

                        var actualX = c.IsometricPosition.x - context.Viewport.Position.x + context.DisplaySize.width / 2;
                        var actualY = c.IsometricPosition.z - context.Viewport.Position.y;

                        SDL.SDL_RenderDrawLines (_renderer, new SDL.SDL_Point[] {
                                new SDL.SDL_Point { x = actualX, y = actualY },
                                new SDL.SDL_Point { x = actualX + 13, y = actualY },
                                new SDL.SDL_Point { x = actualX + 13, y = actualY + 13 },
                                new SDL.SDL_Point { x = actualX, y = actualY + 13 },
                                new SDL.SDL_Point { x = actualX, y = actualY },
                        }, 5);

                        SDL.SDL_RenderDrawPoint (
                            _renderer,
                            c.IsometricPosition.x - context.Viewport.Position.x + context.DisplaySize.width / 2 - 1,
                            c.IsometricPosition.z - context.Viewport.Position.y + 2);

                        SDL.SDL_SetTextureColorMod (t, 255, 255, 255);
                        SDL.SDL_SetRenderDrawColor (_renderer, 255, 255, 255, 255);
                    }
                }
                //*/

                SDL.SDL_RenderPresent (_renderer);
            }
        }

        private static void Quit () {
            if (!(_font == IntPtr.Zero)) {
                SDL_ttf.TTF_CloseFont (_font);
            }

            if (!(_renderer == IntPtr.Zero)) {
                SDL.SDL_DestroyRenderer (_renderer);
            }

            if (!(_window == IntPtr.Zero)) {
                SDL.SDL_DestroyWindow (_window);
            }

            Resources.Release ();

            SDL_ttf.TTF_Quit ();
            SDL_image.IMG_Quit ();
            SDL.SDL_Quit ();
        }
    }
}