namespace isometric_1 {
    using System;

    using isometric_1.Builders;
    using isometric_1.Helpers;
    using isometric_1.ManagedSdl;
    using isometric_1.Scene;
    using isometric_1.Types;

    using SDL2;

    class Program {

        static void Main (string[] args) {

            if (args != null && args.Length > 0) {

                var cmd = args[0];

                if (cmd == "make-tileset-template") {
                    var result = MakeTileSetTemplate ();
                    Console.WriteLine ($"Tileset template created: \"{result}\"");

                    return;
                } else if (cmd == "make-library-template") {
                    var result = MakeLibraryTemplate ();
                    Console.WriteLine ($"Library template created: \"{result}\"");

                    return;
                } else if(cmd == "finder") {
                    (new SdlProgram.Finder()).Execute();
                }
            } else {
                var game = new SdlProgram.Game ();
                game.Execute ();
            }
        }

        private static string MakeTileSetTemplate () {

            var tileset = new ImageTileSetMetadata {
                Name = "templateName",
                Description = "templateDescription",
                BitmapFile = "path/to/your/bitmap",
                Tiles = new ImageTile[] {
                new ImageTile ()
                }
            };

            var outputPath = System.IO.Path.Combine (Resources.GetCatalogPath ("tilesets"), "template.xml");
            tileset.Save (outputPath);

            return outputPath;
        }

        private static string MakeLibraryTemplate () {
            var library = new MapTilePrototypeLibrary {
                Name = "Template of Library",
                ImageTileSetFile = "path/to/your/tileset",
                TileSize = new Size3d (54, 54, 27),
                Tiles = new MapTilePrototype[] {
                    new MapTilePrototype {
                        Name = "prototype1",
                        Type = MapTileType.Floor,
                        Orientation = MapTileOrientation.XY,
                        FloorId = 0,
                        BlockId = ImageTile.NOT_SET,
                        DecorationIds = new int[] { 1, 2 }
                    },
                    new MapTilePrototype {
                        Name = "prototype2",
                        Type = MapTileType.Wall,
                        Orientation = MapTileOrientation.XY,
                        FloorId = ImageTile.NOT_SET,
                        BlockId = 1,
                        DecorationIds = null
                    }
                }
            };

            var outputPath = System.IO.Path.Combine (Resources.GetCatalogPath ("libraries"), "template.xml");
            library.Save (outputPath);

            return outputPath;
        }
    }
}