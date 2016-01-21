using System;
using System.Runtime.InteropServices;

//Code from http://stackoverflow.com/a/32417530

namespace libVDesk {
  internal static class Guids {
    public static readonly Guid CLSID_ImmersiveShell =
        new Guid(0xC2F03A33, 0x21F5, 0x47FA, 0xB4, 0xBB, 0x15, 0x63, 0x62, 0xA2, 0xF2, 0x39);
    public static readonly Guid CLSID_VirtualDesktopManagerInternal =
        new Guid(0xC5E0CDCA, 0x7B6E, 0x41B2, 0x9F, 0xC4, 0xD9, 0x39, 0x75, 0xCC, 0x46, 0x7B);
    public static readonly Guid CLSID_VirtualDesktopManager =
        new Guid("AA509086-5CA9-4C25-8F95-589D3C07B48A");
    public static readonly Guid IID_IVirtualDesktopManagerInternal =
        new Guid("AF8DA486-95BB-4460-B3B7-6E7A6B2962B5");
    public static readonly Guid IID_IVirtualDesktop =
        new Guid("FF72FFDD-BE7E-43FC-9C03-AD81681E88E4");
  }

  [ComImport]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("FF72FFDD-BE7E-43FC-9C03-AD81681E88E4")]
  internal interface IVirtualDesktop {
    void notimpl1(); // void IsViewVisible(IApplicationView view, out int visible);
    Guid GetId();
  }

  [ComImport]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("AF8DA486-95BB-4460-B3B7-6E7A6B2962B5")]
  internal interface IVirtualDesktopManagerInternal {
    int GetCount();
    void notimpl1();  // void MoveViewToDesktop(IApplicationView view, IVirtualDesktop desktop);
    void notimpl2();  // void CanViewMoveDesktops(IApplicationView view, out int itcan);
    IVirtualDesktop GetCurrentDesktop();
    void GetDesktops(out IObjectArray desktops);
    [PreserveSig]
    int GetAdjacentDesktop(IVirtualDesktop from, int direction, out IVirtualDesktop desktop);
    void SwitchDesktop(IVirtualDesktop desktop);
    IVirtualDesktop CreateDesktop();
    void RemoveDesktop(IVirtualDesktop desktop, IVirtualDesktop fallback);
    IVirtualDesktop FindDesktop(ref Guid desktopid);
  }

  [ComImport]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
  internal interface IVirtualDesktopManager {
    int IsWindowOnCurrentVirtualDesktop(IntPtr topLevelWindow);
    Guid GetWindowDesktopId(IntPtr topLevelWindow);
    void MoveWindowToDesktop(IntPtr topLevelWindow, ref Guid desktopId);
  }

  [ComImport]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
  internal interface IObjectArray {
    void GetCount(out int count);
    void GetAt(int index, ref Guid iid, [MarshalAs(UnmanagedType.Interface)]out object obj);
  }

  [ComImport]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
  internal interface IServiceProvider10 {
    [return: MarshalAs(UnmanagedType.IUnknown)]
    object QueryService(ref Guid service, ref Guid riid);
  }

}