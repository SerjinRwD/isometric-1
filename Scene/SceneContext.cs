namespace isometric_1.Scene {
    using System.Collections.Generic;

    using isometric_1.Builders;
    using isometric_1.Contract;
    using isometric_1.PathFinder;
    using isometric_1.Types;

    public sealed class SceneContext : IUpdateable {
        public IPathFinder PathFinder { get; private set; }
        public Viewport Viewport { get; private set; }
        public ImageTileSet TileSet { get; private set; }
        public Map Map { get; private set; }
        public List<AbstractActor> Actors { get; private set; }

        public SceneContext (
            Size2d mapSize,
            Size2d viewportSize,
            ImageTileSet tileSet,
            IMapBuilder builder) {

            TileSet = tileSet;
            Map = new Map (mapSize, builder);
            PathFinder = new AStartPathFinder (Map);
            Viewport = new Viewport (this, 0, 0, viewportSize);

            Actors = new List<AbstractActor>();

            Map.Markers.ForEach(m => {
                if(m.Type == "player-1") {
                    var p = new PlayerActor(this, m.MapPosition, 0);
                    Actors.Add(p);
                    Viewport.Position = Point3d.CalcIsometric(p.Position).ToPoint2d() - (0, Viewport.Size.height >> 1);
                }
            });
        }

        public void Update () {

            Viewport.Update ();

            foreach(var a in Actors) {
                a.Update();
            }

        }
    }
}