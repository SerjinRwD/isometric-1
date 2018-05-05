namespace isometric_1.Scene {
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml;
    using System;
    using ManagedSdl;

    public class ImageTileSetMetadata {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BitmapFile { get; set; }
        public ImageTile[] Tiles { get; set; }

        public static ImageTileSetMetadata Load (string path) {
            var s = new XmlSerializer(typeof(ImageTileSetMetadata));

            using(var rs = File.OpenRead(path)) {
                return (ImageTileSetMetadata)s.Deserialize(rs);
            }
        }

        public void Save(string path) {
            ImageTileSetMetadata.Save(path, this);
        }

        public static void Save (string path, ImageTileSetMetadata tileSet) {
            var s = new XmlSerializer(typeof(ImageTileSetMetadata));

            using(var ws = File.OpenWrite(path)) {
                s.Serialize(ws, tileSet);
            }
        }
    }
}