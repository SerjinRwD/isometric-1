namespace isometric_1.Scene {
    using System;

    using isometric_1.Types;

    using SDL2;

    /// <summary>
    /// Параметры освещённости ячейки
    /// </summary>
    public struct MapTileLight {
        public SDL.SDL_Color color;
        public byte intensity;
        public (byte, byte, byte) modulatedColor;

        public MapTileLight Blend (SDL.SDL_Color destColor, byte destIntensity) {
            //*
            // alpha blending
            var srcI = intensity / 255.0D;
            var destI = destIntensity / 255.0D;

            var newIntensity = (byte) Math.Ceiling (255.0D * (srcI + destI * (1.0D - srcI)));
            var newColor = ((new Color(color) * srcI) + (new Color(destColor) * (1.0D - srcI))).ToSDLColor();

            return new MapTileLight(newColor, newIntensity);
            //*/
            /*
            // color modulate
            return new MapTileLight(((new Color(color)) * (new Color(destColor))).ToSDLColor(), destIntensity);
            //*/
        }

        public MapTileLight (SDL.SDL_Color color, byte intensity) {
            this.color = color;
            this.intensity = intensity;

            var r = (byte) Math.Ceiling ((double) color.r / 255.0D * (double) intensity);
            var g = (byte) Math.Ceiling ((double) color.g / 255.0D * (double) intensity);
            var b = (byte) Math.Ceiling ((double) color.b / 255.0D * (double) intensity);

            this.modulatedColor = (r, g, b);
        }
    }
}