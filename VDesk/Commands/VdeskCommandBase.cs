using McMaster.Extensions.CommandLineUtils;

namespace VDesk.Commands
{
    [HelpOption("--help")]
    public abstract class VdeskCommandBase
    {
        public abstract int OnExecute(CommandLineApplication app);
    }
}