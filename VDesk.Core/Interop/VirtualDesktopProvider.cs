using VDesk.Core.Interop.ComObjects;
using VDesk.Core.Interop.Proxy;

namespace VDesk.Core.Interop;

internal class VirtualDesktopProvider<TVirtualDesktopManagerInternal, TVirtualDesktop, TApplicationView, TApplicationViewCollection> : IVirtualDesktopProvider
{
    private readonly IApplicationViewCollection _applicationViewCollection;
    private readonly IVirtualDesktopManagerInternal _virtualDesktopManagerInternal;
    private Dictionary<Guid, IVirtualDesktop> _knownDesktops = new();

    public VirtualDesktopProvider()
    {
        _applicationViewCollection = new ApplicationViewCollection<TApplicationViewCollection, TApplicationView>();
        _virtualDesktopManagerInternal = new VirtualDesktopManagerInternal<TVirtualDesktopManagerInternal, TVirtualDesktop, TApplicationView>(_applicationViewCollection);
    }
    public virtual bool IsSupported
        => true;


    protected static InvalidOperationException InitializationIsRequired
        => new("Initialization is required.");

    public Guid[] GetDesktop()
    {
        _knownDesktops = _virtualDesktopManagerInternal
            .GetDesktops().ToDictionary(d => d.GetID(), d => d);

        return _knownDesktops.Keys.ToArray();
    }

    public Guid Create()
    {
        var virtualDesktop = _virtualDesktopManagerInternal
            .CreateDesktop();
        _knownDesktops.Add(virtualDesktop.GetID(), virtualDesktop);

        return virtualDesktop.GetID();
    }

    public void MoveToDesktop(IntPtr hWnd, Guid virtualDesktopId)
    {
        if (_knownDesktops.TryGetValue(virtualDesktopId, out var virtualDesktop))
        {
            _virtualDesktopManagerInternal.MoveViewToDesktop(hWnd, virtualDesktop);
        }
        else
        {
            throw new KeyNotFoundException($"cannot found virtualdesktop with key {virtualDesktopId.ToString()}");
        }
    }

    public void Switch(Guid virtualDesktopId)
    {
        if (_knownDesktops.TryGetValue(virtualDesktopId, out var virtualDesktop))
        {
            _virtualDesktopManagerInternal.SwitchDesktop(virtualDesktop);
        }
        else
        {
            throw new KeyNotFoundException($"cannot found virtualdesktop with key {virtualDesktopId.ToString()}");
        }
    }
}