using VDesk.Core.Interop.Proxy;

namespace VDesk.Core.Interop;

internal abstract class VirtualDesktopProvider : IVirtualDesktopProvider
{
    protected IApplicationViewCollection ApplicationViewCollection;
    protected IVirtualDesktopManagerInternal VirtualDesktopManagerInternal;
    protected Dictionary<Guid, IVirtualDesktop> KnownDesktops = new();

    public virtual bool IsSupported
        => true;


    protected static InvalidOperationException InitializationIsRequired
        => new("Initialization is required.");

    public Guid[] GetDesktop()
    {
        KnownDesktops = VirtualDesktopManagerInternal
            .GetDesktops().ToDictionary(d => d.GetID(), d => d);

        return KnownDesktops.Keys.ToArray();
    }

    public Guid Create()
    {
        var virtualDesktop = VirtualDesktopManagerInternal
            .CreateDesktop();
        KnownDesktops.Add(virtualDesktop.GetID(), virtualDesktop);

        return virtualDesktop.GetID();
    }

    public void MoveToDesktop(IntPtr hWnd, Guid virtualDesktopId)
    {
        if (KnownDesktops.TryGetValue(virtualDesktopId, out var virtualDesktop))
        {
            VirtualDesktopManagerInternal.MoveViewToDesktop(hWnd, virtualDesktop);
        }
        else
        {
            throw new KeyNotFoundException($"cannot found virtualdesktop with key {virtualDesktopId.ToString()}");
        }
    }

    public void Switch(Guid virtualDesktopId)
    {
        if (KnownDesktops.TryGetValue(virtualDesktopId, out var virtualDesktop))
        {
            VirtualDesktopManagerInternal.SwitchDesktop(virtualDesktop);
        }
        else
        {
            throw new KeyNotFoundException($"cannot found virtualdesktop with key {virtualDesktopId.ToString()}");
        }
    }
}

internal class NotSupportedVirtualDesktop : VirtualDesktopProvider
{
    public override bool IsSupported
        => false;
}