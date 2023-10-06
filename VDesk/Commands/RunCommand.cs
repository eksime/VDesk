using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;
using VDesk.Services;
using VDesk.Utils;

namespace VDesk.Commands
{
    [Command(Description = "Run a command")]
    internal class RunCommand : VdeskCommandBase
    {
        private readonly IVirtualDesktopService _virtualDesktopService;
        private readonly IProcessService _processService;
        private readonly IWindowService _windowService;
        
        public RunCommand(IVirtualDesktopService virtualDesktopService, IProcessService processService, IWindowService windowService)
        {
            _virtualDesktopService = virtualDesktopService;
            _processService = processService;
            _windowService = windowService;

        }
        
        [Option("-o|--on", CommandOptionType.SingleValue, Description = "Desktop on witch the command is run")]
        [Range(1, 100)]
        public int DesktopNumber { get; set; }

        [Argument(0, Description = "Command to execute")]
        [Required]
        public string Command { get; set; }
        
        [Option("-a|--arguments", CommandOptionType.SingleValue, Description = "arguments of the command")]
        public string? Arguments { get; set; }
        
        [Option("-n|--no-switch", Description = "Don't switch to virtual desktop")]
        public bool? NoSwitch { get; set; }
        
        [Option("--half-split")]
        public HalfSplit? HalfSplit { get; set; }

        public override int OnExecute(CommandLineApplication app)
        {
            var targetDesktop = _virtualDesktopService.CreateAndSelect(DesktopNumber);

            if (!NoSwitch.HasValue || !NoSwitch.Value)
                targetDesktop.Switch();

            var proc = _processService.Start(Command, Arguments ?? string.Empty);


            if (NoSwitch.HasValue && NoSwitch.Value)
            {
                proc.WaitForMainWindow();
                
                if (proc.MainWindowHandle.ToInt64() != 0)
                    _virtualDesktopService.MoveToDesktop(proc.MainWindowHandle, targetDesktop);
            }

            proc.WaitForMainWindow();
            _windowService.MoveHalfSplit(proc.MainWindowHandle, HalfSplit);

            return 0;
        }
    }
}