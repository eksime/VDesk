using VDesk.Wrappers;
using WindowsDesktop;

namespace VDesk.Services
{
    public interface IVirtualDesktopService
    {
        IVirtualDesktop CreateAndSelect(int n);
        IVirtualDesktop[] GetDesktops();
        void Create();
        void MoveToDesktop(IntPtr hWnd, IVirtualDesktop virtualDesktop);
    }
    
    public class VirtualDesktopService : IVirtualDesktopService
    {
        public IVirtualDesktop CreateAndSelect(int n)
        {
            while (n > VirtualDesktop.GetDesktops().Length)
                VirtualDesktop.Create();

            return new VirtualDesktopWrapper(VirtualDesktop.GetDesktops()[Math.Max(0, n - 1)]);
        }
        
        public IVirtualDesktop[] GetDesktops() => VirtualDesktop.GetDesktops().Select(vd => new VirtualDesktopWrapper(vd)).ToArray();
        public void Create() => VirtualDesktop.Create();
        public void MoveToDesktop(IntPtr hWnd, IVirtualDesktop virtualDesktop) => VirtualDesktop.MoveToDesktop(hWnd, virtualDesktop.Instance);
    }
}