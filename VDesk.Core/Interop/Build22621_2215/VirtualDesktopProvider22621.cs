namespace VDesk.Core.Interop.Build22621_2215;

internal class VirtualDesktopProvider22621 : VirtualDesktopProvider
{
    public VirtualDesktopProvider22621()
    {
        ApplicationViewCollection = new ApplicationViewCollection();
        VirtualDesktopManagerInternal = new VirtualDesktopManagerInternal((ApplicationViewCollection) ApplicationViewCollection);
    }
}
