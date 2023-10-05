using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;
using WindowsDesktop;

namespace VDesk.Commands
{
    [Command(Description = "Run a command")]
    internal class RunCommand : VdeskCommandBase
    {
        [Option("-o|--on", CommandOptionType.SingleValue, Description = "Desktop on witch the command is run")]
        [Range(0, 10)]
        public int DesktopNumber { get; set; }

        [Option("-c|--command", CommandOptionType.SingleValue, Description = "Command to execute")]
        [Required]
        public string Command { get; set; }
        
        [Option("-a|--argument", CommandOptionType.SingleValue, Description = "arguments of the command")]
        public string arguments { get; set; }
        
        [Option("-n|--no-switch", Description = "Don't switch to virtual desktop")]
        public bool? NoSwitch { get; set; }

        public override int OnExecute(CommandLineApplication app)
        {
            var targetDesktop = VirtualDesktopHelper.CreateAndSelect(DesktopNumber);

            if (!NoSwitch.HasValue || !NoSwitch.Value)
                targetDesktop.Switch();

            var startInfo = new ProcessStartInfo(Command, arguments);

            try
            {
                if (Directory.Exists(Path.GetDirectoryName(Command)))
                    startInfo.WorkingDirectory = Path.GetDirectoryName(Command);
            }
            catch
            {
                //Don't really want to do anything here.
            }

            var proc = Process.Start(startInfo);

            if (!NoSwitch.HasValue || !NoSwitch.Value)
                return 0;
                
            for (var backoff = 1; proc.MainWindowHandle.ToInt64() == 0 && backoff <= 0x10000; backoff <<= 1)
                Thread.Sleep(backoff);

            if (proc.MainWindowHandle.ToInt64() != 0)
                VirtualDesktop.MoveToDesktop(proc.MainWindowHandle, targetDesktop);
            return 0;
        }
    }
}