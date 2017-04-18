# VDesk

#### Download:

See the [releases page](https://github.com/eksime/VDesk/releases/).

#### Usage:

*Please note that VDesk requires Windows 10, and does not allow the use of virtual desktops on other versions of Windows*

Create n new desktops:

`vdesk create [n]`

Create up to n desktops:

`vdesk create-max [n]`

Run a program on a new desktop:

`vdesk run [command] [args]`

> It is common for applications that have a background process (like Chrome / Skype) to not launch on the correct desktop when using a `run` or `run-on` command. Try using a `*-switch` command, and check the program's command line options for ways to create a new window - For example Chrome has the /new-window option, which allows it to work with `*-switch` commands.

Run a program on a new desktop, and switch to it:

`vdesk run-switch [command] [<args>]`

Run a program on desktop n:

`vdesk run-on [n] [command] [<args>]`

Run a program on desktop n, and switch to it:

`vdesk run-on-switch [n] [command] [<args>]`

Generally, prepending a shortcut's 'target' field with `vdesk run` will have it launch in its own virtual desktop. Command line arguments to applications should be preserved.

#### Examples:
To launch notepad on a new desktop:

`vdesk run notepad`

To launch notepad on desktop 3 and open `C:\some file.txt`:

`vdesk run-on 3 notepad "C:\some file.txt"`

To launch a new VirtualBox vm fullscreen on its own virtual desktop, and switch to it:

`vdesk run-switch "C:\Path to Vbox\VirtualBox.exe" --comment "VM" --startvm "vm-id-no" --fullscreen`

To open Github in a new Chrome window on a new virtual desktop:

`vdesk run-switch "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" /new-window "https://github.com"`

## Copyright notice

Copyright (C) 2016

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
