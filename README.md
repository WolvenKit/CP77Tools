# CP77Tools
Modding tools for the cyberpunk 2077 game.

# CP77 Tools Preview Version 0.1.2

- requires NET5.0.

**The cp77 tools require the oo2ext_7_win64.dll to work.
Copy and paste the dll into the cp77Tools folder.**

It can be found here:
`Cyberpunk 2077\bin\x64\oo2ext_7_win64.dll`

CP77 Tools discord: https://discord.gg/Epkq79kd96
CDPR modding community discord: https://discord.gg/USZwxxFrKa 


## Usage: 
* displays the general help
`--help`

* displays help for a specific command
`archive -h`

### Main functions
* extract all files from archive
`archive -e -p "<PATH TO ARCHIVE>.archive"`

* extract all textures from archive
`archive -u --uext png "<PATH TO ARCHIVE>.archive"`


### Debug Options
* dump archive info from archive
`dump -a -p "<PATH TO ARCHIVE>.archive"`
* dumps import info from extracted cr2w file
`cr2w -i -p "<PATH TO FILE>"` 

## v.0.1.3
- much better command line help and syntax

## v.0.1.2
- faster extraction
- supports dumping of .archive info
- supports dumping of .cr2w file info

## v.0.1.1
- supports dumping .archive info
- supports extracting and decompression of archive files

## v0.1
- extract binary files from an .archive bundle


