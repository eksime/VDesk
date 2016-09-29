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

      string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();
      string fullCommandline = Environment.CommandLine.Replace(Environment.GetCommandLineArgs()[0], "");

      if (!VirtualDesktop.IsSupported) {
        Console.Error.WriteLine("Error: Virtual Desktops are not supported on this system.");
        Application.Current.Shutdown();
        return;
      }

      Parser parser = new Parser(with => with.IgnoreUnknownArguments = true);
      try {
        var result = parser.ParseArguments<CreateOptions, CreateMaxOptions, RunOptions, RunSwitchOptions, RunOnOptions, RunOnSwitchOptions>(args)
        .WithParsed<CreateOptions>(o => {

          for (int i = 0; i < o.desktopIndex; i++) {
            VirtualDesktop.Create();
          }

        })
        .WithParsed<CreateMaxOptions>(o => {

          createMaxDesktops(o.desktopIndex);

        })
        .WithParsed<RunOptions>(o => {

          string procArgs = getProcessArguments(fullCommandline, o.processName);

          Process proc = Process.Start(o.processName, procArgs);
          launchProcessOnDesktop(proc, VirtualDesktop.GetDesktops().Length + 1);

        })
        .WithParsed<RunSwitchOptions>(o => {

          string procArgs = getProcessArguments(fullCommandline, o.processName);

          Process proc = Process.Start(o.processName, procArgs);
          int lastDesktopIndex = VirtualDesktop.GetDesktops().Length + 1;

          VirtualDesktop lastDesktop = launchProcessOnDesktop(proc, lastDesktopIndex);
          lastDesktop.Switch();

        })
        .WithParsed<RunOnOptions>(o => {

          string procArgs = getProcessArguments(fullCommandline, o.processName);

          Process proc = Process.Start(o.processName, procArgs);
          launchProcessOnDesktop(proc, o.desktopIndex);

        })
        .WithParsed<RunOnSwitchOptions>(o => {

          int offset = fullCommandline.IndexOf(o.processName);
          string procArgs = string.Concat(fullCommandline.Skip(offset + o.processName.Length).SkipWhile(c => c.Equals('"')).SkipWhile(c => c.Equals(' ')));

          Process proc = Process.Start(o.processName, procArgs);
          VirtualDesktop lastDesktop = launchProcessOnDesktop(proc, o.desktopIndex);

          lastDesktop.Switch();
        });

      } catch (Win32Exception) {
        Console.Error.WriteLine("Error: Unable to launch program.");
        return;
      }

      Application.Current.Shutdown();
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
        Thread.Sleep(0);
      }
      VirtualDesktopHelper.MoveToDesktop(proc.MainWindowHandle, getDesktopFromIndex(n));
      return getDesktopFromIndex(n);
    }

    private VirtualDesktop getDesktopFromIndex(int n) {
      return VirtualDesktop.GetDesktops()[n - 1];
    }

    private string getProcessArguments(string fullCommandline, string procName) {
      int offset = fullCommandline.IndexOf(procName);
      return string.Concat(fullCommandline.Skip(offset + procName.Length).SkipWhile(c => c.Equals('"')).SkipWhile(c => c.Equals(' ')));
    }

  }
}
