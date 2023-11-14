using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using VDesk.Utils;

namespace VDesk.Commands
{
    [HelpOption("--help")]
    public abstract class VdeskCommandBase
    {
        [Option("-v|--verbose", Description = "user verbose")]
        public bool? Verbose { get; set; }
        
        private readonly ILogger<VdeskCommandBase> _logger;
        
        protected VdeskCommandBase(ILogger<VdeskCommandBase> logger)
        {
            _logger = logger;
        }
        public abstract int Execute(CommandLineApplication app);

        public int OnExecute(CommandLineApplication app)
        {
            try
            {
                return Execute(app);
            }
            catch (Exception e)
            {
                if(Verbose.HasValue && Verbose.Value) _logger.LogError(e, $"{e.Message}\n\r \tWindows version: {OS.Build} ");
                else _logger.LogError($"{e.Message}\n\r \tWindows version: {OS.Build}");
                return 1;
            }
        }
    }
}