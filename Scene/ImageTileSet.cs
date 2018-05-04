namespace isometric_1.Scene {
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml;
    using System;
    using isometric_1.Helpers;
    using ManagedSdl;

    public class ImageTileSet {
        public const int NOT_SET = -1;
        public SdlTexture Texture { get; private set; }
        public ImageTile[] Floors { get; private set; }
        public ImageTile[] Blocks { get; private set; }
        public ImageTile[] Decorations { get; private set; }
        public ImageTile[] UserInterface { get; private set; }

        private static int ImageTileComparison(ImageTile x, ImageTile y) {
            if(x == null && y == null) {
                return 0;
            }

            if(x == null) {
                return 1;
            }

            if(y == null) {
                return -1;
            }

            return x.OrderId.CompareTo(y.OrderId);
        }

        private ImageTileSet (ImageTileSetMetadata metadata, SdlRenderer renderer) {
            Texture = SdlTexture.LoadFromBitmap (Resources.GetFilePath ("images", metadata.BitmapFile), renderer);

            Floors = metadata.Floors;
            Blocks = metadata.Blocks;
            Decorations = metadata.Decorations;
            UserInterface = metadata.UserInterface;
        }

        public static ImageTileSet Load(string path, SdlRenderer renderer) {
            var metadata = ImageTileSetMetadata.Load(path);

            Array.Sort(metadata.Floors, ImageTileComparison);
            Array.Sort(metadata.Blocks, ImageTileComparison);
            Array.Sort(metadata.Decorations, ImageTileComparison);
            Array.Sort(metadata.UserInterface, ImageTileComparison);
            
            return new ImageTileSet(metadata, renderer);
        }

        public ImageTileSet (SdlTexture texture, ImageTile[] floors, ImageTile[] blocks, ImageTile[] decorations, ImageTile[] userInterface) {
            Texture = texture;

            Floors = floors;
            Blocks = blocks;
            Decorations = decorations;
            UserInterface = userInterface;
        }
    }
}