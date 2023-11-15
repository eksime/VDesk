using WindowsDesktop.Interop.Proxy;

namespace WindowsDesktop.Interop.Build10240;

internal class ApplicationView : ComBaseObject<IApplicationView>, IApplicationView
{
    public ApplicationView(ComInterfaceAssembly assembly, object comObject)
        : base(assembly, comObject)
    {
    }
}
