using System.Diagnostics;

namespace VDesk.Utils
{
    public static class ProcessExtensions
    {
        public static void WaitForMainWindow(this Process pocess)
        {
            for (var backoff = 1; pocess.MainWindowHandle.ToInt64() == 0 && backoff <= 0x10000; backoff <<= 1)
                Thread.Sleep(backoff);
        }
    }
}