using WindowsDesktop.Interop.Proxy;

namespace WindowsDesktop.Interop.Build10240;

internal class VirtualDesktop : ComBaseObjectLegacy<IVirtualDesktop>, IVirtualDesktop
{
    private Guid? _id;

    public VirtualDesktop(ComInterfaceAssembly assembly, object comObject)
        : base(assembly, comObject)
    {
    }

    public bool IsViewVisible(IntPtr hWnd)
        => this.InvokeMethod<bool>(Args(hWnd));

    public Guid GetID()
        => this._id ?? (Guid)(this._id = this.InvokeMethod<Guid>());

    public string GetName()
        => "";

    public string GetWallpaperPath()
        => "";
}
