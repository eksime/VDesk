using WindowsDesktop.Interop.Proxy;

namespace WindowsDesktop.Interop.Build22621;

internal class VirtualDesktop : ComBaseObject<IVirtualDesktop>, IVirtualDesktop
{
    private Guid? _id;

    public VirtualDesktop(ComInterfaceAssembly assembly, object comObject)
        : base(assembly, comObject)
    {
    }

    public bool IsViewVisible(IntPtr hWnd) => InvokeMethod<bool>(Args(hWnd));

    public Guid GetID() => _id ?? (Guid)(_id = InvokeMethod<Guid>());

    public string GetName() => InvokeMethod<HString>();
}
