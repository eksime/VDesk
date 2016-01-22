
# VDesk

Launches programs on their own virtual desktops, and closes the desktop when the program is closed. (Windows 10 only)

Windows10Interop code taken from http://stackoverflow.com/a/32417530

#####Download:

See the [releases page](https://github.com/eksime/VDesk/releases/).

#####Usage:

`vdesk [n] program [args]`

#####Examples:
To launch notepad on a new desktop:

`vdesk notepad`

To launch notepad on desktop 3 and open `C:\some file.txt`:

`vdesk 3 notepad "C:\some file.txt"`

To launch a new VirtualBox vm fullscreen on it's own virtual desktop:

`vdesk "C:\Path to Vbox\VirtualBox.exe" --comment "VM" --startvm "vm-id-no" --fullscreen`

For most shortcuts it is possible to append `vdesk` to the start of the target field in order to start the program on a new virtual desktop, for this to work `vdesk` must be in PATH.

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
