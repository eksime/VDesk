using System.Runtime.InteropServices;

namespace VDesk.Wrappers
{
    internal abstract class User32Wrapper
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal extern static bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint); 
    }
}