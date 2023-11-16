using VDesk.Core.Interop.Proxy;

namespace VDesk.Core.Interop.ComObjects;

internal class ApplicationViewCollection<TApplicationViewCollection, TApplicationView> : ComBaseObject<TApplicationViewCollection>, IApplicationViewCollection
{
    public ApplicationView<TApplicationView> GetViewForHwnd(IntPtr hWnd)
    {
        var view = InvokeMethod<object>(Args(hWnd))
                   ?? new ArgumentException("ApplicationView is not found.", nameof(hWnd));

        return new ApplicationView<TApplicationView>(view);
    }

    IApplicationView IApplicationViewCollection.GetViewForHwnd(IntPtr hWnd)
        => GetViewForHwnd(hWnd);
}
