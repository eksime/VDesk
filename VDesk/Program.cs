using McMaster.Extensions.CommandLineUtils;
using VDesk_Core.Commands;
using WindowsDesktop;

namespace VDesk_Core;

static class VDesk
{
    [STAThread]
    public static int Main(string[] args)
    {
        if (!VirtualDesktop.IsSupported)
        {
            Console.WriteLine("Virtual Desktops are not supported on this system.");
            return 1;
        }
        
        var app = new CommandLineApplication
        {
            Name = "vdesk",
            Description = "Manage application accros virtual desktop",
        };
        
        app.HelpOption(inherited: true);
        app.Command(CreateCommand.name, CreateCommand.GetCommand);
        app.Command(RunCommand.name, RunCommand.GetCommand);
        app.Command(MoveCommand.name, MoveCommand.GetCommand);
        app.Command(SwitchCommand.name, SwitchCommand.GetCommand);
        
        app.OnExecute(() =>
        {
            Console.WriteLine("Specify a subcommand");
            app.ShowHelp();
            return 1;
        });

        return app.Execute(args);
    }
}