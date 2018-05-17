namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System;

    using isometric_1.Contract;

    public class RenderingList : List<IRenderable> {
        public static IComparer<IRenderable> Comparer { get; private set; } = new OrderingByY ();
    }

    public class OrderingByY : IComparer<IRenderable> {
        public int Compare (IRenderable x, IRenderable y) {
            return x.RegistrationPoint.z.CompareTo (y.RegistrationPoint.z);
        }
    }
}