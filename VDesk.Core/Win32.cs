using System.Runtime.InteropServices;

namespace VDesk.Core;

[ComImport]
[Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IServiceProvider
{
    [return: MarshalAs(UnmanagedType.IUnknown)]
    object QueryService(in Guid guidService, in Guid riid);
}

[ComImport]
[Guid("92ca9dcd-5622-4bba-a805-5e9f541bd8c9")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IObjectArray
{
    uint GetCount();

    [return: MarshalAs(UnmanagedType.Interface)]
    object GetAt(uint iIndex, in Guid riid);
}