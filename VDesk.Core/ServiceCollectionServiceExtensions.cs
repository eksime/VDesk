using Microsoft.Extensions.DependencyInjection;
using VDesk.Core.Interop;
using VDesk.Core.Interop.Build22621_2215;
using VDesk.Utils;

namespace VDesk.Core;

public static class ServiceCollectionServiceExtensions
{
    public static IServiceCollection AddVirtualDesktop(this IServiceCollection services)
    {
        Version v = OS.Build;

        if (v >= new Version(10, 0, 22621, 2215))
        {
            services.AddScoped<IVirtualDesktopProvider, VirtualDesktopProvider22621>();
        } 
        else if (v >= new Version(10, 0, 22000, 0))
        {
            //return new VirtualDesktopProvider22000();
        }
        else if (v >= new Version(10, 0, 20348, 0))
        {
            //return new VirtualDesktopProvider20348();
        }
        else if (v >= new Version(10, 0, 10240, 0))
        {
            //return new VirtualDesktopProvider10240();
        }
        else
        {
            services.AddScoped<IVirtualDesktopProvider, NotSupportedVirtualDesktop>();
        }

        return services;
    }
}