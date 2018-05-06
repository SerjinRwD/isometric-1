namespace isometric_1.Types {
    using System.Xml.Serialization;
    public struct Size3d {
        [XmlAttribute]
        public int width;
        [XmlAttribute]
        public int length;
        [XmlAttribute]
        public int height;

        public Size3d ((int, int, int) size) {
            width = size.Item1;
            length = size.Item2;
            height = size.Item3;
        }
        public Size3d (int w, int l, int h) {
            width = w;
            length = l;
            height = h;
        }

        public void Deconstruct(out int w, out int l, out int h)
        {
            w = this.width;
            l = this.length;
            h = this.height;
        }

        public override string ToString() {
            return $"({width}; {length}; {height})";
        }
    }
}