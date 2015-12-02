# show
A neat utility to allow piping standard output to notepad as in `dir | show`..

> If Notepad2.exe is available in the PATH, that will be used instead.

Command-line arguments
----------------------

    >show
    Saves its stdin to a temporary file and opens it in the default text editor.
    
    show.exe v0.1.0.51120
    Copyright (C) 2014-2015 Kody Brown (@kodybrown).
    Released under the MIT License.
    
    USAGE:
    
      --version, -v  Displays the current version.
    
      --wait, -w     Waits for Notepad to be closed before returning to the
                     console process.
      
      --file, -f     Opens the specified file instead of stdin. This flag is
                     really only here to enable various different ways of
                     showing output during automation, etc.

Editing the opened file
-----------------------

The file that is opened is a saved temporary file. You can edit and save it (or 'save as' to a new location).

But, bare in mind that if you edited a file like the following, the original file will _not_ be updated when you save.

    >type file.txt | show  <-- DON'T DO THIS

To open and edit a file from the command-line use this instead:

    >notepad file.txt

If you _really_ want to use show for that, use the `-f` flag.

    >show -f file.txt

Examples
--------

Show the resuls of the `dir` command in Notepad.

    >dir | show
    >dir /S /B *.txt | show

Show the git log in Notepad.

    >git log | show

Show the output of (all?) DOS commands in Notepad.

    >set "_dpath=C:\logs\%date:~10,4%-%date:~4,2%-%date:~7,2%" & mkdir "%_dpath%" & copy /Z /Y /V "C:\temp\*.log" "%_dpath%\*" | show

Show the current directory in Notepad.

    >cd | show

A note about StdIn vs. StdErr
-----------------------------

The `show` utility will only show its stdin in Notepad. For instance, the following statement will not include the error message in Notepad. For instance, running the following:

    >dir C:\DoesNotExist | show
    File Not Found

Displays `File Not Found` in the console window and the following in Notepad.

     Volume in drive C is mymachine
     Volume Serial Number is 1234-ABCD
    
     Directory of C:\

So, if there is a potential error, that you would want to see, use the following:

    >dir C:\DoesNotExist **2>&1** | show

Which will show the following, complete response in Notepad.

     Volume in drive C is mymachine
     Volume Serial Number is 1234-ABCD
    
     Directory of C:\
    
    File Not Found
