using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace vdesk {
  [Verb("create")]
  class CreateOptions {
    [Value(0)]
    public int desktopIndex { get; set; }
  }

  [Verb("create-max")]
  class CreateMaxOptions {
    [Value(0)]
    public int desktopIndex { get; set; }
  }

  [Verb("run")]
  class RunOptions {
    [Value(0)]
    public string processName { get; set; }
  }

  [Verb("run-switch")]
  class RunSwitchOptions {
    [Value(0)]
    public string processName { get; set; }
  }

  [Verb("run-on")]
  class RunOnOptions {
    [Value(0)]
    public int desktopIndex { get; set; }

    [Value(2)]
    public string processName { get; set; }
  }

  [Verb("run-on-switch")]
  class RunOnSwitchOptions {
    [Value(0)]
    public int desktopIndex { get; set; }

    [Value(1)]
    public string processName { get; set; }
  }

}
