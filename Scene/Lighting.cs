namespace isometric_1.Scene {
    using isometric_1.Types;

    using SDL2;

    /// <summary>
    /// Параметры источника света
    /// </summary>
    public struct Lighting {
        public SDL.SDL_Color color;
        public MapPoint mapPosition;

        public Lighting (SDL.SDL_Color color) {
            this.color = color;
            this.mapPosition = new MapPoint();
        }

        public Lighting (MapPoint mapPosition, SDL.SDL_Color color) {
            this.color = color;
            this.mapPosition = mapPosition;
        }
    }
}