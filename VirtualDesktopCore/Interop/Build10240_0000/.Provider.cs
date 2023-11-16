using WindowsDesktop.Interop.Proxy;

namespace WindowsDesktop.Interop.Build10240;

internal class VirtualDesktopProvider10240 : VirtualDesktopProvider
{
    private ApplicationViewCollection? _applicationViewCollection;
    private VirtualDesktopManagerInternal? _virtualDesktopManagerInternal;

    public IVirtualDesktopManagerInternal VirtualDesktopManagerInternal
        => this._virtualDesktopManagerInternal ?? throw InitializationIsRequired;

    private protected void InitializeCore(ComInterfaceAssembly assembly)
    {
        this._applicationViewCollection = new ApplicationViewCollection(assembly);
        this._virtualDesktopManagerInternal = new VirtualDesktopManagerInternal(assembly, this._applicationViewCollection);
    }
}
