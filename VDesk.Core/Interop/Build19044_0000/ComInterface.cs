using System.Runtime.InteropServices;

namespace VDesk.Core.Interop.Build19044_0000;

[ComImport]
[Guid("372E1D3B-38D3-42E4-A15B-8AB2B178F513")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IApplicationView
{
    void GetIids(out ulong iidCount, out IntPtr iids);
    HString GetRuntimeClassName();
    IntPtr GetTrustLevel();
    void SetFocus();
    void SwitchTo();
    void TryInvokeBack(IntPtr callback);
    IntPtr GetThumbnailWindow();
    IntPtr GetMonitor();
    int GetVisibility();
    void SetCloak(ApplicationViewCloakType cloakType, int unknown);
    IntPtr GetPosition(in Guid guid, out IntPtr position);
    void SetPosition(in IntPtr position);
    void InsertAfterWindow(IntPtr hwnd);
    Rect GetExtendedFramePosition();
    [return: MarshalAs(UnmanagedType.LPWStr)] string GetAppUserModelId();
    void SetAppUserModelId([MarshalAs(UnmanagedType.LPWStr)] string id);
    bool IsEqualByAppUserModelId(string id);
    uint GetViewState();
    void SetViewState(uint state);
    int GetNeediness();
    ulong GetLastActivationTimestamp();
    void SetLastActivationTimestamp(ulong timestamp);
    Guid GetVirtualDesktopId();
    void SetVirtualDesktopId(in Guid guid);
    int GetShowInSwitchers();
    void SetShowInSwitchers(int flag);
    int GetScaleFactor();
    bool CanReceiveInput();
    ApplicationViewCompatibilityPolicy GetCompatibilityPolicyType();
    void SetCompatibilityPolicyType(ApplicationViewCompatibilityPolicy flags);
    IntPtr GetPositionPriority();
    void SetPositionPriority(IntPtr priority);
    void GetSizeConstraints(IntPtr monitor, out Size size1, out Size size2);
    void GetSizeConstraintsForDpi(uint uint1, out Size size1, out Size size2);
    void SetSizeConstraintsForDpi(ref uint uint1, in Size size1, in Size size2);
    int QuerySizeConstraintsFromApp();
    void OnMinSizePreferencesUpdated(IntPtr hwnd);
    void ApplyOperation(IntPtr operation);
    bool IsTray();
    bool IsInHighZOrderBand();
    bool IsSplashScreenPresented();
    void Flash();
    IApplicationView GetRootSwitchableOwner();
    IObjectArray EnumerateOwnershipTree();
    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetEnterpriseId();
    bool IsMirrored();
}

[ComImport]
[Guid("1841C6D7-4F9D-42C0-AF41-8747538F10E5")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IApplicationViewCollection
{
    IObjectArray GetViews();
    IObjectArray GetViewsByZOrder();
    IObjectArray GetViewsByAppUserModelId(string id);
    IApplicationView GetViewForHwnd(IntPtr hwnd);
    IApplicationView GetViewForApplication(object application);
    IApplicationView GetViewForAppUserModelId(string id);
    IntPtr GetViewInFocus();
    void RefreshCollection();
    int RegisterForApplicationViewChanges(object listener);
    int RegisterForApplicationViewPositionChanges(object listener);
    void UnregisterForApplicationViewChanges(int cookie);
}

[ComImport]
[Guid("FF72FFDD-BE7E-43FC-9C03-AD81681E88E4")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IVirtualDesktop
{
    bool IsViewVisible(IApplicationView view);
    Guid GetID();
}

[ComImport]
[Guid("F31574D6-B682-4CDC-BD56-1827860ABEC6")]
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
    void RemoveDesktop(IVirtualDesktop pRemove, IVirtualDesktop pFallbackDesktop);
    IVirtualDesktop FindDesktop(in Guid desktopId);
}

[StructLayout(LayoutKind.Sequential)]
public struct Size
{
    public int X;
    public int Y;
}

[StructLayout(LayoutKind.Sequential)]
public struct Rect
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
}

public enum ApplicationViewCloakType
{
    AVCT_NONE = 0,
    AVCT_DEFAULT = 1,
    AVCT_VIRTUAL_DESKTOP = 2
}

public enum ApplicationViewCompatibilityPolicy
{
    AVCP_NONE = 0,
    AVCP_SMALL_SCREEN = 1,
    AVCP_TABLET_SMALL_SCREEN = 2,
    AVCP_VERY_SMALL_SCREEN = 3,
    AVCP_HIGH_SCALE_FACTOR = 4
}