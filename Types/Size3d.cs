namespace isometric_1.Types {
    public struct Size3d {
        public int width;
        public int length;
        public int height;

        public Size3d((int, int, int) size) {
            width = size.Item1;
            length = size.Item2;
            height = size.Item3;    
        }
        public Size3d (int w, int l, int h) {
            width = w;
            length = l;
            height = h;
        }
    }
}