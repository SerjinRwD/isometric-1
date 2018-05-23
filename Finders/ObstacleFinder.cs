namespace isometric_1.Finders {

    using System.Linq;
    using System;

    using isometric_1.Scene;
    using isometric_1.Types;

    public static class ObstacleFinder {
        public struct Result {
            public bool IsGoalAchieved { get; private set; }
            public MapPoint ObstaclePosition { get; private set; }

            public Result (bool achieved, MapPoint position) {
                IsGoalAchieved = achieved;
                ObstaclePosition = position;
            }
        }

        public static Result Check (Map map, MapTile start, MapTile goal) {

            var rad = Math.Atan2(goal.MapCoords.row - start.MapCoords.row, goal.MapCoords.column - start.MapCoords.column);
            var dist = (double)Compute.EuclideanDistance (start.MapCoords, goal.MapCoords) * 2.0D;

            MapTile current = start;

            if (current.MapCoords == goal.MapCoords) {
                return new Result (true, MapPoint.Zero);
            }

            for(var step = 1.0D; step < dist; step += 1.0D) {


                var dx = start.MapCoords.column + (int)Math.Round(step * Math.Cos(rad));
                var dy = start.MapCoords.row + (int)Math.Round(step * Math.Sin(rad));

                if(dx < 0 || dy < 0 || dx >= map.MapSize.width || dy >= map.MapSize.height) {
                    return new Result (false, MapPoint.Zero);
                }

                var tile = map.Tiles[dx, dy];

                if (tile.MapCoords == goal.MapCoords) {
                    return new Result (true, MapPoint.Zero);
                }

                if (tile.MapCoords.level > current.MapCoords.level || tile.TileType == MapTileType.Wall) {
                    return new Result (false, tile.MapCoords);
                }

                current = tile;
            }

            return new Result (false, MapPoint.Zero);
        }
        public static Result Check (MapTile start, MapTile goal) {

            if (start.MapCoords == goal.MapCoords) {
                return new Result (true, MapPoint.Zero);
            }

            var timeout = 1000;

            var current = start;

            while (timeout-- > 0) {
                //*
                var distances = current.Neighbors.Values.Select (s => new { tile = s, d = Compute.EuclideanDistance (s.MapCoords, goal.MapCoords) });
                var nearest = distances.FirstOrDefault (p => p.d == distances.Min (s => s.d));
                //*/

                /*
                var dir = Compute.DirectionBetweenPoints(current.MapCoords, goal.MapCoords);

                if(!current.Neighbors.ContainsKey(dir)) {
                    return new Result (false, MapPoint.Zero);  
                }

                var nearest = new { tile = current.Neighbors[dir] };
                //*/
                if (nearest.tile.MapCoords == goal.MapCoords) {
                    return new Result (true, MapPoint.Zero);
                }

                if (nearest == null) {
                    return new Result (false, MapPoint.Zero);
                }

                if (nearest.tile.MapCoords.level > current.MapCoords.level || nearest.tile.TileType == MapTileType.Wall) {
                    return new Result (false, nearest.tile.MapCoords);
                }

                current = nearest.tile;
            }

            return new Result (false, MapPoint.Zero);
        }
    }
}