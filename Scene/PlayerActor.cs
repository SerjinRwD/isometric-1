namespace isometric_1.Scene {
    using System;
    using System.Collections.Generic;

    using isometric_1.Types;

    public class PlayerActor : AbstractActor {

        public Stack<Point2d> CurrentPath { get; protected set; }
        private bool _needFindPath;

        public override void OnMouseDown (object sender, ManagedSdl.SdlMouseButtonEventArgs args) {
            if (GenericState == ActorGenericState.Waiting) {

                var screenPos = Scene.Viewport.GetCursorPos (args.MouseButtonEvent.x, args.MouseButtonEvent.y);

                var tile = Scene.Map.TileAtScreenPos (screenPos);

                if (tile != null) {
                    GenericState = ActorGenericState.Moving;
                    Waypoint = tile.MapPosition;
                    _needFindPath = true;
                    Console.WriteLine($"Get new waypoint: {Waypoint}");

                }

            }
        }

        public override void Update () {
            base.Update();

            switch (GenericState) {
                case ActorGenericState.Waiting:
                    return;

                case ActorGenericState.Moving:
                    if (CurrentPath == null) {
                        CurrentPath = Scene.PathFinder.Find (MapPosition, Waypoint);

                        if (CurrentPath == null) {
                            GenericState = ActorGenericState.Waiting;
                            return;
                        }
                    }

                    if (CurrentPath.Count > 0) {
                        Waypoint = CurrentPath.Pop ();
                        Position = new Point3d(Waypoint.x, 0, Waypoint.y, Scene.Map.TileSize);
                    } else {
                        Console.WriteLine($"Get goal. Go waiting.");
                        GenericState = ActorGenericState.Waiting;
                        CurrentPath = null;
                        return;
                    }

                    break;
            }
        }

        public PlayerActor (SceneContext scene, Point2d mapPosition, int tileId) : base (scene, mapPosition, tileId) { }
    }
}