namespace isometric_1.Contract
{
    using isometric_1.Scene;
    using isometric_1.Types;
    using System;

    public interface IMapBuilder
    {
        MapTile[,] Build(Size2d mapSize, SceneContext context);
    }
}