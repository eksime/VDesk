using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using VDesk.Core;
using VDesk.Services;
using VDesk.Utils;

namespace VDesk.Commands
{
    [Command(Description = "Run a command")]
    internal class RunCommand : VdeskCommandBase
    {
        private readonly IVirtualDesktopProvider _virtualDesktopProvider;
        private readonly IProcessService _processService;
        private readonly IWindowService _windowService;

        public RunCommand(ILogger<RunCommand> logger,
            IProcessService processService, IWindowService windowService,
            IVirtualDesktopProvider virtualDesktopProvider)
            : base(logger)
        {
            _processService = processService;
            _windowService = windowService;
            _virtualDesktopProvider = virtualDesktopProvider;
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

        [Option("--half-split")] public HalfSplit? HalfSplit { get; set; }

        public override int Execute(CommandLineApplication app)
        {
            if (Verbose.HasValue && Verbose.Value)
            {
                Logger.LogInformation($"Provider: {_virtualDesktopProvider.GetType()}");
            }

            var desktopIds = _virtualDesktopProvider.GetDesktop();

            while (DesktopNumber > desktopIds.Length)
            {
                desktopIds = desktopIds.Append(_virtualDesktopProvider.Create()).ToArray();
            }

            var desktopId = desktopIds[DesktopNumber - 1];

            if (!NoSwitch.HasValue || !NoSwitch.Value)
                _virtualDesktopProvider.Switch(desktopId);

            _processService.Start(Command, Arguments ?? string.Empty, out var hWnd);

            if (NoSwitch.HasValue && NoSwitch.Value)
            {
                // For unknown reason, without it the view is not found
                Thread.Sleep(1);
                _virtualDesktopProvider.MoveToDesktop(hWnd, desktopId);
            }

            _windowService.MoveHalfSplit(hWnd, HalfSplit);


            return 0;
        }
    }
}