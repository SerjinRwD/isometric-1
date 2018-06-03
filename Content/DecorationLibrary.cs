namespace isometric_1.Content {
    using System.Collections.Generic;
    using System.IO;

    using isometric_1.Scene;

    using Newtonsoft.Json;

    public class DecorationLibrary : IContentLibrary<DecorationPrototype> {
        public string Name { get; set; }
        public Dictionary<string, DecorationPrototype> Collection { get; set; }

        public static DecorationLibrary Load (string path) {

            DecorationLibrary library;

            using (var rs = File.OpenText (path)) {
                var s = new JsonSerializer ();

                library = (DecorationLibrary) s.Deserialize (rs, typeof (DecorationLibrary));
            }

            return library;
        }

        public static void Save (string path, DecorationLibrary library) {
            using (var ws = File.CreateText (path)) {
                var s = new JsonSerializer ();
                s.Serialize (ws, library);
            }
        }
    }
}