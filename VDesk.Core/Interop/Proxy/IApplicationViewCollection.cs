namespace VDesk.Core.Interop.Proxy;

[ComInterface]
public interface IApplicationViewCollection
{
    IApplicationView GetViewForHwnd(IntPtr hWnd);
}
