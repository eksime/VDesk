using System.Runtime.CompilerServices;
using VDesk.Core.Interop.Proxy;

namespace VDesk.Core.Interop.ComObjects;

internal class VirtualDesktopManagerInternal<TVirtualDesktopManagerInternal, TVirtualDesktop, TApplicationView> : ComBaseObject<TVirtualDesktopManagerInternal>, IVirtualDesktopManagerInternal
{
    private readonly IApplicationViewCollection _applicationViewCollection;

    public VirtualDesktopManagerInternal(IApplicationViewCollection applicationViewCollection)
        : base(CLSID.VirtualDesktopManagerInternal)
    {
        _applicationViewCollection = applicationViewCollection;
    }

    public IEnumerable<IVirtualDesktop> GetDesktops()
    {
        var array = InvokeMethod<IObjectArray>();
        if (array == null) yield break;

        var count = array.GetCount();
        var vdType = typeof(TVirtualDesktop);

        for (var i = 0u; i < count; i++)
        {
            var ppvObject = array.GetAt(i, vdType.GUID);
            yield return new VirtualDesktop<TVirtualDesktop>(ppvObject);
        }
    }

    public IVirtualDesktop GetAdjacentDesktop(IVirtualDesktop pDesktopReference, AdjacentDesktop uDirection)
        => InvokeMethodAndWrap(Args(((VirtualDesktop<TVirtualDesktop>)pDesktopReference).ComObject, uDirection));

    public IVirtualDesktop CreateDesktop()
        => InvokeMethodAndWrap();

    public void SwitchDesktop(IVirtualDesktop desktop)
        => InvokeMethod(Args(((VirtualDesktop<TVirtualDesktop>)desktop).ComObject));

    public void RemoveDesktop(IVirtualDesktop pRemove, IVirtualDesktop pFallbackDesktop)
        => InvokeMethod(Args(((VirtualDesktop<TVirtualDesktop>)pRemove).ComObject, ((VirtualDesktop<TVirtualDesktop>)pFallbackDesktop).ComObject));

    public void MoveViewToDesktop(IntPtr hWnd, IVirtualDesktop desktop)
        => InvokeMethod(Args(((ApplicationView<TApplicationView>)_applicationViewCollection.GetViewForHwnd(hWnd)).ComObject, ((VirtualDesktop<TVirtualDesktop>)desktop).ComObject));

    public void SetDesktopName(IVirtualDesktop desktop, string name)
        => InvokeMethod(Args(((VirtualDesktop<TVirtualDesktop>)desktop).ComObject, new HString(name)));

    private VirtualDesktop<TVirtualDesktop> InvokeMethodAndWrap(object?[]? parameters = null, [CallerMemberName] string methodName = "")
        => new(InvokeMethod<object>(parameters, methodName) ?? throw new Exception("Failed to get IVirtualDesktop instance."));
}
