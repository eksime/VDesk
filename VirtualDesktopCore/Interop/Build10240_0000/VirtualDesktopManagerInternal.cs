using System.Runtime.CompilerServices;
using WindowsDesktop.Interop.Proxy;

namespace WindowsDesktop.Interop.Build10240;

internal class VirtualDesktopManagerInternal : ComBaseObjectLegacy<IVirtualDesktopManagerInternal>, IVirtualDesktopManagerInternal
{
    private readonly ApplicationViewCollection _applicationViewCollection;

    public VirtualDesktopManagerInternal(ComInterfaceAssembly assembly, ApplicationViewCollection applicationViewCollection)
        : base(assembly, CLSID.VirtualDesktopManagerInternal)
    {
        _applicationViewCollection = applicationViewCollection;
    }

    public IEnumerable<IVirtualDesktop> GetDesktops()
    {
        var array = this.InvokeMethod<IObjectArray>();
        if (array == null) yield break;

        var count = array.GetCount();
        var vdType = this.ComInterfaceAssembly.GetType(nameof(IVirtualDesktop));

        for (var i = 0u; i < count; i++)
        {
            var ppvObject = array.GetAt(i, vdType.GUID);
            yield return new VirtualDesktop(this.ComInterfaceAssembly, ppvObject);
        }
    }

    public IVirtualDesktop GetCurrentDesktop()
        => this.InvokeMethodAndWrap();

    public IVirtualDesktop GetAdjacentDesktop(IVirtualDesktop pDesktopReference, AdjacentDesktop uDirection)
        => this.InvokeMethodAndWrap(Args(((VirtualDesktop)pDesktopReference).ComObject, uDirection));

    public IVirtualDesktop FindDesktop(Guid desktopId)
        => this.InvokeMethodAndWrap(Args(desktopId));

    public IVirtualDesktop CreateDesktop()
        => this.InvokeMethodAndWrap();

    public void SwitchDesktop(IVirtualDesktop desktop)
        => this.InvokeMethod(Args(((VirtualDesktop)desktop).ComObject));

    public void RemoveDesktop(IVirtualDesktop pRemove, IVirtualDesktop pFallbackDesktop)
        => this.InvokeMethod(Args(((VirtualDesktop)pRemove).ComObject, ((VirtualDesktop)pFallbackDesktop).ComObject));

    public void MoveViewToDesktop(IntPtr hWnd, IVirtualDesktop desktop)
        => this.InvokeMethod(Args(this._applicationViewCollection.GetViewForHwnd(hWnd).ComObject, ((VirtualDesktop)desktop).ComObject));

    public void SetDesktopName(IVirtualDesktop desktop, string name)
        => throw new NotSupportedException();

    public void SetDesktopWallpaper(IVirtualDesktop desktop, string path)
        => throw new NotSupportedException();

    public void UpdateWallpaperPathForAllDesktops(string path)
        => throw new NotSupportedException();

    private VirtualDesktop InvokeMethodAndWrap(object?[]? parameters = null, [CallerMemberName] string methodName = "")
        => new(this.ComInterfaceAssembly, this.InvokeMethod<object>(parameters, methodName) ?? throw new Exception("Failed to get IVirtualDesktop instance."));
}
