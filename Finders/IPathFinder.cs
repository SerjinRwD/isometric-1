namespace isometric_1.Finders
{
    using System.Collections.Generic;
    using isometric_1.Scene;
    using isometric_1.Types;

    public interface IPathFinder
    {
        Stack<MapPoint> Find(MapPoint start, MapPoint end);
    }
}