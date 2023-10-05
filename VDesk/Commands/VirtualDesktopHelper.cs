using WindowsDesktop;

namespace VDesk_Core.Commands
{
    public static class VirtualDesktopHelper
    {
        public static VirtualDesktop CreateAndSelect(int n)
        {
            while (n > VirtualDesktop.GetDesktops().Length)
                VirtualDesktop.Create();

            return VirtualDesktop.GetDesktops()[Math.Max(0, n-1)]; 
        }
    }
}