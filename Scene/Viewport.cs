namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System;
    using isometric_1.Contract;
    using isometric_1.ManagedSdl;
    using isometric_1.Types;
    using SDL2;

    public class Viewport : AbstractSdlEventListener, IUpdateable {
        public SceneContext Context { get; private set; }
        public Point2d Position { get; /*private */set; }
        public Point2d BottomRight { get; private set; }
        public Size2d Size { get; private set; }
        public Direction Direction { get; private set; }
        public bool IsMove { get; private set; }


        private MapTile _prevTile;

        private static Dictionary<Direction, Action<Viewport>> _handling = new Dictionary<Direction, Action<Viewport>> { // Я ленивый и терпеть не могу switch-конструкцию
            { Direction.Right, v => v.Position += (5, 0) },
            { Direction.Up, v => v.Position += (0, -5) },
            { Direction.Left, v => v.Position += (-5, 0) },
            { Direction.Down, v => v.Position += (0, 5) }
        };

        private static Dictionary<SDL.SDL_Keycode, Direction> _mapping = new Dictionary<SDL.SDL_Keycode, Direction> { // 
            { SDL.SDL_Keycode.SDLK_RIGHT, Direction.Right },
            { SDL.SDL_Keycode.SDLK_UP, Direction.Up },
            { SDL.SDL_Keycode.SDLK_DOWN, Direction.Down },
            { SDL.SDL_Keycode.SDLK_LEFT, Direction.Left }
        };

        public Viewport (SceneContext context, int x, int y, Size2d size) {
            Context = context;
            Position = new Point2d (x, y);
            BottomRight = new Point2d (x + size.width, y + size.height);
            Size = size;
        }

        public override void OnKeyDown (object sender, SdlKeyboardEventArgs args) {
            if (_mapping.ContainsKey (args.Keycode)) {
                Direction = _mapping[args.Keycode];
                IsMove = true;
            }
        }

        public override void OnKeyUp (object sender, SdlKeyboardEventArgs args) {
            if (_mapping.ContainsKey (args.Keycode)) {
                IsMove = false;
            }
        }

        public Point2d GetCursorPos(int mouseX, int mouseY) {
            return new Point2d(mouseX + Position.x - (Size.width >> 1), mouseY + Position.y);
        }

        public override void OnMouseMotion (object sender, SdlMouseMotionEventArgs args) {

            var cursorPos = GetCursorPos(args.MouseMotionEvent.x, args.MouseMotionEvent.y);

            var current = Context.Map.TileAtScreenPos(cursorPos);

            if (current != null) {
                if(_prevTile != null) {
                    _prevTile.IsSelected = false;
                }
                current.IsSelected = true;
                _prevTile = current;
            }

            // debug
            /*
            var cursor = new Point2d(cursorX, cursorY);
            Console.WriteLine ($"mouse: {cursor}, hash: {key}, match: {Context.Map.Hash.ContainsKey (key)}");
            */
        }

        public void Update () {
            if (!IsMove) {
                return;
            }

            _handling[Direction]?.Invoke (this);

            BottomRight = new Point2d (Position.x + Size.width, Position.y + Size.height);
        }
    }
}