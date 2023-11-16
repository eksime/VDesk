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
        
        protected readonly ILogger<VdeskCommandBase> Logger;
        
        protected VdeskCommandBase(ILogger<VdeskCommandBase> logger)
        {
            Logger = logger;
        }
        public abstract int Execute(CommandLineApplication app);

        // ReSharper disable once UnusedMember.Global
        public int OnExecute(CommandLineApplication app)
        {
            try
            {
                if (Verbose.HasValue && Verbose.Value)
                {
                    //_logger.LogInformation($"*** {AssemblyInfo.Title} Library ***");
                    //_logger.LogInformation($"Version:  {AssemblyInfo.VersionString}");
                    Logger.LogInformation($"OS Build: {OS.Build}");
                    //_logger.LogInformation($"Provider: {tProvider.GeType().Name}");
                }
                return Execute(app);
            }
            catch (Exception e)
            {
                if(Verbose.HasValue && Verbose.Value) Logger.LogError(e, $"{e.Message}\n\r \tWindows version: {OS.Build} ");
                else Logger.LogError($"{e.Message}\n\r \tWindows version: {OS.Build}");
                return 1;
            }
        }
    }
}