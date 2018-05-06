namespace isometric_1.Types
{
    public struct Size2d
    {
        public int width;
        public int height;

        public Size2d (int w, int h) {
            width = w;
            height = h;
        }

        public void Deconstruct(out int w, out int h)
        {
            w = this.width;
            h = this.height;
        }

        public override string ToString() {
            return $"({width}; {height})";
        }
    }
}