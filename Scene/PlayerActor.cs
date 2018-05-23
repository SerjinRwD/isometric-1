namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System;

    using isometric_1.Types;

    public class PlayerActor : AbstractActor {

        public Stack<MapPoint> CurrentPath { get; protected set; }
        private MapPoint _currentWaypoint;
        private Point3d _nextPosition;
        private bool _needSort = false;

        public override void OnMouseDown (object sender, ManagedSdl.SdlMouseButtonEventArgs args) {
            if (GenericState == ActorGenericState.Waiting) {

                var screenPos = SceneContext.Current.Viewport.GetCursorPos (args.MouseButtonEvent.x, args.MouseButtonEvent.y);

                var tile = SceneContext.Current.Map.TileAtScreenPos (screenPos);

                if (tile != null) {
                    GenericState = ActorGenericState.CheckPath;
                    Destination = tile.MapCoords;
                }

            }
        }

        public override void Update () {
            base.Update ();

            switch (GenericState) {
                case ActorGenericState.Waiting:
                    return;

                case ActorGenericState.CheckPath:
                    if (CurrentPath == null) {
                        CurrentPath = SceneContext.Current.PathFinder.Find (MapPosition, Destination);

                        if (CurrentPath == null) {
                            GenericState = ActorGenericState.Waiting;
                            return;
                        }
                    }

                    if (CurrentPath.Count > 0) {
                        _currentWaypoint = CurrentPath.Pop ();
                        _nextPosition = _currentWaypoint.ToPoint3d (SceneContext.Current.Map.TileSize);
                        Direction = Compute.DirectionBetweenPoints (MapPosition, _currentWaypoint);

                        GenericState = ActorGenericState.MovingToWaypoint;
                    } else {
                        GenericState = ActorGenericState.Waiting;
                        CurrentPath = null;
                        return;
                    }

                    break;

                case ActorGenericState.MovingToWaypoint:
                    var d = Compute.ManhattanDistance (Position.ToPoint2d (false), _nextPosition.ToPoint2d (false));

                    if (d <= (SceneContext.Current.Map.TileSize.width >> 4)) {
                        Position = _currentWaypoint.ToPoint3d (SceneContext.Current.Map.TileSize);
                        GenericState = ActorGenericState.CheckPath;
                    } else {
                        Position = Compute.StepToDirection (Position, Direction, 1);
                    }

                    _needSort = true;

                    break;
            }

            var tile = SceneContext.Current.Map.Tiles[_currentWaypoint.column, _currentWaypoint.row];
            Position = new Point3d (Position.x, tile.GetYForActor (this), Position.z);
            IsometricPosition = Compute.Isometric (Position);
            RegistrationPoint = Compute.Isometric (Position + (Image.RegistrationX, 0, Image.RegistrationY));

            if (_needSort) {
                var id = SceneContext.Current.Rendering.IndexOf(this);
                SceneContext.Current.Rendering.Sort (id, 10, RenderingList.Comparer);

                _needSort = false;
            }
        }

        public PlayerActor (MapPoint mapPosition, ImageTile image) : base (mapPosition, image) { }
    }
}