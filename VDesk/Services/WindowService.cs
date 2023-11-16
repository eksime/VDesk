using System.Windows;
using VDesk.Utils;
using VDesk.Wrappers;

namespace VDesk.Services
{
    public interface IWindowService
    {
        void MoveHalfSplit(IntPtr hWnd, HalfSplit? split);
    }

    public class WindowService : IWindowService
    {
        public void MoveHalfSplit(IntPtr hWnd, HalfSplit? split)
        {
            switch (split)
            {
                case HalfSplit.Left:
                    Win32.MoveWindow(hWnd, 0, 0, (int)SystemParameters.MaximizedPrimaryScreenWidth / 2,
                        (int) (SystemParameters.MaximizedPrimaryScreenHeight ), true);
                    break;
                case HalfSplit.Right:
                    Win32.MoveWindow(hWnd, (int)(SystemParameters.MaximizedPrimaryScreenWidth / 2) + 1, 0,
                        (int)SystemParameters.PrimaryScreenWidth / 2, (int) (SystemParameters.MaximizedPrimaryScreenHeight), true);
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(split));
            }
        }
    }
}