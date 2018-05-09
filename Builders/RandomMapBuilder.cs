namespace isometric_1.Builders {
    using System;
    using isometric_1.Contract;
    using isometric_1.Scene;
    using isometric_1.Types;

    public class RandomMapBuilder : AbstractMapBuilder {
        public RandomMapBuilder (MapTilePrototypeLibrary library) : base (library) { }
        
        public override MapBuildResult Build (Size2d mapSize) {
            throw new NotImplementedException ();
        }
    }
}