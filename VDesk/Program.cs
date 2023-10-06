using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VDesk.Commands;
using VDesk.Services;
using WindowsDesktop;

namespace VDesk;

static class VDesk
{
    [STAThread]
    public static int Main(string[] args)
    {
        if (VirtualDesktop.IsSupported)
            return MainAsync(args).GetAwaiter().GetResult();
        Console.WriteLine("Virtual Desktops are not supported on this system.");
        return 1;
    }

    static async Task<int> MainAsync(string[] args)
    {
        return await new HostBuilder().ConfigureLogging((context, builder) =>
            {
                builder.AddConsole();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<IVirtualDesktopService, VirtualDesktopService>();
                services.AddScoped<IWindowService, WindowService>();
                services.AddScoped<IProcessService, ProcessService>();
            })
            .RunCommandLineApplicationAsync<VdeskCommand>(args);
    }
}