using VDesk.Utils;
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
        void Switch(IVirtualDesktop virtualDesktop);
    }

    public class VirtualDesktopService : IVirtualDesktopService
    {
        public IVirtualDesktop CreateAndSelect(int n)
        {
            return ExecuteWithLog(() =>
            {
                while (n > VirtualDesktop.GetDesktops().Length)
                    VirtualDesktop.Create();

                return new VirtualDesktopWrapper(VirtualDesktop.GetDesktops()[Math.Max(0, n - 1)]);
            });
        }

        public IVirtualDesktop[] GetDesktops() => ExecuteWithLog<IVirtualDesktop[]>(()
            => VirtualDesktop.GetDesktops().Select(vd => new VirtualDesktopWrapper(vd)).ToArray());
        public void Create() => ExecuteWithLog(VirtualDesktop.Create);
        public void MoveToDesktop(IntPtr hWnd, IVirtualDesktop virtualDesktop)
            => ExecuteWithLog(() => VirtualDesktop.MoveToDesktop(hWnd, virtualDesktop.Instance));
        public void Switch(IVirtualDesktop virtualDesktop)
        {
            ExecuteWithLog(() => virtualDesktop.Instance.Switch());
        }

        private T ExecuteWithLog<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                throw new VirtualDesktopException(e);
            }
        }

        private void ExecuteWithLog(Action func)
        {
            try
            {
                func();
            }
            catch (Exception e)
            {
                throw new VirtualDesktopException(e);
            }
        }
    }
}