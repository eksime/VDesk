using System.Diagnostics;

namespace VDesk.Services
{
    public interface IProcessService
    {
        Process? Start(ProcessStartInfo processInfo);

        Process? Start(string command, string arguments);
    }

    public class ProcessService : IProcessService
    {

        public Process? Start(ProcessStartInfo processInfo)
        {
            return Process.Start(processInfo);
        }
        public Process? Start(string command, string arguments)
        {
            var startInfo = new ProcessStartInfo(command, arguments);

            try
            {
                if (Directory.Exists(Path.GetDirectoryName(command)))
                    startInfo.WorkingDirectory = Path.GetDirectoryName(command);
            }
            catch
            {
                //Don't really want to do anything here.
            }

            return Process.Start(startInfo);
        }
    }
}