# VDesk

#### Download:

See the [releases page](https://github.com/eksime/VDesk/releases/).

#### Usage:

*Please note that VDesk requires Windows 10, and does not allow the use of virtual desktops on other versions of Windows*

`vdesk create[:n]`

`vdesk [on:<`*`n`*`>] [noswitch:{`*`true`*`|`*`false`*`}] [autoremove:{`*`true`*`|`*`false`*`}] <run:`*`command`*`> [`*`args`*`]`

#### Examples:
Create total of n desktops:

`vdesk create:n`

Run a program on a new desktop:

`vdesk run:command [args]`

> **Note:** If VDesk doesn't work at first, check the program's command line options for ways to create a new window - For example Chrome has the `/new-window` argument which allows it to function with VDesk.

Run a program on a new desktop, and prevent swapping to the new desktop:

`vdesk noswitch:true run:command [args]`

> **Note:** The `noswitch:true` parameter is known to cause applications like Chrome / Skype to not launch on the correct desktop.

Run a program on a new desktop, and close the created desktop when the program exits:

`vdesk autoremove:true run:command [args]`

Run a program on desktop n:

`vdesk on:n run:command [args]`

Run a program on desktop n, and prevent switching to it:

`vdesk on:n noswitch:true run:command [args]`

To launch notepad on a new desktop:

`vdesk run:notepad`

To launch notepad on desktop 3 and open `C:\some file.txt`:

`vdesk on:3 run:notepad "C:\some file.txt"`

To launch a new VirtualBox vm fullscreen on its own virtual desktop, and switch to it:

`vdesk run:"C:\Path to Vbox\VirtualBox.exe" --comment "VM" --startvm "vm-id-no" --fullscreen`

To open Github in a new Chrome window on a new virtual desktop:

`vdesk run:"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" /new-window "https://github.com"`

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
