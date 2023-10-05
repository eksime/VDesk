using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;
using WindowsDesktop;

namespace VDesk_Core.Commands
{
    public static class RunCommand
    {
        public static string name => "Run";

        public static void GetCommand(CommandLineApplication runCmd)
        {

            var desktopNumber = runCmd.Option<int>("-o|--on", "Desktop on witch the command is run",
                CommandOptionType.SingleValue);
            var command = runCmd.Option("-c|--command", "Command to execute", CommandOptionType.SingleValue)
                .IsRequired();
            var noSwitch =
                runCmd.Option("-n|--no-switch", "Don't switch to virtual desktop", CommandOptionType.NoValue);
            var arguments = runCmd.Option("-a|--argument", "arguments of the command", CommandOptionType.SingleValue);

            runCmd.OnExecute(() =>
            {
                var appPath = command.Value();
                var appArgs = arguments.Value();

                //launch on desktop i
                var targetDesktop = VirtualDesktopHelper.CreateAndSelect(desktopNumber.ParsedValue);

                if (!noSwitch.HasValue())
                    targetDesktop.Switch();


                var startInfo = new ProcessStartInfo(appPath, appArgs);

                try
                {
                    if (Directory.Exists(Path.GetDirectoryName(appPath)))
                        startInfo.WorkingDirectory = Path.GetDirectoryName(appPath);
                }
                catch
                {
                    //Don't really want to do anything here.
                }

                var proc = Process.Start(startInfo);

                if (!noSwitch.HasValue())
                    return;
                
                for (var backoff = 1; proc.MainWindowHandle.ToInt64() == 0 && backoff <= 0x10000; backoff <<= 1)
                    Thread.Sleep(backoff);

                if (proc.MainWindowHandle.ToInt64() != 0)
                    VirtualDesktop.MoveToDesktop(proc.MainWindowHandle, targetDesktop);
            });
        }
    }
}