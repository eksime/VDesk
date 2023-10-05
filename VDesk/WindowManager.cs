using System.Runtime.InteropServices;

namespace VDesk
{
    public abstract class WindowManager
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint); 
    }
}