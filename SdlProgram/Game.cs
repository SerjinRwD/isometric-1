namespace isometric_1.SdlProgram {
    using isometric_1.Builders;
    using isometric_1.Helpers;
    using isometric_1.ManagedSdl;
    using isometric_1.Scene;
    using isometric_1.SdlProgram;
    using isometric_1.Types;

    using SDL2;

    public class Game : AbstractSdlProgram {

        private bool _quit;

        public override void Init() {
            base.Init();

            SDL.SDL_ShowCursor(0);
        }

        public override void Run () {

            var font = SdlFont.LoadFromTTF (Resources.GetFilePath (@"fonts", "DejaVuSansMono.ttf"), 12);

            var tileset = ImageTileSet.Load (Resources.GetFilePath ("tilesets", "global.xml"), Renderer);
            var library = MapTilePrototypeLibrary.Load (Resources.GetFilePath ("libraries", "demo.xml"), Renderer);

            SceneContext.Init (
                new Size2d (128, 128), new Size2d (Window.Size.width, Window.Size.height), tileset, new Demo1MapBuilder (library)); // new PyramidMapBuilder (library)); // 

            var emitter = new SdlEventEmitter ();

            emitter.Quit += (s, a) => _quit = true;
            emitter.MouseMotion += SceneContext.Current.Viewport.OnMouseMotion;
            emitter.KeyUp += SceneContext.Current.Viewport.OnKeyUp;
            emitter.KeyDown += SceneContext.Current.Viewport.OnKeyDown;
            emitter.KeyDown += (s, a) => {
                if (a.Keycode == SDL.SDL_Keycode.SDLK_ESCAPE) _quit = true;
            };

            foreach (var a in SceneContext.Current.Actors) {
                emitter.MouseMotion += a.OnMouseMotion;
                emitter.MouseDown += a.OnMouseDown;
                emitter.MouseUp += a.OnMouseUp;
                emitter.KeyUp += a.OnKeyUp;
                emitter.KeyDown += a.OnKeyDown;
            }

            while (!_quit) {
                // event handling
                emitter.Poll ();

                SceneContext.Current.Update ();

                // rendering
                Renderer.SetDrawColor (0, 0, 0, 255);
                Renderer.Clear ();

                SceneContext.Current.Rendering.ForEach(r => r.Render (Renderer, SceneContext.Current.Viewport));
                //SceneContext.Current.Map.BypassTiles((tiles, i, j) => tiles[i, j].Render (Renderer, SceneContext.Current.Viewport));

                //Renderer.SetDrawColor (255, 255, 55, 255);
                //Renderer.DrawText($"GC.TotalMemory: {(System.GC.GetTotalMemory(false))}", 16, 16, font);

                Renderer.Present ();
            }
        }
    }
}