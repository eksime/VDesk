using WindowsDesktop.Interop.Build10240;
using WindowsDesktop.Interop.Proxy;

namespace WindowsDesktop.Interop.Build22000;

internal class VirtualDesktopProvider22000 : VirtualDesktopProvider
{
    private ApplicationViewCollection? _applicationViewCollection;
    private VirtualDesktopManagerInternal? _virtualDesktopManagerInternal;

    private protected void InitializeCore(ComInterfaceAssembly assembly)
    {
        this._applicationViewCollection = new ApplicationViewCollection(assembly);
        this._virtualDesktopManagerInternal = new VirtualDesktopManagerInternal(assembly, this._applicationViewCollection);
    }

}
