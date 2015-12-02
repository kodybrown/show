# show
A neat utility to allow piping standard output to notepad as in `dir | show`..

> If Notepad2.exe is available in the PATH, that will be used instead.

Editing the opened file
-----------------------

The file that is opened is a saved temporary file. You can edit and save it (or 'save as' to a new location).

But, bare in mind that if you edited a file like the following, the original file will _not_ be updated when you save.

    >type file.txt | show  <-- DON'T DO THIS

To open and edit a file from the command-line use this instead:

    >notepad file.txt

Examples
--------

Show the resuls of the `dir` command in Notepad.

    >dir | show
    >dir /S /B | show

Show the git log in Notepad.

    >git log | show

Show the output of (all?) DOS commands in Notepad.

    >set "_dpath=C:\logs\%date:~10,4%-%date:~4,2%-%date:~7,2%" ^
     mkdir "%_dpath%" ^
     copy /Z /Y /V "C:\temp\*.log" "%_dpath%\*" | show
