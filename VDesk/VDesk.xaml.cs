using CommandLine;
using System;
using System.Linq;
using System.Windows;
using WindowsDesktop;

using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace vdesk {
  public partial class VDesk : Window {

    public VDesk() {
      InitializeComponent();

      // Get commmandline args:
      string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray(); // args to vdesk as array

      if (!VirtualDesktop.IsSupported) {
        Console.Error.WriteLine("Error: Virtual Desktops are not supported on this system."); // probably running xp or something
        Application.Current.Shutdown();
        return;
      }

      Parser parser = new Parser(with => with.IgnoreUnknownArguments = true); // the args to pass to process.start are not handled by Parser
      try {
        var result = parser.ParseArguments<CreateOptions, CreateMaxOptions, RunOptions, RunSwitchOptions, RunOnOptions, RunOnSwitchOptions>(args)
          .WithParsed<CreateOptions>(o => {
            // vdesk create [o.desktopIndex]
            for (int i = 0; i < o.desktopIndex; i++) {
              VirtualDesktop.Create();
            }

          })
          .WithParsed<CreateMaxOptions>(o => {
            //vdesk create-max [o.desktopIndex]
            createMaxDesktops(o.desktopIndex);

          })
          .WithParsed<RunOptions>(o => {
            // vdesk run [o.processName] [<procArgs>]
            Process proc = Process.Start(o.processName, getProcessArguments(Environment.CommandLine, o.processName));
            launchProcessOnDesktop(proc, VirtualDesktop.GetDesktops().Length + 1);

          })
          .WithParsed<RunSwitchOptions>(o => {
            // vdesk run-switch [o.processName] [<procArgs>]
            Process proc = Process.Start(o.processName, getProcessArguments(Environment.CommandLine, o.processName));

            int lastDesktopIndex = VirtualDesktop.GetDesktops().Length + 1;
            VirtualDesktop lastDesktop = launchProcessOnDesktop(proc, lastDesktopIndex);
            lastDesktop.Switch();

          })
          .WithParsed<RunOnOptions>(o => {
            // vdesk run-on [o.desktopIndex] [o.processName] [<procArgs>]
            Debug.Print(getProcessArguments(Environment.CommandLine, o.processName));
            Process proc = Process.Start(o.processName, getProcessArguments(Environment.CommandLine, o.processName));
            launchProcessOnDesktop(proc, o.desktopIndex);

          })
          .WithParsed<RunOnSwitchOptions>(o => {
            // vdesk run-on-switch [o.desktopIndex] [o.processName] [<procArgs>]

            Process proc = Process.Start(o.processName, getProcessArguments(Environment.CommandLine, o.processName));
            VirtualDesktop lastDesktop = launchProcessOnDesktop(proc, o.desktopIndex);

            lastDesktop.Switch();
          });

      } catch (Win32Exception) {
        Console.Error.WriteLine("Error: Unable to launch program.");
      } finally {
        Application.Current.Shutdown();
      }
      return;
    }

    private void createMaxDesktops(int n) {
      VirtualDesktop[] desktops = VirtualDesktop.GetDesktops();
      for (int i = desktops.Length; i < n; i++) {
        VirtualDesktop.Create();
      }
    }

    private VirtualDesktop launchProcessOnDesktop(Process proc, int n) {
      createMaxDesktops(n);
      while (proc.MainWindowHandle.ToInt64() == 0) {
        // spawning the process can be slow, wait for a few ms until the process has created a main window.
        // TODO: exit this while loop and do not call .MoveToDesktop if the process doesn't yeild a main window handle in a reasonable timeframe.
        Thread.Sleep(0);
      }
      VirtualDesktopHelper.MoveToDesktop(proc.MainWindowHandle, getDesktopFromIndex(n));
      return getDesktopFromIndex(n);
    }

    private VirtualDesktop getDesktopFromIndex(int n) {
      return VirtualDesktop.GetDesktops()[n - 1];
    }

    private string getProcessArguments(string fullCommandline, string procName) {
      /* Explanation:
       * offset is the index of the end of the process name within the full commandline argument
       * 
       * example: offset = 42
       *                                           v
       * "/somepath/vdesk.exe" "some executable.exe" -some args "some more args" 
       * 
       * We skip the offset, producing:
       * 
       * " -some args "some more args" 
       * 
       * Then if the next char is '"' we skip that,
       * and if the next character is ' ' (it should be) we skip that too. 
       * Leaving what should be the intact args to pass to the process:
       * 
       * -some args "some more args" 
       * 
       */
      int offset = fullCommandline.IndexOf(procName) + procName.Length;
      return string.Concat(fullCommandline.Skip(offset).SkipWhile(c => c.Equals('"')).SkipWhile(c => c.Equals(' ')));
    }

  }
}
