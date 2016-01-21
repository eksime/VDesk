using System;
using libVDesk;
using System.Diagnostics;

namespace VDesk {
  static class Program {
    static void Main(string[] args) {
      if (args.Length == 0) return; //need at least 1 arg.

      Process proc = new Process();
      proc.StartInfo.FileName = args[0];

      if (args.Length > 1) {
        String commandLineArgs = Environment.CommandLine.Remove(0, Environment.CommandLine.IndexOf(" ") + 2); //remove own executable name
        commandLineArgs = commandLineArgs.Remove(0, args[0].Length + (commandLineArgs.StartsWith("\"") ? 3 : 1)); //remove second arg
        proc.StartInfo.Arguments = commandLineArgs; //pass all other args through to second program.
      }

      //Create a new desktop and launch the program; wait for the program to close, and remove the desktop.
      var desk = Desktop.Create(); 
      desk.MakeVisible();

      proc.Start();
      proc.WaitForExit();

      desk.Remove();

      return;
    }
  }
}
