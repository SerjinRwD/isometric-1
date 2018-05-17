namespace isometric_1.Scene {
    using System.Collections.Generic;

    using isometric_1.Builders;
    using isometric_1.Contract;
    using isometric_1.PathFinder;
    using isometric_1.Types;

    public sealed class SceneContext : IUpdateable {
        public static SceneContext Current { get; private set; }
        public IPathFinder PathFinder { get; private set; }
        public Viewport Viewport { get; private set; }
        public ImageTileSet TileSet { get; private set; }
        public Map Map { get; private set; }
        public List<AbstractActor> Actors { get; private set; }
        public RenderingList Rendering { get; private set; }

        private SceneContext () { }

        public static void Init (
            Size2d mapSize,
            Size2d viewportSize,
            ImageTileSet tileSet,
            IMapBuilder builder = null) {

            SceneContext.Current = new SceneContext ();

            SceneContext.Current.Rendering = new RenderingList ();

            SceneContext.Current.TileSet = tileSet;

            SceneContext.Current.Map = builder == null ? new Map (mapSize) : new Map (mapSize, builder);
            SceneContext.Current.Map.Init ();

            SceneContext.Current.PathFinder = new AStarPathFinder (SceneContext.Current.Map);
            SceneContext.Current.Viewport = new Viewport (0, 0, viewportSize);

            SceneContext.Current.Actors = new List<AbstractActor> ();

            SceneContext.Current.Map.Markers?.ForEach (m => {
                if (m.Type == "player-1") {
                    var p = new PlayerActor (m.MapPosition, SceneContext.Current.TileSet.Tiles[0]);
                    SceneContext.Current.Actors.Add (p);
                    SceneContext.Current.Viewport.Position = Compute.Isometric (p.Position).ToPoint2d () - (0, SceneContext.Current.Viewport.Size.height >> 1);

                    SceneContext.Current.Rendering.Add(p);
                }
            });

            SceneContext.Current.Map.BypassTiles((tiles, i, j) => {
                SceneContext.Current.Rendering.Add(tiles[i, j]);
            });

            SceneContext.Current.Rendering.Sort(RenderingList.Comparer);
        }

        public void Update () {

            Viewport.Update ();

            foreach (var a in Actors) {
                a.Update ();
            }

        }
    }
}