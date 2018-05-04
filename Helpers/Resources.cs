namespace isometric_1.Helpers {
    using System.IO;
    using System.Reflection;
    using System;

    public static class Resources {
        private static string _basePath;

        public static string BasePath {
            get {
                if (string.IsNullOrWhiteSpace (_basePath)) {
                    _basePath = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
                }

                return _basePath;
            }
        }

        public static string GetFilePath (string catalog, string fileName) {
            var path = Path.Combine (BasePath, "resources", catalog, fileName);

            if (!File.Exists (path)) {
                throw new FileNotFoundException (fileName);
            }

            return path;
        }

        public static string GetCatalogPath (string catalog) {
            var path = Path.Combine (BasePath, "resources", catalog);

            if (!Directory.Exists (path)) {
                throw new DirectoryNotFoundException (catalog);
            }

            return path;
        }
    }
}