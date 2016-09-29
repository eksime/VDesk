using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace vdesk {
  [Verb("run", HelpText = "Add file contents to the index.")]
  class RunOptions {
    [Value(0)]
    public string processName { get; set; }
  }

  [Verb("on", HelpText = "Add file contents to the index.")]
  class RunOnOptions {

    [Value(0)]
    public int desktopIndex { get; set; }

    [Value(1)]
    public string command { get; set; }

    [Value(2)]
    public string processName { get; set; }
  }

  [Verb("create", HelpText = "Add file contents to the index.")]
  class CreateOptions {
    [Value(0)]
    public string command { get; set; }

    [Value(1)]
    public int desktopIndex { get; set; }
  }
}
