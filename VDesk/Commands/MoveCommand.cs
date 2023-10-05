using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;
using WindowsDesktop;

namespace VDesk_Core.Commands
{
    public static class MoveCommand
    {
        public static string name => "Move";

        public static void GetCommand(CommandLineApplication moveCmd)
        {
            moveCmd.Description = "Move application already open to a specific desktop";
            var desktopNumber = moveCmd.Option<int>("-o|--on", "Desktop on witch the command is run",
                CommandOptionType.SingleValue);
            var processName = moveCmd.Option("-p|--process", "Process to move", CommandOptionType.SingleValue)
                .IsRequired();
            var noSwitch = moveCmd.Option("-n|--no-switch", "Don't switch to virtual desktop",
                CommandOptionType.NoValue);

            moveCmd.OnExecute(() =>
            {
                var process = Process.GetProcessesByName(processName.Value()).FirstOrDefault();
                if (process is null)
                {
                    Console.WriteLine($"Process {processName.Value()} not found");
                    return;
                }
                IntPtr hWnd = process.MainWindowHandle;
                
                var targetDesktop = VirtualDesktopHelper.CreateAndSelect(desktopNumber.ParsedValue);

                VirtualDesktop.MoveToDesktop(hWnd, targetDesktop);

                if (noSwitch.HasValue())
                    return;

                targetDesktop.Switch();

            });
        }
    }
}