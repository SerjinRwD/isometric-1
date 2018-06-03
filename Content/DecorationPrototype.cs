namespace isometric_1.Content {
    using isometric_1.Scene;
    using isometric_1.Types;

    using Newtonsoft.Json;

    public class DecorationPrototype : IContentPrototype<Decoration> {
        public string Name { get; set; }
        public string ImageName { get; set; }

        [JsonIgnore]
        public Image Image { get; set; }
        public bool IsWall { get; set; }

        public Decoration Create (MapPoint position) {
            return new Decoration (position, Image);
        }
    }
}