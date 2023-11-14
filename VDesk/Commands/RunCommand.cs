using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
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
        
        public RunCommand(ILogger<RunCommand> logger,IVirtualDesktopService virtualDesktopService, IProcessService processService, IWindowService windowService)
            : base(logger)
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

        public override int Execute(CommandLineApplication app)
        {
            var targetDesktop = _virtualDesktopService.CreateAndSelect(DesktopNumber);

            if (!NoSwitch.HasValue || !NoSwitch.Value)
                _virtualDesktopService.Switch(targetDesktop);
            
            _processService.Start(Command, Arguments ?? string.Empty, out var hWnd);

            if (NoSwitch.HasValue && NoSwitch.Value)
            {
                _virtualDesktopService.MoveToDesktop(hWnd, targetDesktop);
            }
            
            _windowService.MoveHalfSplit(hWnd, HalfSplit);


            return 0;
        }
    }
}