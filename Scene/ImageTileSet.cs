namespace isometric_1.Scene {
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml;
    using System;
    using isometric_1.Helpers;
    using ManagedSdl;

    public class ImageTileSet {
        public SdlTexture Texture { get; private set; }
        public ImageTile[] Tiles { get; private set; }

        private ImageTileSet (ImageTileSetMetadata metadata, SdlRenderer renderer) {
            Texture = SdlTexture.LoadFromBitmap (Resources.GetFilePath ("images", metadata.BitmapFile), renderer);

            Tiles = metadata.Tiles;
        }

        public static ImageTileSet Load(string path, SdlRenderer renderer) {
            var metadata = ImageTileSetMetadata.Load(path);

            Array.Sort(metadata.Tiles, ImageTile.Comparison);
            
            return new ImageTileSet(metadata, renderer);
        }

        public ImageTileSet (SdlTexture texture, ImageTile[] tiles) {
            Texture = texture;
            Tiles = tiles;
        }
    }
}