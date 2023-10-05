using McMaster.Extensions.CommandLineUtils;
using VDesk.Commands;
using WindowsDesktop;

namespace VDesk;

static class VDesk
{
    [STAThread]
    public static int Main(string[] args)
    {
        if (VirtualDesktop.IsSupported)
            return CommandLineApplication.Execute<VdeskCommand>(args);
        Console.WriteLine("Virtual Desktops are not supported on this system.");
        return 1;

    }
}