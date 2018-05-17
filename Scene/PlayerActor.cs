namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System;

    using isometric_1.Types;

    public class PlayerActor : AbstractActor {

        public Stack<Point2d> CurrentPath { get; protected set; }
        private Point2d _currentWaypoint;
        private Direction _currentDirection;
        private Point3d _nextPosition;

        public override void OnMouseDown (object sender, ManagedSdl.SdlMouseButtonEventArgs args) {
            if (GenericState == ActorGenericState.Waiting) {

                var screenPos = SceneContext.Current.Viewport.GetCursorPos (args.MouseButtonEvent.x, args.MouseButtonEvent.y);

                var tile = SceneContext.Current.Map.TileAtScreenPos (screenPos);

                if (tile != null) {
                    GenericState = ActorGenericState.CheckPath;
                    Destination = tile.MapPosition;
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
                        _nextPosition = new Point3d (_currentWaypoint.x, 0, _currentWaypoint.y, SceneContext.Current.Map.TileSize);
                        _currentDirection = Compute.DirectionBetweenPoints (MapPosition, _currentWaypoint);

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
                        Position = new Point3d (_currentWaypoint.x, 0, _currentWaypoint.y, SceneContext.Current.Map.TileSize);
                        GenericState = ActorGenericState.CheckPath;
                    } else {
                        Position = Compute.StepToDirection (Position, _currentDirection, 1);
                    }
                    break;
            }

            var tile = SceneContext.Current.Map.Tiles[_currentWaypoint.x, _currentWaypoint.y];
            Position = new Point3d (Position.x, tile.GetYForActor (this), Position.z);
            IsometricPosition = Compute.Isometric (Position);
            RegistrationPoint = Compute.Isometric (Position + (Image.RegistrationX, 0, Image.RegistrationY));
            SceneContext.Current.Rendering.Sort (RenderingList.Comparer);
        }

        public PlayerActor (Point2d mapPosition, ImageTile image) : base (mapPosition, image) { }
    }
}