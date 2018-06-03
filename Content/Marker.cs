namespace isometric_1.Content {
    using System.Xml.Serialization;

    using isometric_1.Types;

    public enum MarkerProduct {
        Actor,
        Decoration,
        Trigger

    }

    public class Marker {
        
        [XmlAttribute]
        public string Type { get; set; }
        [XmlAttribute]
        public int ImageId { get; set; }

        [XmlIgnore]
        public MapPoint MapPosition { get; private set; }

        public Marker Create(MapPoint mapPosition) {
            return new Marker(mapPosition, Type, ImageId);
        }

        public Marker () { }

        public Marker (MapPoint mapPosition, string type, int imageId) {
            ImageId = imageId;
            Type = type;
            MapPosition = mapPosition;
        }
    }
}