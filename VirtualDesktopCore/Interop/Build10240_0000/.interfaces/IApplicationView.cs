using System;
using System.Runtime.InteropServices;

namespace WindowsDesktop.Interop.Build10240
{
    // ## Fixes for breaking changes in .NET 5
    // * InterfaceIsIInspectable -> InterfaceIsIUnknown
    // * Add three dummy entries to the start of the interface; Proc3() - Proc5()
    // 
    // see also: https://docs.microsoft.com/en-us/dotnet/core/compatibility/interop/5.0/casting-rcw-to-inspectable-interface-throws-exception

    [ComImport]
    [Guid("00000000-0000-0000-0000-000000000000") /* replace at runtime */]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IApplicationView
    {

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
}
