using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows;
using Windows.Foundation.Metadata;
using McMaster.Extensions.CommandLineUtils;
using VDesk.Services;
using VDesk.Utils;

namespace VDesk.Commands
{
    [Command(Description = "Move application already open to a specific desktop")]
    public class MoveCommand : VdeskCommandBase
    {
        private readonly IVirtualDesktopService _virtualDesktopService;
        private readonly IWindowService _windowService;

        public MoveCommand(IVirtualDesktopService virtualDesktopService, IWindowService windowService)
        {
            _virtualDesktopService = virtualDesktopService;
            _windowService = windowService;
        }

        [Option("-o|--on", CommandOptionType.SingleValue, Description = "Desktop on witch the command is run")]
        [System.ComponentModel.DataAnnotations.Range(1, 10)]
        public int DesktopNumber { get; set; } = 1;

        [Argument(0, Description = "Process to move")]
        [Required]
        public string ProcessName { get; set; }

        [Option("-n|--no-switch", Description = "Don't switch to virtual desktop")]
        public bool? NoSwitch { get; set; }

        [Option("--half-split")]
        public HalfSplit? HalfSplit { get; }

        public override int OnExecute(CommandLineApplication app)
        {
            var process = Process.GetProcessesByName(ProcessName).FirstOrDefault();
            if (process is null)
            {
                Console.WriteLine($"Process {ProcessName} not found");
                return 1;
            }
            var hWnd = process.MainWindowHandle;

            var targetDesktop = _virtualDesktopService.CreateAndSelect(DesktopNumber);
            _virtualDesktopService.MoveToDesktop(hWnd, targetDesktop);
           _windowService.MoveHalfSplit(hWnd, HalfSplit); 

            if (NoSwitch.HasValue && NoSwitch.Value)
                return 0;

            targetDesktop.Switch();

            return 0;
        }
    }
}