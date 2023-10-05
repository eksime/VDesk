using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace VDesk.Commands
{
    [Command(Description = "Switch to a specific virtual desktop")]
    internal class SwitchCommand : VdeskCommandBase
    {
        [Argument(0, Description = "Number of the virtual desktop to go to")]
        [Range(0, 10)]
        private int Number { get; }

        public override int OnExecute(CommandLineApplication app)
        {
            var virtualDesktop = VirtualDesktopHelper.CreateAndSelect(Number);
            virtualDesktop.Switch();
            return 0;
        }
    }
}