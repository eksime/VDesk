using System;
using System.Runtime.InteropServices;
using WindowsDesktop.Interop.Build10240;

namespace WindowsDesktop.Interop.Build22621
{
    [ComImport]
    [Guid("00000000-0000-0000-0000-000000000000") /* replace at runtime */]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVirtualDesktopManagerInternal
    {
        int GetCount();

        void MoveViewToDesktop(IApplicationView pView, IVirtualDesktop desktop);

        bool CanViewMoveDesktops(IApplicationView pView);

        IVirtualDesktop GetCurrentDesktop();

        IObjectArray GetDesktops();

        IVirtualDesktop GetAdjacentDesktop(IVirtualDesktop pDesktopReference, int uDirection);

        void SwitchDesktop(IVirtualDesktop desktop);

        IVirtualDesktop CreateDesktop();

        void MoveDesktop(IVirtualDesktop desktop, int nIndex);

        void RemoveDesktop(IVirtualDesktop pRemove, IVirtualDesktop pFallbackDesktop);

        IVirtualDesktop FindDesktop(in Guid desktopId);

        void GetDesktopSwitchIncludeExcludeViews(IVirtualDesktop desktop, out IObjectArray o1, out IObjectArray o2);

        void SetDesktopName(IVirtualDesktop desktop, HString name);

        void SetDesktopWallpaper(IVirtualDesktop desktop, HString path);

        void UpdateWallpaperPathForAllDesktops(HString path);

        void CopyDesktopState(IApplicationView pView0, IApplicationView pView1);

        IVirtualDesktop CreateRemoteDesktop(HString name);
        
        void SwitchRemoteDesktop(IVirtualDesktop desktop);
        
        void SwitchDesktopWithAnimation(IVirtualDesktop desktop);
        
        IVirtualDesktop GetLastActiveDesktop();
        
        void WaitForAnimationToComplete();
    }
}
