using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using VDesk.Services;

namespace VDesk.Commands
{
    [Command(Description = "Create virtual desktop")]
    public class CreateCommand : VdeskCommandBase
    {
        private readonly IVirtualDesktopService _virtualDesktopService;
        
        public CreateCommand(ILogger<CreateCommand> logger, IVirtualDesktopService virtualDesktopService)
            : base(logger)
        {
            _virtualDesktopService = virtualDesktopService;
        }

        [Argument(0, Description = "Number of virtual desktop to create")]
        [Range(1, 100)]
        [Required]
        public int Number { get; set; } = 1;
        
        public override int Execute(CommandLineApplication app)
        {
            var difference = Number - _virtualDesktopService.GetDesktops().Length;
            if (difference <= 0)
                return 0;
            
            for (var i = 0; i < difference; i++)
            {
                _virtualDesktopService.Create();
            }
            
            return 0;
        }
    }
}