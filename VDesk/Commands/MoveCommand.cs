using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using VDesk.Core;
using VDesk.Services;
using VDesk.Utils;

namespace VDesk.Commands
{
    [Command(Description = "Move application already open to a specific desktop")]
    public class MoveCommand : VdeskCommandBase
    {
        private readonly IVirtualDesktopProvider _virtualDesktopProvider;
        private readonly IWindowService _windowService;
        private readonly IProcessService _processService;

        public MoveCommand(ILogger<MoveCommand> logger, IWindowService windowService, IProcessService processService, IVirtualDesktopProvider virtualDesktopProvider)
            : base(logger)
        {
            _windowService = windowService;
            _processService = processService;
            _virtualDesktopProvider = virtualDesktopProvider;
        }

        [Option("-o|--on", CommandOptionType.SingleValue, Description = "Desktop on witch the command is run")]
        [Range(1, 10)]
        public int DesktopNumber { get; set; } = 1;

        [Argument(0, Description = "Process to move")]
        [Required]
        public string ProcessName { get; set; }

        [Option("-n|--no-switch", Description = "Don't switch to virtual desktop")]
        public bool? NoSwitch { get; set; }

        [Option("--half-split")]
        public HalfSplit? HalfSplit { get; set; }

        public override int Execute(CommandLineApplication app)
        {
            var process = Process.GetProcessesByName(ProcessName).FirstOrDefault();
            if (process is null)
            {
                Console.WriteLine($"Process {ProcessName} not found");
                return 1;
            }
            
            var hWnd = _processService.GetMainWindowHandle(process);
            var desktopIds = _virtualDesktopProvider.GetDesktop();

            while (DesktopNumber > desktopIds.Length)
            {
                desktopIds = desktopIds.Append(_virtualDesktopProvider.Create()).ToArray();
            }

            var desktopId = desktopIds[DesktopNumber - 1];
            
            _virtualDesktopProvider.MoveToDesktop(hWnd, desktopId);
            
           _windowService.MoveHalfSplit(hWnd, HalfSplit); 

            if (NoSwitch.HasValue && NoSwitch.Value)
                return 0;

            _virtualDesktopProvider.Switch(desktopId);

            return 0;
        }
    }
}