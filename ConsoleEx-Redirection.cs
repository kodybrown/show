/*!
	Copyright (C) 2008-2015 Kody Brown (kody@bricksoft.com).
	
	MIT License:
	
	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to
	deal in the Software without restriction, including without limitation the
	rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
	sell copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:
	
	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.
	
	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
	FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
	DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Bricksoft.PowerCode
{
	// Original source:
	// http://stackoverflow.com/questions/3453220/how-to-detect-if-console-in-stdin-has-been-redirected/3453272#3453272
	public partial class ConsoleEx
	{
		private static List<DeleteFileWhenDone> _deleteFiles = new List<DeleteFileWhenDone>();

		public static bool IsOutputRedirected
		{
			get { return FileType.Char != GetFileType(GetStdHandle(StdHandle.Stdout)); }
		}

		public static bool IsInputRedirected
		{
			get { return FileType.Char != GetFileType(GetStdHandle(StdHandle.Stdin)); }
		}

		public static bool IsErrorRedirected
		{
			get { return FileType.Char != GetFileType(GetStdHandle(StdHandle.Stderr)); }
		}

		public static string RedirectInputToFile()
		{
			string f = Path.GetTempFileName();
			File.WriteAllText(f, Console.In.ReadToEnd());
			_deleteFiles.Add(new DeleteFileWhenDone(f));
			return f;
		}

		// P/Invoke:
		private enum FileType { Unknown, Disk, Char, Pipe };
		private enum StdHandle { Stdin = -10, Stdout = -11, Stderr = -12 };

		[DllImport("kernel32.dll")]
		private static extern FileType GetFileType( IntPtr hdl );

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetStdHandle( StdHandle std );

		private class DeleteFileWhenDone
		{
			public string _file { get; private set; }

			public DeleteFileWhenDone( string file )
			{
				_file = file;
			}

			~DeleteFileWhenDone()
			{
				if (File.Exists(_file)) {
					File.SetAttributes(_file, FileAttributes.Normal);
					File.Delete(_file);
				}
			}
		}
	}
}
