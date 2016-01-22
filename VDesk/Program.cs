using System;
using libVDesk;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace VDesk {
  static class Program {
    static readonly int desktopIndex = 0;
    static readonly int executable = 1;
    static readonly int arguments = 2;

    static void Main(string[] args) {
      if (Environment.OSVersion.Version.Major != 10) return; //run only on Windows 10
      if (args.Length == 0) return; //need at least 1 arg.

      String[] clArgs = parseArgs(Environment.CommandLine);
      Process proc = new Process();

      int index = int.Parse(clArgs[desktopIndex]) - 1; //set desktop index
      proc.StartInfo.FileName = clArgs[executable]; //set executable name
      proc.StartInfo.Arguments = clArgs[arguments]; //set arg list

      //If we're opening a program on desktop 10, ensure there are 10 desktops.
      for (int i = Desktop.Count - 1; i < index; i++) 
        Desktop.Create();
      
      //get the desktop, or create a new one, and switch to it
      Desktop desk = index < 0 ? Desktop.Create() : Desktop.FromIndex(index);
      desk.MakeVisible();

      try {
        //try starting the program
        proc.Start();

      } catch (Win32Exception) {
        //Error launching program.
        Console.Error.WriteLine("Failed to start program.\nCheck executable path.");

      } finally {
        //If we created a desktop just for this program, remove it after the program has finished executing.
        if (index < 0) {
          proc.WaitForExit();
          desk.Remove();
        }

      }

      return;
    }

    static String[] parseArgs(String cla) {
      String[] ret = new string[3];
      cla = cla.Remove(0, cla.IndexOf("  ")).Trim(); //remove executable name
      GroupCollection groups = Regex.Match(cla, "(?:(-?\\d+) )?(\"[^\"]+\"|[\\w-:_/.\\\\]+) ?(.*)").Groups;
      
      for (int i = 1; i < 4; i++)
        ret[i-1] = groups[i].Value;
      
      if (ret[0].Equals(""))
        ret[0] = "-1";

      return ret;
    }
  }
}
