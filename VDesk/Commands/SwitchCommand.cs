using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using VDesk.Services;

namespace VDesk.Commands
{
    [Command(Description = "Switch to a specific virtual desktop")]
    internal class SwitchCommand : VdeskCommandBase
    {
        private readonly IVirtualDesktopService _virtualDesktopService;
        
        public SwitchCommand(IVirtualDesktopService virtualDesktopService)
        {
            _virtualDesktopService = virtualDesktopService;
        }
        
        [Argument(0, Description = "Number of the virtual desktop to go to")]
        [Range(0, 10)]
        public int Number { get; }

        public override int OnExecute(CommandLineApplication app)
        {
            var virtualDesktop = _virtualDesktopService.CreateAndSelect(Number);
            virtualDesktop.Switch();
            return 0;
        }
    }
}