using WindowsDesktop.Interop.Build10240;
using WindowsDesktop.Interop.Proxy;

namespace WindowsDesktop.Interop.Build22621;

internal class VirtualDesktopProvider22621 : VirtualDesktopProvider
{
    private ApplicationViewCollection? _applicationViewCollection;
    private VirtualDesktopManagerInternal? _virtualDesktopManagerInternal;

    public override IVirtualDesktopManagerInternal VirtualDesktopManagerInternal
        => this._virtualDesktopManagerInternal ?? throw InitializationIsRequired;

    private protected override void InitializeCore(ComInterfaceAssembly assembly)
    {
        this._applicationViewCollection = new ApplicationViewCollection(assembly);
        this._virtualDesktopManagerInternal = new VirtualDesktopManagerInternal(assembly, this._applicationViewCollection);
    }

}
