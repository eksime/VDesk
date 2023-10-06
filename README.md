# VDesk
---
This is a fork of [eksime/VDesk](https://github.com/eksime/VDesk) that support Windows 10 and 11.
The main reason of the fork is that the original project seam to be abadonned.

> **Note :** due to migration in .netcore, the command argument have change. If you install this version, you will need to addapt your usage of vdesk.

#### Download:

See the [releases page](https://github.com/eksime/VDesk/releases/).

#### Requirements
- Windows 10 build 19041 (20H1) or later
- Windows 11 (Not tested yet)

#### Usage:

`vdesk create <number>`

`vdesk run [options] <Command>`

`vdesk move [options] <ProcessName>`

`vdesk switch <Number>`

Run `vdesk [command] --help` for more information about a command.

#### Examples:
Create total of 5 desktops:

`vdesk create 5`

Run notepad on virtual desktop 4 without switching:

`vdesk run -n -o 4 notepad.exe`

> **Note:** If VDesk doesn't work at first, check the program's command line options for ways to create a new window - For example Chrome has the `/new-window` argument which allows it to function with VDesk.

Move notepad on virtual desktop 5 half split to right

`vdesk move --half-split right -o 5 notepad`

Run a program on desktop n, and prevent switching to it:

`vdesk run --on n --no-switch <command>`

To launch notepad on current desktop:

`vdesk run notepad`

To launch notepad on desktop 3 and open `C:\some file.txt`:

`vdesk run -o 3 notepad -a "C:\some file.txt"`

To launch a new VirtualBox vm fullscreen on its own virtual desktop, and switch to it:

`vdesk run "C:\Path to Vbox\VirtualBox.exe" -a "--comment \"VM\" --startvm \"vm-id-no\" --fullscreen"`
> **Note:** to be tested !!

To open Github in a new Chrome window on the second desktop split half screen to the left:

`vdesk run -o 2 --half-split left "C:\Program Files\Google\Chrome\Application\chrome.exe" -a "/new-window https://github.com"`

## Copyright notice

Copyright (C) 2018

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
