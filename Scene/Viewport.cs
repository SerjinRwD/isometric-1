namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System;
    using isometric_1.Contract;
    using isometric_1.Types;
    using SDL2;

    public class Viewport : ISdlEventListener, IUpdateable {
        public SceneContext Context { get; private set; }
        public Point2d Position { get; private set; }
        public Point2d BottomRight { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Direction Direction { get; private set; }
        public bool IsMove { get; private set; }

        private int _precalculatedCellWidthHalf;
        private int _precalculatedCellLengthHalf;
        private int _precalculatedCellLengthQuarter;

        private Point2d _prevIsoTile = new Point2d(0, 0);

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

        public Viewport (SceneContext context, int x, int y, int w, int h) {
            Context = context;
            Position = new Point2d (x, y);
            BottomRight = new Point2d (x + w, y + h);
            Width = w;
            Height = h;

            _precalculatedCellWidthHalf = Context.CellSize.width >> 1;
            _precalculatedCellLengthHalf = Context.CellSize.length >> 1;
            _precalculatedCellLengthQuarter = Context.CellSize.length >> 2;
        }

        public void OnKeyDown (object sender, SdlKeyboardEventArgs args) {
            if (_mapping.ContainsKey (args.Keycode)) {
                Direction = _mapping[args.Keycode];
                IsMove = true;
            }
        }

        public void OnKeyUp (object sender, SdlKeyboardEventArgs args) {
            if (_mapping.ContainsKey (args.Keycode)) {
                IsMove = false;
            }
        }

        public void OnMouseMotion (object sender, SdlMouseMotionEventArgs args) {

            var cursorX = args.MouseMotionEvent.x + Position.x - (Context.DisplaySize.width >> 1);
            var cursorY = args.MouseMotionEvent.y + Position.y;

            var isoTileX = cursorX / _precalculatedCellWidthHalf * _precalculatedCellWidthHalf;
            var isoTileY = cursorY / _precalculatedCellLengthHalf * _precalculatedCellLengthHalf + (isoTileX % 2 == 0 ? 0 : _precalculatedCellLengthQuarter);

            var currentIsoTile = new Point2d (
                isoTileX,
                isoTileY);

            Context.Map.Hash[_prevIsoTile].IsSelected = false;

            if (Context.Map.Hash.ContainsKey (currentIsoTile)) {
                Context.Map.Hash[currentIsoTile].IsSelected = true;
                _prevIsoTile = currentIsoTile;
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

            BottomRight = new Point2d (Position.x + Width, Position.y + Height);
        }
    }
}