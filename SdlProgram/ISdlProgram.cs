namespace isometric_1.SdlProgram {
    using isometric_1.ManagedSdl;
    
    public interface ISdlProgram {
        SdlRenderer Renderer { get; }
        SdlWindow Window { get; }

        void Execute();
        void Init ();
        void Run ();
        void Quit ();
    }
}