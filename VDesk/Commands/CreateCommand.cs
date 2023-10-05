using McMaster.Extensions.CommandLineUtils;
using WindowsDesktop;

namespace VDesk_Core.Commands
{
    public static class CreateCommand
    {
        public static string name => "Create";
        
        public static void GetCommand(CommandLineApplication createCmd)
        {
            createCmd.Description = "Create desktop";
            var number = createCmd.Argument<int>("number", "Name of the config")
                .Accepts(o => o.Range(1, 10))
                .IsRequired();
            
            createCmd.OnExecute(()  =>
            {
                
                while (number.ParsedValue > VirtualDesktop.GetDesktops().Length)
                    VirtualDesktop.Create();
            });
        }
    }
}