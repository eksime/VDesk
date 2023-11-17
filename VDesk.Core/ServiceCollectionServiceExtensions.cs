using Microsoft.Extensions.DependencyInjection;
using VDesk.Core.Interop;
using VDesk.Core.Interop.Build19044_0000;
using Build10240 = VDesk.Core.Interop.Build10240_0000;
using Build22621 = VDesk.Core.Interop.Build22621_2215;
using Build20348 = VDesk.Core.Interop.Build20348_0000;
using Build22000 = VDesk.Core.Interop.Build22000_0000;
using Build17134 = VDesk.Core.Interop.Build17134_0000;
using VDesk.Utils;

namespace VDesk.Core;

public static class ServiceCollectionServiceExtensions
{
    public static IServiceCollection AddVirtualDesktop(this IServiceCollection services)
    {
        var v = OS.Build;

        if (v >= new Version(10, 0, 22621, 2215))
        {
            services.AddScoped<IVirtualDesktopProvider, VirtualDesktopProvider<Build22621.IVirtualDesktopManagerInternal, Build22621.IVirtualDesktop, Build22621.IApplicationView, Build22621.IApplicationViewCollection>>();
        } 
        else if (v >= new Version(10, 0, 22000, 0))
        {
            services.AddScoped<IVirtualDesktopProvider, VirtualDesktopProvider<Build22000.IVirtualDesktopManagerInternal, Build22000.IVirtualDesktop, Build22000.IApplicationView, Build22000.IApplicationViewCollection>>();
        }
        else if (v >= new Version(10, 0, 20348, 0))
        {
            services.AddScoped<IVirtualDesktopProvider, VirtualDesktopProvider<Build20348.IVirtualDesktopManagerInternal, Build20348.IVirtualDesktop, Build20348.IApplicationView, Build20348.IApplicationViewCollection>>();
        }
        else if (v >= new Version(10, 0, 19044, 0))
        {
            services.AddScoped<IVirtualDesktopProvider, VirtualDesktopProvider<IVirtualDesktopManagerInternal, IVirtualDesktop, IApplicationView, IApplicationViewCollection>>();
        }
        else if (v >= new Version(10, 0, 17134, 0))
        {
            services.AddScoped<IVirtualDesktopProvider, VirtualDesktopProvider<Build17134.IVirtualDesktopManagerInternal, Build17134.IVirtualDesktop, Build17134.IApplicationView, Build17134.IApplicationViewCollection>>();
        }
        else if (v >= new Version(10, 0, 10240, 0))
        {
            services.AddScoped<IVirtualDesktopProvider, VirtualDesktopProvider<Build10240.IVirtualDesktopManagerInternal, Build10240.IVirtualDesktop, Build10240.IApplicationView, Build10240.IApplicationViewCollection>>();
        }
        else
        {
            //throw new NotSupportedException();
        }

        return services;
    }
}