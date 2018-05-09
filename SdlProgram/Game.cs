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

        public override void Run () {

            var font = SdlFont.LoadFromTTF (Resources.GetFilePath (@"fonts", "DejaVuSansMono.ttf"), 8);

            var tileset = ImageTileSet.Load (Resources.GetFilePath ("tilesets", "global.xml"), Renderer);
            var library = MapTilePrototypeLibrary.Load (Resources.GetFilePath ("libraries", "demo.xml"), Renderer);

            var context = new SceneContext (
                new Size2d (128, 128), new Size2d (Window.Size.width, Window.Size.height), tileset, new Demo1MapBuilder (library)); // new Demo1MapBuilder ()); // 

            var emitter = new SdlEventEmitter ();

            emitter.Quit += (s, a) => _quit = true;
            emitter.MouseMotion += context.Viewport.OnMouseMotion;
            emitter.KeyUp += context.Viewport.OnKeyUp;
            emitter.KeyDown += context.Viewport.OnKeyDown;
            emitter.KeyDown += (s, a) => {
                if (a.Keycode == SDL.SDL_Keycode.SDLK_ESCAPE) _quit = true;
            };

            foreach (var a in context.Actors) {
                emitter.MouseMotion += a.OnMouseMotion;
                emitter.MouseDown += a.OnMouseDown;
                emitter.MouseUp += a.OnMouseUp;
                emitter.KeyUp += a.OnKeyUp;
                emitter.KeyDown += a.OnKeyDown;
            }

            while (!_quit) {
                // event handling
                emitter.Poll ();

                context.Update ();

                // rendering
                Renderer.SetDrawColor (0, 0, 0, 255);
                Renderer.Clear ();

                context.Map.Render (Renderer, context.Viewport);

                Renderer.Present ();
            }
        }
    }
}