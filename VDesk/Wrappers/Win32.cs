using System.Runtime.InteropServices;

namespace VDesk.Wrappers
{
    internal static class Win32
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal extern static bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr window, out int process);
        
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
    }

    public enum SM
    {
        CXMAXIMIZED = 61,
        CYMAXIMIZED = 62
    }
}