namespace isometric_1.Scene {
    using isometric_1.Types;

    using SDL2;

    /// <summary>
    /// Параметры источника света
    /// </summary>
    public struct Lighting {
        public SDL.SDL_Color color;
        public byte intensity;
        public MapPoint mapPosition;

        public Lighting (SDL.SDL_Color color, byte intensity) {
            this.color = color;
            this.intensity = intensity;
            this.mapPosition = new MapPoint();
        }

        public Lighting (MapPoint mapPosition, SDL.SDL_Color color, byte intensity) {
            this.color = color;
            this.intensity = intensity;
            this.mapPosition = mapPosition;
        }

        public MapTileLight ToMapTileLight() {
            return new MapTileLight(color, intensity);
        }
    }
}