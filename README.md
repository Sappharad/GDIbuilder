GDIbuilder
==========

A utility to assist with building, modifying or extracting Dreamcast .gdi images.

In build mode, when provided with a folder of data files, the IP.BIN bootsector, and optional raw 
CDDA tracks, this tool will automatically generate the data track(s) for the high density area of a 
GD-ROM image. It also generates the track TOC which is written into the bootsector.

In patch mode, when provided with an existing .gdi and a folder containing data files, this tool
can create a modified copy of an existing disc. Files in the data folder in the same location as the
original disc will be replaced in the copy, and files that don't exist in the original will be added.

In navigate or extract mode, the contents of the high density area of a disc image can be viewed.
These files along with IP.BIN can also be extracted.

A bootable GD-ROM requires the primary executable (usually called 1ST_READ.BIN) to be placed at the
end of the final data track or it will not be loaded by the console. This requirement does not exist 
for MIL-CD's.

In addition to graphical UI's for Windows, macOS and Linux, a command line verison of the tool called
buildgdi is also available and capapble of building, patching or extracting.

ISO9660 code was forked from .NET DiscUtils, with a number of modifications made:
- When Joilet is disabled (which it is for this tool), don't output supplementary file tables
- Reversed the order that DiscUtils outputs the ISO sections. (Directory Tables come before files now)
- Fixed bug in non-joilet filename output. Filenames were not being appended with ;1 like they should be.
- Added Start LBA offset for entire image
- Added End LBA offset for entire image. Image will be padded to desired size.
- Added End of last file LBA, if set all files will be pushed back in the image to this location.
- Added properties to set most of the text identifiers (Application, Volume Set, Preparer, etc.)
- Omitted some stuff not being used by this application, such as other image formats and filesystems.
- Added cancellation token support for disc building, allowing the build process to be terminated early.
- Added a GDReader class which extends CDReader and allows the high density area of GDI's to be read.

The fork of .NET DiscUtils is available as a Nuget package for other developers to read and write .gdi
from other applications without needing to use the buildgdi command line tool. For example usage of
the library via the Nuget package, see the Program.cs in the buildgdi folder. This command line tool
performs all of the basic operations of the library.