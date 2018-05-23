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
        public Direction Orientation { get; set; }

        [XmlAttribute]
        public int FloorId { get; set; }

        [XmlAttribute]
        public int WallSouthId { get; set; }

        [XmlAttribute]
        public int WallNorthId { get; set; }

        public MapTile Create (MapPoint mapPosition) {
            return new MapTile (
                mapPosition, Library.TileSize, Type,
                Library.TileSet.Tiles[FloorId], Library.TileSet.Tiles[WallSouthId], Library.TileSet.Tiles[WallNorthId],
                Orientation);
        }
    }
}