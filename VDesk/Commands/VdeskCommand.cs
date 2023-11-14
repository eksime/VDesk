using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace VDesk.Commands
{
    [Command(Name = "vdesk", FullName = "vdesk", Description = "Manage application accros virtual desktop")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [Subcommand(typeof(CreateCommand), typeof(MoveCommand), typeof(RunCommand), typeof(SwitchCommand))]
    internal class VdeskCommand : VdeskCommandBase
    {
        public override int Execute(CommandLineApplication app)
        {
            Console.WriteLine("Specify a subcommand");
            app.ShowHelp();
            return 1;
        }
        
        private static string GetVersion()
            => typeof(VdeskCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

        public VdeskCommand(ILogger<VdeskCommand> logger)
            : base(logger)
        {
        }
    }
}