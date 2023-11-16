using System.Runtime.InteropServices;

namespace VDesk.Core.Interop.Build17134_0000;

[ComImport]
[Guid("871F602A-2B58-42B4-8C4B-6C43D642C06F")]
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
    void SetCloak(Build10240_0000.ApplicationViewCloakType cloakType, int unknown);
    IntPtr GetPosition(in Guid guid, out IntPtr position);
    void SetPosition(in IntPtr position);
    void InsertAfterWindow(IntPtr hwnd);
    Build10240_0000.Rect GetExtendedFramePosition();
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
    Build10240_0000.ApplicationViewCompatibilityPolicy GetCompatibilityPolicyType();
    void SetCompatibilityPolicyType(Build10240_0000.ApplicationViewCompatibilityPolicy flags);
    IntPtr GetPositionPriority();
    void SetPositionPriority(IntPtr priority);
    void GetSizeConstraints(IntPtr monitor, out Build10240_0000.Size size1, out Build10240_0000.Size size2);
    void GetSizeConstraintsForDpi(uint uint1, out Build10240_0000.Size size1, out Build10240_0000.Size size2);
    void SetSizeConstraintsForDpi(ref uint uint1, in Build10240_0000.Size size1, in Build10240_0000.Size size2);
    int QuerySizeConstraintsFromApp();
    void OnMinSizePreferencesUpdated(IntPtr hwnd);
    void ApplyOperation(IntPtr operation);
    bool IsTray();
    bool IsInHighZOrderBand();
    bool IsSplashScreenPresented();
    void Flash();
    Build10240_0000.IApplicationView GetRootSwitchableOwner();
    IObjectArray EnumerateOwnershipTree();
    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetEnterpriseId();
    bool IsMirrored();
}

[ComImport]
[Guid("2C08ADF0-A386-4B35-9250-0FE183476FCC")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IApplicationViewCollection
{
    IObjectArray GetViews();
    IObjectArray GetViewsByZOrder();
    IObjectArray GetViewsByAppUserModelId(string id);
    Build10240_0000.IApplicationView GetViewForHwnd(IntPtr hwnd);
    Build10240_0000.IApplicationView GetViewForApplication(object application);
    Build10240_0000.IApplicationView GetViewForAppUserModelId(string id);
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
    bool IsViewVisible(Build10240_0000.IApplicationView view);
    Guid GetID();
}

[ComImport]
[Guid("F31574D6-B682-4CDC-BD56-1827860ABEC6")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IVirtualDesktopManagerInternal
{
    int GetCount();
    void MoveViewToDesktop(Build10240_0000.IApplicationView pView, Build10240_0000.IVirtualDesktop desktop);
    bool CanViewMoveDesktops(Build10240_0000.IApplicationView pView);
    Build10240_0000.IVirtualDesktop GetCurrentDesktop();
    IObjectArray GetDesktops();
    Build10240_0000.IVirtualDesktop GetAdjacentDesktop(Build10240_0000.IVirtualDesktop pDesktopReference, int uDirection);
    void SwitchDesktop(Build10240_0000.IVirtualDesktop desktop);
    Build10240_0000.IVirtualDesktop CreateDesktop();
    void RemoveDesktop(Build10240_0000.IVirtualDesktop pRemove, Build10240_0000.IVirtualDesktop pFallbackDesktop);
    Build10240_0000.IVirtualDesktop FindDesktop(in Guid desktopId);
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