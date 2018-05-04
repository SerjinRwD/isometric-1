namespace isometric_1.ManagedSdl {
    using System;
    using SDL2;
    
    public class SdlException : Exception {
        public SdlException () : base ($"{SDL.SDL_GetError()}") { }

        public SdlException (string method) : base ($"{method}: {SDL.SDL_GetError()}") { }
    }
}