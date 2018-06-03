namespace isometric_1.Content {
    using isometric_1.Types;

    public interface IContentPrototype<T> {
        string Name { get; set; }
        string ImageName { get; set; }
        Image Image { get; set; }
        T Create (MapPoint position);
    }
}