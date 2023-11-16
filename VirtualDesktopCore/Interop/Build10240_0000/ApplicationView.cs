using WindowsDesktop.Interop.Proxy;

namespace WindowsDesktop.Interop.Build10240;

internal class ApplicationView : ComBaseObjectLegacy<IApplicationView>, IApplicationView
{
    public ApplicationView(ComInterfaceAssembly assembly, object comObject)
        : base(assembly, comObject)
    {
    }
}
