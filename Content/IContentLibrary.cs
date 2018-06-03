namespace isometric_1.Content {
    using System.Collections.Generic;
    
    public interface IContentLibrary<T> {
        string Name { get; set; }
        Dictionary<string, T> Collection { get; set; }
    }
}