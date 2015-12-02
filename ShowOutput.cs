using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Bricksoft.PowerCode;

namespace ViewStandardIn
{
	class ShowOutput
	{
		static int Main( string[] args )
		{
			bool optWait = false;
			string tmpfile = "";
			string input = "";
			Process p;
			ProcessStartInfo startInfo;

			for (int i = 0; i < args.Length; i++) {
				string a = args[i].Trim();

				if (a.Equals("version", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("--version", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("-version", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("/version", StringComparison.CurrentCultureIgnoreCase)) {
					DisplayVersion(true);
					return 0;
				} else if (a.Equals("--v", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("-v", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("/v", StringComparison.CurrentCultureIgnoreCase)) {
					DisplayVersion(false);
					return 0;

				} else if (a.Equals("wait", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("--wait", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("-wait", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("/wait", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("-w", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("/w", StringComparison.CurrentCultureIgnoreCase)) {
					optWait = true;

				} else if (a.Equals("file", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("--file", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("-file", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("/file", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("-f", StringComparison.CurrentCultureIgnoreCase)
						|| a.Equals("/f", StringComparison.CurrentCultureIgnoreCase)) {
					if (i == args.Length - 1) {
						DisplayVersion(true);
						Console.Out.WriteLine();
						Console.Error.WriteLine("    **** ERROR: --file is missing file name", a);
						return 4;
					}
					tmpfile = args[i + 1];
					input = File.ReadAllText(tmpfile);

				} else {
					DisplayVersion(true);
					Console.Out.WriteLine();
					Console.Error.WriteLine("    **** ERROR: unknown command: {0}", a);
					return 3;
				}
			}

			if (!ConsoleEx.IsInputRedirected && (args.Length == 0 || tmpfile.Length == 0)) {
				DisplayVersion(true);
				Console.Out.WriteLine();
				Console.Error.WriteLine("    **** ERROR: missing content to display");
				return 1;
			}

			// Read stdin
			if (ConsoleEx.IsInputRedirected && tmpfile.Length == 0) {
				input = Console.In.ReadToEnd();
				tmpfile = Path.GetTempFileName() + ".txt";
				File.WriteAllText(tmpfile, input);
			}

			// Show the file in the default .txt editor.
			startInfo = new ProcessStartInfo();
			startInfo.Arguments = "";
			startInfo.FileName = tmpfile;
			startInfo.Verb = "Open";
			startInfo.WorkingDirectory = Environment.CurrentDirectory;

			p = Process.Start(startInfo);

			if (optWait) {
				p.WaitForExit(30000);
			}

			return 0;
		}

		private static void DisplayVersion( bool full )
		{
			FileVersionInfo fi = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
			if (full) {
				Console.Out.WriteLine("{4} v{0}.{1}.{2}.{3}", fi.FileMajorPart, fi.FileMinorPart, fi.ProductBuildPart, fi.ProductPrivatePart, fi.OriginalFilename);
				Console.Out.WriteLine(fi.LegalCopyright);
				Console.Out.WriteLine("Saves its stdin to a temporary file and opens it in the default text editor.");
			} else {
				Console.Out.WriteLine("{0}.{1}.{2}.{3}", fi.FileMajorPart, fi.FileMinorPart, fi.ProductBuildPart, fi.ProductPrivatePart);
			}
		}
	}
}
