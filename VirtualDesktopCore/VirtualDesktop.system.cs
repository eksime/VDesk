using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsDesktop.Interop;
using WindowsDesktop.Interop.Build10240;
using WindowsDesktop.Interop.Build20348;
using WindowsDesktop.Interop.Build22000;
using WindowsDesktop.Interop.Build22621_2215;
using WindowsDesktop.Interop.Proxy;
using WindowsDesktop.Properties;
using WindowsDesktop.Utils;

namespace WindowsDesktop;

partial class VirtualDesktop
{
    private static readonly VirtualDesktopProvider Provider;
    private static readonly ConcurrentDictionary<Guid, VirtualDesktop> KnownDesktops = new();
    //private static readonly ExplorerRestartListenerWindow ExplorerRestartListener = new(() => HandleExplorerRestarted());
    private static ComInterfaceAssembly? _assembly;
    private readonly IVirtualDesktop _source;
    private string _name;

    /// <summary>
    /// Gets a value indicating virtual desktops are supported by the host.
    /// </summary>
    public static bool IsSupported
        => Provider.IsSupported;

    static VirtualDesktop()
    {
        
        Provider = CreateProvider();

        Debug.WriteLine($"*** {AssemblyInfo.Title} Library ***");
        Debug.WriteLine($"Version:  {AssemblyInfo.VersionString}");
        Debug.WriteLine($"OS Build: {OS.Build}");
        Debug.WriteLine($"Provider: {Provider.GetType().Name}");
    }

    /// <summary>
    ///  Create provider for current OS version.
    ///  Test order matters. Make sure you test the highest version first.
    /// </summary>
    /// <returns></returns>
    private static VirtualDesktopProvider CreateProvider()
    {
        Version v = OS.Build;

        if (v >= new Version(10, 0, 22621, 2215))
        {
            return new VirtualDesktopProvider22621();
        }

        if (v >= new Version(10, 0, 22000, 0))
        {
            return new VirtualDesktopProvider22000();
        }

        if (v >= new Version(10, 0, 20348, 0))
        {
            return new VirtualDesktopProvider20348();
        }
        
        if (v >= new Version(10, 0, 10240, 0))
        {
            return new VirtualDesktopProvider10240();
        }
       
        return new NotSupportedVirtualDesktop();
    }


    private VirtualDesktop(IVirtualDesktop source)
    {
        _source = source;
        _name = source.GetName();
        Id = source.GetID();
    }

    internal static VirtualDesktop FromComObject(IVirtualDesktop desktop)
        => KnownDesktops.GetOrAdd(desktop.GetID(), _ => new VirtualDesktop(desktop));


    private static T? SafeInvoke<T>(Func<T> action, params HResult[] hResult)
    {
        try
        {
            return action();
        }
        catch (COMException ex) when (ex.Match(hResult is { Length: 0 } ? new[] { HResult.TYPE_E_ELEMENTNOTFOUND, } : hResult))
        {
            return default;
        }
    }

    private static bool SafeInvoke(Action action, params HResult[] hResult)
    {
        try
        {
            action();
            return true;
        }
        catch (COMException ex) when (ex.Match(hResult is { Length: 0 } ? new[] { HResult.TYPE_E_ELEMENTNOTFOUND, } : hResult))
        {
            return false;
        }
    }
}
