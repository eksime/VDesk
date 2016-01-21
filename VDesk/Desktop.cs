using System;
using System.Runtime.InteropServices;

//Code from http://stackoverflow.com/a/32417530

namespace libVDesk {
  public class Desktop {
    public static int Count {
      // Returns the number of desktops
      get { return DesktopManager.Manager.GetCount(); }
    }

    public static Desktop Current {
      // Returns current desktop
      get { return new Desktop(DesktopManager.Manager.GetCurrentDesktop()); }
    }

    public static Desktop FromIndex(int index) {
      // Create desktop object from index 0..Count-1
      return new Desktop(DesktopManager.GetDesktop(index));
    }

    public static Desktop FromWindow(IntPtr hWnd) {
      // Creates desktop object on which window <hWnd> is displayed
      Guid id = DesktopManager.WManager.GetWindowDesktopId(hWnd);
      return new Desktop(DesktopManager.Manager.FindDesktop(ref id));
    }

    public static Desktop Create() {
      // Create a new desktop
      return new Desktop(DesktopManager.Manager.CreateDesktop());
    }

    public void Remove(Desktop fallback = null) {
      // Destroy desktop and switch to <fallback>
      var back = fallback == null ? DesktopManager.GetDesktop(0) : fallback.itf;
      DesktopManager.Manager.RemoveDesktop(itf, back);
    }

    public bool IsVisible {
      // Returns <true> if this desktop is the current displayed one
      get { return object.ReferenceEquals(itf, DesktopManager.Manager.GetCurrentDesktop()); }
    }

    public void MakeVisible() {
      // Make this desktop visible
      DesktopManager.Manager.SwitchDesktop(itf);
    }

    public Desktop Left {
      // Returns desktop at the left of this one, null if none
      get {
        IVirtualDesktop desktop;
        int hr = DesktopManager.Manager.GetAdjacentDesktop(itf, 3, out desktop);
        if (hr == 0) return new Desktop(desktop);
        else return null;

      }
    }

    public Desktop Right {
      // Returns desktop at the right of this one, null if none
      get {
        IVirtualDesktop desktop;
        int hr = DesktopManager.Manager.GetAdjacentDesktop(itf, 4, out desktop);
        if (hr == 0) return new Desktop(desktop);
        else return null;
      }
    }

    public void MoveWindow(IntPtr handle) {
      // Move window <handle> to this desktop
      DesktopManager.WManager.MoveWindowToDesktop(handle, itf.GetId());
    }

    public bool HasWindow(IntPtr handle) {
      // Returns true if window <handle> is on this desktop
      return itf.GetId() == DesktopManager.WManager.GetWindowDesktopId(handle);
    }

    public override int GetHashCode() {
      return itf.GetHashCode();
    }
    public override bool Equals(object obj) {
      var desk = obj as Desktop;
      return desk != null && object.ReferenceEquals(this.itf, desk.itf);
    }

    private IVirtualDesktop itf;
    private Desktop(IVirtualDesktop itf) { this.itf = itf; }
  }

  internal static class DesktopManager {
    static DesktopManager() {
      var shell = (IServiceProvider10)Activator.CreateInstance(Type.GetTypeFromCLSID(Guids.CLSID_ImmersiveShell));
      Manager = (IVirtualDesktopManagerInternal)shell.QueryService(Guids.CLSID_VirtualDesktopManagerInternal, Guids.IID_IVirtualDesktopManagerInternal);
      WManager = (IVirtualDesktopManager)Activator.CreateInstance(Type.GetTypeFromCLSID(Guids.CLSID_VirtualDesktopManager));
    }

    internal static IVirtualDesktop GetDesktop(int index) {
      int count = Manager.GetCount();
      if (index < 0 || index >= count) throw new ArgumentOutOfRangeException("index");
      IObjectArray desktops;
      Manager.GetDesktops(out desktops);
      object objdesk;
      desktops.GetAt(index, Guids.IID_IVirtualDesktop, out objdesk);
      Marshal.ReleaseComObject(desktops);
      return (IVirtualDesktop)objdesk;
    }

    internal static IVirtualDesktopManagerInternal Manager;
    internal static IVirtualDesktopManager WManager;
  }
}