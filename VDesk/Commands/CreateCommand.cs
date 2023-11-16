using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using VDesk.Core;

namespace VDesk.Commands
{
    [Command(Description = "Create virtual desktop")]
    public class CreateCommand : VdeskCommandBase
    {
        private readonly IVirtualDesktopProvider _virtualDesktopProvider;
        
        public CreateCommand(ILogger<CreateCommand> logger, IVirtualDesktopProvider virtualDesktopProvider)
            : base(logger)
        {
            _virtualDesktopProvider = virtualDesktopProvider;
        }

        [Argument(0, Description = "Number of virtual desktop to create")]
        [Range(1, 100)]
        [Required]
        public int Number { get; set; } = 1;
        
        public override int Execute(CommandLineApplication app)
        {
            var desktopIds = _virtualDesktopProvider.GetDesktop();

            while (Number > desktopIds.Length)
            {
                desktopIds = desktopIds.Append(_virtualDesktopProvider.Create()).ToArray();
            }
            
            return 0;
        }
    }
}