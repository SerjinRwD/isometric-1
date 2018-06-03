namespace isometric_1.Scene {
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using System;

    using isometric_1.Types;
    using isometric_1.Helpers;
    using isometric_1.ManagedSdl;

    public class MapTilePrototypeLibrary {
        public string Name { get; set; }
        public string ImageTileSetFile { get; set; }

        [XmlIgnore]
        public ImageTileSet TileSet { get; set; }
        public Size3d TileSize { get; set; }

        public MapTilePrototype[] Tiles { get; set; }
        [XmlIgnore]
        public Dictionary<string, MapTilePrototype> HashedTiles { get; set; }

        public Marker[] Markers { get; set; }
        [XmlIgnore]
        public Dictionary<string, Marker> HashedMarkers { get; set; }

        public static void Save (string path, MapTilePrototypeLibrary library) {
            var s = new XmlSerializer (typeof (MapTilePrototypeLibrary));

            using (var ws = File.OpenWrite (path)) {
                s.Serialize (ws, library);
            }
        }

        public void Save (string path) {
            MapTilePrototypeLibrary.Save (path, this);
        }

        public static MapTilePrototypeLibrary Load (string path, SdlRenderer renderer) {
            var s = new XmlSerializer (typeof (MapTilePrototypeLibrary));

            MapTilePrototypeLibrary library = null;

            using (var ws = File.OpenRead (path)) {
                library = (MapTilePrototypeLibrary) s.Deserialize (ws);
            }

            library.TileSet = ImageTileSet.Load(Data.GetFilePath("tilesets", library.ImageTileSetFile), renderer);
            
            library.HashedTiles = new Dictionary<string, MapTilePrototype>();

            foreach(var tile in library.Tiles) {
                tile.Library = library;
                library.HashedTiles.Add(tile.Name, tile);
            }

            library.HashedMarkers = new Dictionary<string, Marker>();

            foreach(var marker in library.Markers) {
                library.HashedMarkers.Add(marker.Type, marker);
            }

            return library;
        }
    }
}