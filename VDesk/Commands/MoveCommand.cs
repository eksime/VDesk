using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows;
using McMaster.Extensions.CommandLineUtils;
using VDesk.Services;

namespace VDesk.Commands
{
    [Command(Description = "Move application already open to a specific desktop")]
    public class MoveCommand : VdeskCommandBase
    {
        private readonly IVirtualDesktopService _virtualDesktopService;
        private readonly IWindowService _windowService;
        
        public MoveCommand(IVirtualDesktopService virtualDesktopService, IWindowService windowService)
        {
            _virtualDesktopService = virtualDesktopService;
            _windowService = windowService;
        }
        
        [Option("-o|--on", CommandOptionType.SingleValue, Description = "Desktop on witch the command is run")]
        [Range(0, 10)]
        public int DesktopNumber { get; set; }

        [Option("-p|--process", CommandOptionType.SingleValue, Description = "Process to move")]
        [Required]
        public string ProcessName { get; set; }
        
        [Option("-n|--no-switch", Description = "Don't switch to virtual desktop")]
        public bool? NoSwitch { get; set; }
        
        [Option("--half-split")]
        [AllowedValues("left", "right", IgnoreCase = true)]
        public string HalfSplit { get; }
        
        public override int OnExecute(CommandLineApplication app)
        {
            var process = Process.GetProcessesByName(ProcessName).FirstOrDefault();
            if (process is null)
            {
                Console.WriteLine($"Process {ProcessName} not found");
                return 1;
            }
            IntPtr hWnd = process.MainWindowHandle;
                
            var targetDesktop = _virtualDesktopService.CreateAndSelect(DesktopNumber);

            _virtualDesktopService.MoveToDesktop(hWnd, targetDesktop);

            if(!string.IsNullOrEmpty(HalfSplit))
                switch (HalfSplit)
                {
                    case "left":
                        _windowService.MoveWindow(hWnd, 0, 0, (int) SystemParameters.PrimaryScreenWidth / 2, (int) SystemParameters.PrimaryScreenHeight, true);
                        break;
                    case "right":
                        _windowService.MoveWindow(hWnd, (int) (SystemParameters.PrimaryScreenWidth / 2 ) + 1, 0, (int) SystemParameters.PrimaryScreenWidth / 2, (int) SystemParameters.PrimaryScreenHeight, true);
                        break;
                }

            if (NoSwitch.HasValue && NoSwitch.Value)
                return 0;

            targetDesktop.Switch();

            return 0;
        }
    }
}