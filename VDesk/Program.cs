using System;
using libVDesk;
using System.Diagnostics;

namespace VDesk {
  static class Program {
    static void Main(string[] args) {
      if (Environment.OSVersion.Version.Major != 10) return; //run only on Windows 10
      if (args.Length == 0) return; //need at least 1 arg.

      //remove own executable name from command line
      String commandLineArgs = Environment.CommandLine.Remove(0, Environment.CommandLine.IndexOf(" ") + 2);

      Process proc = new Process();
      Int32 index = -1; //index of desktop to use. Vlaues < 0 create a new desktop.
      Int32 param = 0; 

      if (int.TryParse(args[param], out index)) {
        commandLineArgs = commandLineArgs.Remove(0, args[0].Length + 1); //remove index from command line
        index--; //use human numbering for desktops {1 ...} rather than {0 ...}
        param = 1; //second param is executable name
      }

      proc.StartInfo.FileName = args[param]; //set executable name

      if (args.Length > param + 1) { //remove second arg & pass all other args through to second program.
        proc.StartInfo.Arguments = commandLineArgs.Remove(0, args[param].Length + (commandLineArgs.StartsWith("\"") ? 3 : 1));
      }
      
      //If we're opening a program on desktop 10, ensure there are 10 desktops.
      for (int i = Desktop.Count - 1; i < index; i++) {
        Desktop.Create();
      }

      //get the desktop, or create a new one, switch to it, and start the program.
      Desktop desk = index < 0 ? Desktop.Create() : Desktop.FromIndex(index);
      desk.MakeVisible();
      proc.Start();
      
      //If we created a desktop just for this program, remove it after the program has finished executing.
      if (index < 0) {
        proc.WaitForExit();
        desk.Remove();
      }

      return;
    }
  }
}
