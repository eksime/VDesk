using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace VDesk {
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer {
        public Installer() {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver) {
            base.Install(stateSaver);

            string targetDir = Context.Parameters["TARGETDIR"];
            PathAdd(targetDir);
        }

        public override void Rollback(IDictionary savedState) {
            base.Rollback(savedState);

            string targetDir = Context.Parameters["TARGETDIR"];
            PathRemove(targetDir);
        }

        public override void Uninstall(IDictionary savedState) {
            base.Uninstall(savedState);

            string targetDir = Context.Parameters["TARGETDIR"];
            PathRemove(targetDir);
        }


        public void PathAdd(string directory) {
            directory = directory.TrimEnd('\\');

            List<string> path = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine).Split(';').ToList();
            path.Add(directory);

            string pathStr = string.Join(";", path);
            Environment.SetEnvironmentVariable("PATH", pathStr, EnvironmentVariableTarget.Machine);
        }

        public void PathRemove(string directory) {
            directory = directory.TrimEnd('\\');

            List<string> path = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine).Split(';').ToList();
            path.Remove(directory);

            string pathStr = string.Join(";", path);
            Environment.SetEnvironmentVariable("PATH", pathStr, EnvironmentVariableTarget.Machine);
        }
    }
}
