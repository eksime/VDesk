using McMaster.Extensions.CommandLineUtils;
using WindowsDesktop;

namespace VDesk_Core.Commands
{
    public static class SwitchCommand
    {
        public static string name => "Switch";
        
        public static void GetCommand(CommandLineApplication createCmd)
        {
            createCmd.Description = "Switch to specific desktop";
            var number = createCmd.Argument<int>("number", "number of the desktop to switch to")
                .Accepts(o => o.Range(1, 10))
                .IsRequired();
            
            createCmd.OnExecute(()  =>
            {
                var virtualDesktop = VirtualDesktopHelper.CreateAndSelect(number.ParsedValue);
                virtualDesktop.Switch();
            });
        }
    }
}