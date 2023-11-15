using WindowsDesktop.Interop.Build10240;
using WindowsDesktop.Interop.Proxy;

namespace WindowsDesktop.Interop.Build22000;

internal class VirtualDesktopProvider22000 : VirtualDesktopProvider
{
    private IVirtualDesktopManager? _virtualDesktopManager;
    private ApplicationViewCollection? _applicationViewCollection;
    private VirtualDesktopManagerInternal? _virtualDesktopManagerInternal;

    public override IVirtualDesktopManager VirtualDesktopManager
        => this._virtualDesktopManager ?? throw InitializationIsRequired;

    public override IVirtualDesktopManagerInternal VirtualDesktopManagerInternal
        => this._virtualDesktopManagerInternal ?? throw InitializationIsRequired;

    private protected override void InitializeCore(ComInterfaceAssembly assembly)
    {
        var type = Type.GetTypeFromCLSID(CLSID.VirtualDesktopManager)
            ?? throw new Exception($"No type found for CLSID '{CLSID.VirtualDesktopManager}'.");
        this._virtualDesktopManager = Activator.CreateInstance(type) is IVirtualDesktopManager manager
            ? manager
            : throw new Exception($"Failed to create instance of Type '{typeof(IVirtualDesktopManager)}'.");

        this._applicationViewCollection = new ApplicationViewCollection(assembly);
        this._virtualDesktopManagerInternal = new VirtualDesktopManagerInternal(assembly, this._applicationViewCollection);
    }

}
