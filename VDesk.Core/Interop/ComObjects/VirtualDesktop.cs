using VDesk.Core.Interop.Proxy;

namespace VDesk.Core.Interop.ComObjects;

internal class VirtualDesktop<TVirtualDesktop> : ComBaseObject<TVirtualDesktop>, IVirtualDesktop
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
