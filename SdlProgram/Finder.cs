namespace isometric_1.SdlProgram {
    using System.Collections.Generic;
    using System;

    using isometric_1.Helpers;
    using isometric_1.ManagedSdl;
    using isometric_1.Finders;
    using isometric_1.Scene;
    using isometric_1.Types;

    using SDL2;

    public class Finder : AbstractSdlProgram {

        enum InsertMode {
            PutStart,
            PutEnd,
            PutWall,
            PutFloor
        }

        int _width;
        int _height;
        int _cellSize;

        SDL.SDL_Color _colorText = SdlColorFactory.FromRGB ("#ee0101");
        SDL.SDL_Color _colorGrid = SdlColorFactory.FromRGB ("#efefef");
        SDL.SDL_Color _colorWall = SdlColorFactory.FromRGB ("#8b8b8b");
        SDL.SDL_Color _colorStart = SdlColorFactory.FromRGB ("#afaf44");
        SDL.SDL_Color _colorEnd = SdlColorFactory.FromRGB ("#90ff90");
        SDL.SDL_Color _colorPoint = SdlColorFactory.FromRGB ("#ffff0f");
        SDL.SDL_Color _colorRed = SdlColorFactory.FromRGB ("#ff0000");
        SDL.SDL_Color _colorGreen = SdlColorFactory.FromRGB ("#00ff00");

        MapPoint _startPoint;
        MapPoint _endPoint;
        ObstacleFinder.Result _obstacleCheck;
        MapPoint _cursorOnMap;
        SdlFont _font;

        InsertMode _insertMode;
        bool _doInsert;
        bool _quit;

        List<MapPoint> _path;

        public override void Init () {
            base.Init ();

            _quit = false;
            _doInsert = false;
            _cellSize = 54;
            _width = Window.Size.width / _cellSize;
            _height = Window.Size.height / _cellSize;

            SceneContext.Init(new Size2d (_width, _height), new Size2d(0, 0), null);

            _font = SdlFont.LoadFromTTF (Resources.GetFilePath ("fonts", "DejaVuSansMono.ttf"), 9);

            Console.WriteLine ($"MapSize: {SceneContext.Current.Map.MapSize}");
        }

        public override void Run () {

            var emitter = new SdlEventEmitter ();

            emitter.Quit += (s, a) => _quit = true;

            emitter.KeyDown += (s, a) => {
                switch (a.Keycode) {

                    case SDL.SDL_Keycode.SDLK_ESCAPE:
                        _quit = true;
                        break;

                    case SDL.SDL_Keycode.SDLK_1:
                        _insertMode = InsertMode.PutStart;
                        break;

                    case SDL.SDL_Keycode.SDLK_2:
                        _insertMode = InsertMode.PutEnd;
                        break;

                    case SDL.SDL_Keycode.SDLK_3:
                        _insertMode = InsertMode.PutWall;
                        break;

                    case SDL.SDL_Keycode.SDLK_4:
                        _insertMode = InsertMode.PutFloor;
                        break;
                }
            };

            emitter.MouseDown += (s, a) => _doInsert = true;
            emitter.MouseUp += (s, a) => _doInsert = false;
            emitter.MouseMotion += (s, a) => _cursorOnMap = new MapPoint (a.MouseMotionEvent.x / _cellSize, a.MouseMotionEvent.y / _cellSize);

            while (!_quit) {
                emitter.Poll ();

                // rendering
                Renderer.SetDrawColor (0, 0, 0, 255);
                Renderer.Clear ();

                Update ();

                DrawMatrix ();
                DrawPathFinder ();
                DrawPath ();

                Renderer.Present ();
            }
        }

        private void Update () {

            (int x, int y) = _cursorOnMap;

            var tile = SceneContext.Current.Map.Tiles[x, y];

            if (_doInsert) {
                switch (_insertMode) {
                    case InsertMode.PutStart:
                        tile.TileType = MapTileType.Floor;
                        tile.ForceNeighborsRecalculateEdges();
                        _startPoint = _cursorOnMap;
                        break;

                    case InsertMode.PutEnd:
                        tile.TileType = MapTileType.Floor;
                        tile.ForceNeighborsRecalculateEdges();
                        _endPoint = _cursorOnMap;
                        var stack = SceneContext.Current.PathFinder.Find (_startPoint, _endPoint);
                        stack.Push(_startPoint);
                        _path = new List<MapPoint>(stack.ToArray());

                        _obstacleCheck = ObstacleFinder.Check(
                            SceneContext.Current.Map,
                            SceneContext.Current.Map.Tiles[_startPoint.column, _startPoint.row],
                            SceneContext.Current.Map.Tiles[_endPoint.column, _endPoint.row]);

                        break;

                    case InsertMode.PutWall:
                        tile.TileType = MapTileType.Wall;
                        tile.ForceNeighborsRecalculateEdges();
                        break;

                    case InsertMode.PutFloor:
                        tile.TileType = MapTileType.Floor;
                        tile.ForceNeighborsRecalculateEdges();
                        break;
                }
            }

        }

        private void DrawMatrix () {

            // tiles
            for (var i = 0; i < _width; i++) {
                for (var j = 0; j < _height; j++) {
                    if (SceneContext.Current.Map.Tiles[i, j].TileType == MapTileType.Wall) {
                        Renderer.SetDrawColor (_colorWall);
                        Renderer.DrawRect (i * _cellSize, j * _cellSize, _cellSize, _cellSize, true);
                    }
                }
            }

            // start
            Renderer.SetDrawColor (_colorStart);
            Renderer.DrawRect (_startPoint.column * _cellSize, _startPoint.row * _cellSize, _cellSize, _cellSize, true);

            // end
            Renderer.SetDrawColor (_colorEnd);
            Renderer.DrawRect (_endPoint.column * _cellSize, _endPoint.row * _cellSize, _cellSize, _cellSize, true);

            // grid
            Renderer.SetDrawColor (_colorGrid);

            for (var i = 0; i < _width; i++) {
                if (i == 0) {
                    continue;
                }

                Renderer.DrawLine (i * _cellSize, 0, i * _cellSize, _height * _cellSize);
            }

            for (var j = 0; j < _height; j++) {
                if (j == 0) {
                    continue;
                }

                Renderer.DrawLine (0, j * _cellSize, _width * _cellSize, j * _cellSize);
            }
        }

        private void DrawPathFinder () {
            Renderer.SetDrawColor (_colorText);

            for (var i = 0; i < _width; i++) {
                for (var j = 0; j < _height; j++) {
                    var n = SceneContext.Current.Map.Tiles[i, j];

/*
                    Renderer.DrawText ($"{(n.closed ? "c" : " ")}", i * _cellSize + _cellSize - 10, j * _cellSize + 4, _font);
                    Renderer.DrawText ($"{n.MapPosition}", i * _cellSize + 2, j * _cellSize + 4, _font);

                    Renderer.DrawText ($"f/g:", i * _cellSize + 2, j * _cellSize + 28, _font);
                    Renderer.DrawText ($"{(n.f == int.MaxValue ? -1 : n.f)}/{(n.g == int.MaxValue ? -1 : n.g)}", i * _cellSize + 2, j * _cellSize + 37, _font);
 */

                    foreach(var key in n.Neighbors.Keys) {
                        var d = (int)key;
                        Renderer.DrawText($"{key}", i * _cellSize + (3 - d % 3) * 12, j * _cellSize + (d / 3) * 10, _font);
                    }
                }
            }
        }

        private void DrawPath () {
            if (_path == null || _path.Count == 0) {
                return;
            }

            Renderer.SetDrawColor (_colorPoint);

            for (var i = 1; i < _path.Count; i++) {
                var _prev = _path[i - 1];
                var _cur = _path[i];

                Renderer.DrawLine (_prev.ToPoint2d (_cellSize) + (_cellSize / 2, _cellSize / 2), _cur.ToPoint2d (_cellSize) + (_cellSize / 2, _cellSize / 2));
            }

            Renderer.SetDrawColor (_obstacleCheck.IsGoalAchieved ? _colorGreen : _colorRed);
            Renderer.DrawLine (_startPoint.ToPoint2d (_cellSize) + (_cellSize / 2, _cellSize / 2), _endPoint.ToPoint2d (_cellSize) + (_cellSize / 2, _cellSize / 2));

            if(!_obstacleCheck.IsGoalAchieved) {
                (var x, var y) = _obstacleCheck.ObstaclePosition.ToPoint2d (_cellSize) + (_cellSize / 2 - 2, _cellSize / 2 - 2);

                Renderer.DrawRect(x, y, 4, 4, true);
            }
        }
    }
}