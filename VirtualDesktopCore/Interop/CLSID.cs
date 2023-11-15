namespace WindowsDesktop.Interop;

// ReSharper disable once InconsistentNaming
internal static class CLSID
{
    public static Guid ImmersiveShell { get; } = new("c2f03a33-21f5-47fa-b4bb-156362a2f239");

    public static Guid VirtualDesktopManager { get; } = new("aa509086-5ca9-4c25-8f95-589d3c07b48a");

    public static Guid VirtualDesktopManagerInternal { get; } = new("c5e0cdca-7b6e-41b2-9fc4-d93975cc467b");
}
