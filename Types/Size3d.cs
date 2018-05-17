namespace isometric_1.Types {
    using System.Xml.Serialization;
    public struct Size3d {
        [XmlAttribute]
        public int width;
        [XmlAttribute]
        public int length;
        [XmlAttribute]
        public int height;

        public Size3d (int w, int h, int l) {
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