using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using VDesk.Services;

namespace VDesk.Commands
{
    [Command(Description = "Create virtual desktop")]
    public class CreateCommand : VdeskCommandBase
    {
        private readonly IVirtualDesktopService _virtualDesktopService;
        
        public CreateCommand(IVirtualDesktopService virtualDesktopService)
        {
            _virtualDesktopService = virtualDesktopService;
        }

        [Argument(0, Description = "Number of virtual desktop to create")]
        [Range(0, 10)]
        [Required]
        public int Number { get; set; }
        public override int OnExecute(CommandLineApplication app)
        {
            while (Number > _virtualDesktopService.GetDesktops().Length)
                _virtualDesktopService.Create();
            return 0;
        }
    }
}