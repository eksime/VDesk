using System;
using libVDesk;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Microsoft.Win32;

namespace VDesk {
  static class Program {
    static readonly int executable = 0;
    static readonly int desktopIndex = 1;
    static readonly int command = 2;
    static readonly int arguments = 3;

    
    static void Main(string[] args) {
      if (args.Length == 0) return; //need at least 1 arg.

      switch (args[0]) {
        case "-install": install(); return;  //add registry entries for context menu
        case "-uninstall": uninstall(); return;  //remove registry entries for context menu
        default: run(); return;
      }
    }

    static void install() {
      RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Classes\*\shell", true);

      RegistryKey vdesk = key.CreateSubKey("VDesk");
      vdesk.SetValue("", "Open in new virtual desktop");

      RegistryKey command = vdesk.CreateSubKey("command");
      command.SetValue("", "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\" %*");

      command.Close();
      vdesk.Close();
      key.Close();
    }

    static void uninstall() {
      RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Classes\*\shell", true);

      if (key.OpenSubKey("VDesk") != null) //check key exists
        key.DeleteSubKeyTree("VDesk");
        
      key.Close();
    }

    static void run() {
      String[] clArgs = parseCommandLine(Environment.CommandLine);
      Process proc = new Process();

      int index = int.Parse(clArgs[desktopIndex]) - 1; //set desktop index
      proc.StartInfo.FileName = clArgs[command]; //set executable name
      proc.StartInfo.Arguments = clArgs[arguments]; //set arg list

      //If we're opening a program on desktop 10, ensure there are 10 desktops.
      for (int i = Desktop.Count - 1; i < index; i++)
        Desktop.Create();

      //get the desktop, or create a new one
      Desktop desk = index < 0 ? Desktop.Create() : Desktop.FromIndex(index);

      if (!clArgs[executable].Equals("")) { //if we're launching a program:
        try {
          //swtich to the desktop and try starting the program
          desk.MakeVisible();
          proc.Start();

        }
        catch (Win32Exception) {
          //Error launching program.
          Console.Error.WriteLine("Failed to start program.\nCheck executable path.");

        }
        finally {
          //If we created a desktop just for this program, remove it after the program has finished executing.
          if (index < 0) {
            proc.WaitForExit();
            desk.Remove();
          }

        }
      }

      return;
    }

    static String[] parseCommandLine(String cla) {
      String[] ret = new string[4];

      GroupCollection groups = Regex.Match(cla, @"(\""[^\""]+\""|[\w-:_\/\.\\]+) +(?:(-?\d+) ?)?(\""[^\""]+\""|[\w-:_\/.\\]+)? ?(.*)").Groups;
      
      for (int i = 1; i < 4; i++)
        ret[i-1] = groups[i].Value; //set return values
      
      if (ret[desktopIndex].Equals(""))
        ret[desktopIndex] = "0";  //if index is empty, set index = 0
      
      return ret;
    }
  }
}
