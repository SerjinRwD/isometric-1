namespace isometric_1.Content {
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml;
    using System;

    using isometric_1.Helpers;

    using ManagedSdl;

    public sealed class ImageLibrary {
        public string Name { get; set; }
        public string Description { get; set; }
        public Image[] Images { get; set; }

        [XmlIgnore]
        public Dictionary<string, Image> ImagesHash { get; private set; }

        public ImageLibrary (string name, string description, IEnumerable<Image> images) {
            Name = name;
            Description = description;
            Images = images.ToArray();

            ImagesHash = new Dictionary<string, Image> ();

            foreach (var img in Images) {
                ImagesHash.Add (img.Name, img);
            }
        }

        public static ImageLibrary Load (string path, SdlRenderer renderer) {
            var s = new XmlSerializer (typeof (ImageLibrary));

            ImageLibrary lib;

            using (var rs = File.OpenRead (path)) {
                lib = (ImageLibrary) s.Deserialize (rs);
            }

            lib.ImagesHash = new Dictionary<string, Image> ();

            foreach (var img in lib.Images) {
                img.Texture = SdlTexture.LoadFromBitmap (Data.GetFilePath ("images", img.BitmapFile), renderer);
                lib.ImagesHash.Add (img.Name, img);
            }

            return lib;
        }

        public void Save (string path) {
            ImageLibrary.Save (path, this);
        }

        public static void Save (string path, ImageLibrary tileSet) {
            var s = new XmlSerializer (typeof (ImageLibrary));

            using (var ws = File.OpenWrite (path)) {
                s.Serialize (ws, tileSet);
            }
        }
    }
}