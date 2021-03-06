namespace isometric_1.Finders {
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using isometric_1.Scene;
    using isometric_1.Types;

    /// <summary>
    /// <para>Алгоритм поиска A*. Описание алгоритма с псевдокодом:</para>
    /// <para>http://neerc.ifmo.ru/wiki/index.php?title=Алгоритм_A*</para>
    /// </summary>
    public class AStarPathFinder : IPathFinder {

        public bool IsBusy { get; private set; }

        public Map Map { get; private set; }
        public MapTile[, ] Nodes { get; private set; }
        public Size2d Size { get; private set; }

        public AStarPathFinder (Map map) {
            Size = map.MapSize;

            Map = map;
            Nodes = map.Tiles;
        }

        public Stack<MapPoint> Find (MapPoint start, MapPoint end) {
            return start == end ? new Stack<MapPoint>(new MapPoint[] { end }) : Finding (start, end) ? Trace (start, end) : null;
        }

        private Stack<MapPoint> Trace (MapPoint start, MapPoint end) {

            var result = new Stack<MapPoint>();

            var current = Nodes[end.column, end.row];
            result.Push(current.MapCoords);

            var neighboring = current.Neighbors.Values.Where(p => p.closed); //.OrderBy(s => s.f);
            var min = neighboring.Min(s => s.g);
            current = neighboring.FirstOrDefault(p => p.g == min);

            result.Push(current.MapCoords);

            MapTile next = null;

            while(true) {

                neighboring = current.Neighbors.Values.Where(p => p.closed && p.g < current.g).OrderBy(s => s.f);

                next = neighboring.FirstOrDefault();

                if(next == null) {
                    break;
                }

                result.Push(next.MapCoords);
                current = next;
            }

            return result;
        }

        private bool Finding (MapPoint start, MapPoint end) {

            if (IsBusy) {
                throw new InvalidOperationException ($"{nameof(AStarPathFinder)} is busy!");
            }

            IsBusy = true;

            for (var i = 0; i < Size.width; i++) {
                for (var j = 0; j < Size.height; j++) {
                    Nodes[i, j].Reset ();
                }
            }

            Func<MapPoint, MapPoint, int> heuristics = Compute.ManhattanDistance; // Compute.EuclideanDistance; // Compute.ChebyshevDistance; // 

            var q = new Dictionary<int, MapTile> (); // вершины, которые требуется просмотреть

            var current = Nodes[start.column, start.row];
            current.g = 0;
            current.f = current.g + heuristics (start, end);

            q.Add (current.Id, current);

            var timeout = 1000;

            while (timeout > 0 && q.Count > 0) {
                // Вершина с минимальным f
                current = q.Values.First (p => p.f == q.Min (s => s.Value.f));
                q.Remove (current.Id);

                if (current.IsMath(end)) {
                    IsBusy = false;
                    q.Clear();
                    return true;
                }

                current.closed = true;

                foreach (var dir in current.Neighbors.Keys) {
                    var neighbor = current.Neighbors[dir];
                    var tentativeScore = current.g + current.Egdes[dir];

                    if (neighbor.closed && tentativeScore >= neighbor.g) {
                        continue;
                    }

                    if (!neighbor.closed || tentativeScore < neighbor.g) {
                        neighbor.g = tentativeScore;
                        neighbor.f = neighbor.g + heuristics (neighbor.MapCoords, end);

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
    }
}