/*
 * Created by SharpDevelop.
 * User: cdahmedeh
 * Date: 2017-10-19
 * Time: 6:34 PM
 * 
 */
using System;
using System.Runtime.InteropServices;
using System.Threading;
using CommandLine;

namespace MuraleWinCommand
{
	class Options
	{
		[Option('f', "file", Required = true, HelpText = "Full absolute path of wallpaper image file")]
		public string File { get; set; }
		
		[ParserState]
		public IParserState LastParserState { get; set; }
	}
	
	class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Options options = new Options();
			bool valid = CommandLine.Parser.Default.ParseArgumentsStrict(args, options);
			string file = options.File;
			
			if (valid) {
				SetWallpaper(file);
			} else {
				Console.WriteLine("Invalid Command Line Arguments Provided");
			}
		}
		
		private static void SetWallpaper(String file)
		{
			WALLPAPEROPT wallpaperOpt = new WALLPAPEROPT();
			wallpaperOpt.dwStyle = WallPaperStyle.WPSTYLE_CROPTOFIT;
			wallpaperOpt.SizeOf = Marshal.SizeOf(typeof(WALLPAPEROPT));
			
			IntPtr programWindow = User32.FindWindow("Progman", null);
			User32.SendMessage(programWindow, 0x52c, IntPtr.Zero, IntPtr.Zero);
			
			IActiveDesktop activeDesktop = ActiveDesktop.Create();
			activeDesktop.SetWallpaper(file, 0);
			activeDesktop.SetWallpaperOptions(ref wallpaperOpt, 0);
			activeDesktop.ApplyChanges(AD_Apply.ALL | AD_Apply.FORCE | AD_Apply.BUFFERED_REFRESH);
		}
	}
}