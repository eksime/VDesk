using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using WindowsDesktop;

namespace VDesk.Commands
{
    [Command(Description = "Create virtual desktop")]
    public class CreateCommand : VdeskCommandBase
    {
        [Argument(0, Description = "Number of virtual desktop to create")]
        [Range(0, 10)]
        [Required]
        public int Number { get; set; }
        
        public override int OnExecute(CommandLineApplication app)
        {
            while (Number > VirtualDesktop.GetDesktops().Length)
                VirtualDesktop.Create();
            return 0;
        }
    }
}