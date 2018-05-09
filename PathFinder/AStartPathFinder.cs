namespace isometric_1.PathFinder {
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using isometric_1.Scene;
    using isometric_1.Types;

    /// <summary>
    /// <para>Алгоритм поиска A*. Описание алгоритма с псевдокодом:</para>
    /// <para>http://neerc.ifmo.ru/wiki/index.php?title=Алгоритм_A*</para>
    /// </summary>
    public class AStartPathFinder : IPathFinder {
        public class Node {
            private static int _lastIndex;
            private int _id;
            public int Id { get => _id; }
            public MapTile tile;
            public int f;
            public int g;
            public bool closed;

            public Node (MapTile tile) {
                this.tile = tile;
                _id = _lastIndex++;
            }

            public void Reset () {
                f = g = 0;
                closed = false;
            }

            public bool IsMath(Point2d point) {
                return tile.MapPosition.x == point.x && tile.MapPosition.y == point.y;
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

        public Stack<Point2d> Find (Point2d start, Point2d end) {
            return Finding (start, end) ? Trace (start, end) : null;
        }

        private Stack<Point2d> Trace (Point2d start, Point2d end) {

            //return null;

            var result = new Stack<Point2d>();

            var current = Nodes[end.x, end.y];
            result.Push(current.tile.MapPosition);

            var neighboring = GetNeighboringNodes(current).Where(p => p.closed); //.OrderBy(s => s.f);
            var min = neighboring.Min(s => s.g);
            current = neighboring.FirstOrDefault(p => p.g == min);

            result.Push(current.tile.MapPosition);

            Node next = null;

            while(true) {

                neighboring = GetNeighboringNodes(current).Where(p => p.closed && p.g < current.g).OrderBy(s => s.f);

                next = neighboring.FirstOrDefault();

                if(next == null) {
                    break;
                }

                result.Push(next.tile.MapPosition);
                current = next;
            }

            //result.Add(start);
            //result.Reverse();

            return result;
        }

        private bool Finding (Point2d start, Point2d end) {

            if (IsBusy) {
                throw new InvalidOperationException ("AStartPathFinder is busy!");
            }

            IsBusy = true;

            for (var i = 0; i < Size.width; i++) {
                for (var j = 0; j < Size.height; j++) {
                    Nodes[i, j].Reset ();
                }
            }

            Func<Point2d, Point2d, int> heuristics = Heuristics.ManhattanDistance; // Heuristics.EuclideanDistance; // Heuristics.ChebyshevDistance; // 

            var q = new Dictionary<int, Node> (); // вершины, которые требуется просмотреть

            Node current = Nodes[start.x, start.y];
            current.g = 0;
            current.f = current.g + heuristics (start, end);

            q.Add (current.Id, current);

            var timeout = 1000;

            while (timeout > 0 && q.Count > 0) {
                // Вершина с минимальным f
                current = q.First (p => p.Value.f == q.Min (s => s.Value.f)).Value;
                q.Remove (current.Id);

                if (current.IsMath(end)) {
                    IsBusy = false;
                    q.Clear();
                    return true;
                }

                current.closed = true;

                foreach (var neighbor in GetNeighboringNodes (current)) {
                    var tentativeScore = current.g + CalcMoveCost (current, neighbor);

                    if (neighbor.closed && tentativeScore >= neighbor.g) {
                        continue;
                    }

                    if (!neighbor.closed || tentativeScore < neighbor.g) {
                        neighbor.g = tentativeScore;
                        neighbor.f = neighbor.g + heuristics (neighbor.tile.MapPosition, end);

                        if (!q.ContainsKey (neighbor.Id)) {
                            q.Add (neighbor.Id, neighbor);
                        }
                    }
                }

                timeout--;
            }

            IsBusy = false;
            q.Clear();
            return false;
        }

        private int CalcMoveCost (Node from, Node to) {
            return to.tile.IsEmpty && to.tile.Type == MapTileType.Floor ? 1/* + GetNeighboringNodes(to).Count(p => p.tile.Type == MapTileType.Wall)*/ : 1000;
        }

        private IEnumerable<Node> GetNeighboringNodes (Node node) {

            (int x, int y) = node.tile.MapPosition;
            int rx, ry;

            for(var i = -1; i < 2; i++) {
                for(var j = -1; j < 2; j++) {
                    if(i == 0 && j == 0) {
                        continue;
                    }

                    rx = x + i;
                    ry = y + j;

                    if(rx >= 0 && rx < Size.width && ry >= 0 && ry < Size.height) {
                        yield return Nodes[rx, ry];
                    }
                }
            }
        }
    }
}