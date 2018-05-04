namespace isometric_1.ManagedSdl {
    using System;
    using SDL2;

    public static class SdlLogger {
        public static void Error (string source) {
            Console.WriteLine ($"{source} Error: {SDL.SDL_GetError()}");
        }
    }
}