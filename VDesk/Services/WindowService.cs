using VDesk.Wrappers;

namespace VDesk.Services
{
    public interface IWindowService
    {
        bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint); 
    }
    
    public class WindowService : IWindowService
    {

        public bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint)
            => User32Wrapper.MoveWindow(hWnd, X, Y, nWidth, nHeight, bRepaint);

    }
}