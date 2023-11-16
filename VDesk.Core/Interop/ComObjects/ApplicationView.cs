using VDesk.Core.Interop.Proxy;

namespace VDesk.Core.Interop.ComObjects;

internal class ApplicationView<TApplicationView> : ComBaseObject<TApplicationView>, IApplicationView
{
    public ApplicationView(object comObject)
        : base(comObject)
    {
    }
}