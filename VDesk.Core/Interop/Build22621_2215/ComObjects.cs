using System.Runtime.CompilerServices;
using VDesk.Core.Interop.Proxy;

namespace VDesk.Core.Interop.Build22621_2215;

internal class ApplicationView : ComBaseObject<ComInterfaces.IApplicationView>, IApplicationView
{
    public ApplicationView(object comObject)
        : base(comObject)
    {
    }
}

internal class ApplicationViewCollection : ComBaseObject<ComInterfaces.IApplicationViewCollection>, IApplicationViewCollection
{
    public ApplicationView GetViewForHwnd(IntPtr hWnd)
    {
        var view = InvokeMethod<object>(Args(hWnd))
                   ?? new ArgumentException("ApplicationView is not found.", nameof(hWnd));

        return new ApplicationView(view);
    }

    IApplicationView IApplicationViewCollection.GetViewForHwnd(IntPtr hWnd)
        => GetViewForHwnd(hWnd);
}

internal class VirtualDesktop : ComBaseObject<ComInterfaces.IVirtualDesktop>, IVirtualDesktop
{
    private Guid? _id;

    public VirtualDesktop(object comObject)
        : base(comObject)
    {
    }

    public bool IsViewVisible(IntPtr hWnd) => InvokeMethod<bool>(Args(hWnd));

    public Guid GetID() => _id ?? (Guid)(_id = InvokeMethod<Guid>());

    public string GetName() => InvokeMethod<HString>();
}

internal class VirtualDesktopManagerInternal : ComBaseObject<ComInterfaces.IVirtualDesktopManagerInternal>, IVirtualDesktopManagerInternal
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
        var vdType = typeof(ComInterfaces.IVirtualDesktop);

        for (var i = 0u; i < count; i++)
        {
            var ppvObject = array.GetAt(i, vdType.GUID);
            yield return new VirtualDesktop(ppvObject);
        }
    }

    public IVirtualDesktop GetAdjacentDesktop(IVirtualDesktop pDesktopReference, AdjacentDesktop uDirection)
        => InvokeMethodAndWrap(Args(((VirtualDesktop)pDesktopReference).ComObject, uDirection));

    public IVirtualDesktop CreateDesktop()
        => InvokeMethodAndWrap();

    public void SwitchDesktop(IVirtualDesktop desktop)
        => InvokeMethod(Args(((VirtualDesktop)desktop).ComObject));

    public void RemoveDesktop(IVirtualDesktop pRemove, IVirtualDesktop pFallbackDesktop)
        => InvokeMethod(Args(((VirtualDesktop)pRemove).ComObject, ((VirtualDesktop)pFallbackDesktop).ComObject));

    public void MoveViewToDesktop(IntPtr hWnd, IVirtualDesktop desktop)
        => InvokeMethod(Args(((ApplicationView)_applicationViewCollection.GetViewForHwnd(hWnd)).ComObject, ((VirtualDesktop)desktop).ComObject));

    public void SetDesktopName(IVirtualDesktop desktop, string name)
        => InvokeMethod(Args(((VirtualDesktop)desktop).ComObject, new HString(name)));

    private VirtualDesktop InvokeMethodAndWrap(object?[]? parameters = null, [CallerMemberName] string methodName = "")
        => new(InvokeMethod<object>(parameters, methodName) ?? throw new Exception("Failed to get IVirtualDesktop instance."));
}
