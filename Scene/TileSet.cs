namespace isometric_1.Scene {
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml;
    using System;
    using SDL2;

    public class TileSet : IDisposable {
        public const int NOT_SET = -1;
        
        public IntPtr Texture { get; private set; }
        public Tile[] Floors { get; private set; }
        public Tile[] Blocks { get; private set; }
        public Tile[] Decorations { get; private set; }
        public Tile[] UserInterface { get; private set; }

        public static TileSet Load (string path) {
            throw new NotImplementedException ();
        }

        public static void Save (string path, TileSet tileSet) {
            throw new NotImplementedException ();
        }

        public TileSet () { }

        public TileSet (IntPtr texture, Tile[] floors, Tile[] blocks,Tile[] decorations,Tile[] userInterface) {
            Texture = texture;

            Floors = floors;
            Blocks = blocks;
            Decorations = decorations;
            UserInterface = userInterface;
        }

        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        protected virtual void Dispose (bool disposing) {
            if (disposing) {
                if (Texture != IntPtr.Zero) {
                    SDL.SDL_DestroyTexture (Texture);
                }
            }
        }
    }
}