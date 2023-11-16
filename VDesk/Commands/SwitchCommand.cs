using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using VDesk.Core;

namespace VDesk.Commands
{
    [Command(Description = "Switch to a specific virtual desktop")]
    internal class SwitchCommand : VdeskCommandBase
    {
        private readonly IVirtualDesktopProvider _virtualDesktopProvider;
        
        public SwitchCommand(ILogger<SwitchCommand> logger, IVirtualDesktopProvider virtualDesktopProvider)
            : base(logger)
        {
            _virtualDesktopProvider = virtualDesktopProvider;
        }
        
        [Argument(0, Description = "Number of the virtual desktop to go to")]
        [Range(1, 100)]
        public int Number { get; }

        public override int Execute(CommandLineApplication app)
        {
            var desktopIds = _virtualDesktopProvider.GetDesktop();

            while (Number > desktopIds.Length)
            {
                desktopIds = desktopIds.Append(_virtualDesktopProvider.Create()).ToArray();
            }

            _virtualDesktopProvider.Switch(desktopIds[Number - 1]);
            
            return 0;
        }
    }
}