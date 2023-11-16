using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VDesk.Commands;
using VDesk.Core;
using VDesk.Services;

namespace VDesk;

static class VDesk
{
    [STAThread]
    public static int Main(string[] args)
    {
        return MainAsync(args).GetAwaiter().GetResult();
    }

    static async Task<int> MainAsync(string[] args)
    {
        return await new HostBuilder().ConfigureLogging((context, builder) =>
            {
                builder.AddConsole(configure => configure.FormatterName = Microsoft.Extensions.Logging.Console.ConsoleFormatterNames.Systemd);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<IWindowService, WindowService>();
                services.AddScoped<IProcessService, ProcessService>();
                services.AddVirtualDesktop();
            })
            .RunCommandLineApplicationAsync<VdeskCommand>(args);
    }
}