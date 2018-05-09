namespace isometric_1.Scene {
    using System.Xml.Serialization;
    using System;
    using isometric_1.Types;

    public class MapTilePrototype {
        [XmlIgnore]
        public MapTilePrototypeLibrary Library { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public MapTileType Type { get; set; }

        [XmlAttribute]
        public MapTileOrientation Orientation { get; set; }

        [XmlAttribute]
        public int FloorId { get; set; }

        [XmlAttribute]
        public int BlockId { get; set; }

        [XmlAttribute]
        public int[] DecorationIds { get; set; }

        public MapTile Create (Point2d mapPosition, int level = 0) {
            return new MapTile (mapPosition, level, Library.TileSize, Type, Orientation, FloorId, BlockId, DecorationIds);
        }
    }
}