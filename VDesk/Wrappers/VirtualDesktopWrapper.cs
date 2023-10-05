using WindowsDesktop;

namespace VDesk.Wrappers
{
    public interface IVirtualDesktop
    {

        VirtualDesktop Instance { get; }
        void Switch();
    }
    
    public class VirtualDesktopWrapper : IVirtualDesktop
    {
        private readonly VirtualDesktop _virtualDesktop;
        public VirtualDesktopWrapper(VirtualDesktop virtualDesktop)
        {
            _virtualDesktop = virtualDesktop;
        }

        public VirtualDesktop Instance => _virtualDesktop;
        public void Switch() => _virtualDesktop.Switch();
    }
}