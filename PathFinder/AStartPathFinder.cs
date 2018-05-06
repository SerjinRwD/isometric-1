namespace isometric_1.PathFinder {
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using isometric_1.Scene;
    using isometric_1.Types;

    /// <summary>
    /// <para>Алгоритм поиска A*. Описание алгоритма с псевдокодом:</para>
    /// <para>http://neerc.ifmo.ru/wiki/index.php?title=Алгоритм_A</para>
    /// </summary>
    public class AStartPathFinder : IPathFinder {
        public class Node {
            private static int _lastIndex;
            private int _id;
            public int Id { get => _id; }
            public MapTile tile;
            public int f;
            public int g;

            public Node (MapTile tile) {
                this.tile = tile;
                _id = _lastIndex++;
            }

            public void Reset () {
                f = g = 0;
            }
        }

        public bool IsBusy { get; private set; }

        public Node[, ] Nodes { get; private set; }
        public Size2d Size { get; private set; }

        public AStartPathFinder (Map map) {
            Size = map.MapSize;

            Nodes = new Node[Size.width, Size.height];

            for (var i = 0; i < Size.width; i++) {
                for (var j = 0; j < Size.height; j++) {
                    Nodes[i, j] = new Node (map.Tiles[i, j]);
                }
            }
        }

        public List<Point2d> Find (Point2d start, Point2d end) {
            var result = Finding (start, end);

            return result != null ?
                Trace (start, end, result) :
                null;
        }

        private List<Point2d> Trace (Point2d start, Point2d end, Dictionary<int, Node> nodes) {
            var result = new List<Point2d> ();

            var first = nodes.First ().Value;
            var lastG = first.g;

            result.Add (first.tile.MapPosition.ToPoint2d (false));

            foreach (var n in nodes) { 
                if(n.Value.g == lastG + 1) {
                    result.Add (n.Value.tile.MapPosition.ToPoint2d (false));
                    lastG = n.Value.g;
                }
            }

            return result;
        }

        private Dictionary<int, Node> Finding (Point2d start, Point2d end) {

            if (IsBusy) {
                throw new InvalidOperationException ("AStartPathFinder is busy!");
            }

            IsBusy = true;

            for (var i = 0; i < Size.width; i++) {
                for (var j = 0; j < Size.height; j++) {
                    Nodes[i, j].Reset ();
                }
            }

            var u = new Dictionary<int, Node> ();
            var q = new Dictionary<int, Node> ();

            Node current = Nodes[start.x, start.y];
            current.g = 0;
            current.f = current.g + Heuristics.ManhattanDistance (start, end);

            q.Add (current.Id, current);

            var timeout = 5000;

            while (timeout > 0 && q.Count > 0) {
                // Вершина с минимальным f
                current = q.First (p => p.Value.f == q.Min (s => s.Value.f)).Value;
                q.Remove (current.Id);

                if (current.tile.MapPosition.x == end.x && current.tile.MapPosition.z == end.y) {
                    IsBusy = false;
                    return u;
                }

                if (!u.ContainsKey (current.Id)) {
                    u.Add (current.Id, current);
                }

                foreach (var neighbor in GetNeighboringNodes (current)) {
                    var tentativeScore = current.g + CalcMoveCost (current, neighbor);

                    var uContainsNeighbor = u.ContainsKey (neighbor.Id);

                    if (uContainsNeighbor && neighbor.g >= tentativeScore) {
                        continue;
                    }

                    if (!uContainsNeighbor || neighbor.g < tentativeScore) {
                        neighbor.g = tentativeScore;
                        neighbor.f = neighbor.g + Heuristics.ManhattanDistance (neighbor.tile.MapPosition.ToPoint2d (false), end);

                        if (!q.ContainsKey (neighbor.Id)) {
                            q.Add (neighbor.Id, neighbor);
                        }
                    }
                }

                // Console.WriteLine($"u.Count({u.Count}), q.Count({q.Count})");
                timeout--;
            }

            IsBusy = false;
            return null;
        }

        private int CalcMoveCost (Node from, Node to) {
            return to.tile.IsEmpty && to.tile.Type == MapTileType.Floor ? 1 : 255;
        }

        private IEnumerable<Node> GetNeighboringNodes (Node node) {

            (int nx, int ny) = node.tile.MapPosition.ToPoint2d (false);

            // (+1, 0, 0)
            if ((nx + 1) < Size.width) {
                yield return Nodes[nx + 1, ny];
            }

            // (0, 0, +1)
            if ((ny + 1) < Size.height) {
                yield return Nodes[nx, ny + 1];
            }

            // (-1, 0, 0)
            if ((nx - 1) > 0) {
                yield return Nodes[nx - 1, ny];
            }

            // (0, 0, -1)
            if ((ny - 1) > 0) {
                yield return Nodes[nx, ny - 1];
            }
        }
    }
}