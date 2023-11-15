using WindowsDesktop.Interop.Proxy;

namespace WindowsDesktop.Interop;

internal abstract class VirtualDesktopProvider
{
    public virtual bool IsSupported
        => true;

    public abstract IVirtualDesktopManager VirtualDesktopManager { get; }

    public abstract IVirtualDesktopManagerInternal VirtualDesktopManagerInternal { get; }

    public bool IsInitialized { get; internal set; }

    internal void Initialize(ComInterfaceAssembly assembly)
    {
        if (this.IsInitialized) return;

        this.InitializeCore(assembly);
        this.IsInitialized = true;
    }

    private protected abstract void InitializeCore(ComInterfaceAssembly assembly);

    internal class NotSupported : VirtualDesktopProvider
    {
        public override bool IsSupported
            => false;

        public override IVirtualDesktopManager VirtualDesktopManager
            => throw new NotSupportedException();

        public override IVirtualDesktopManagerInternal VirtualDesktopManagerInternal
            => throw new NotSupportedException();

        private protected override void InitializeCore(ComInterfaceAssembly assembly)
            => throw new NotSupportedException();
    }

    protected static InvalidOperationException InitializationIsRequired
        => new("Initialization is required.");
}
