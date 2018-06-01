using System;

namespace MeseMe.ServerLogger.Console
{
	public static class SConsole
	{
		public static void WriteWithColors(ConsoleColor backColor, ConsoleColor foreColor, string text)
		{
			var bConsoleColor = System.Console.BackgroundColor;
			var fConsoleColor = System.Console.ForegroundColor;

			System.Console.BackgroundColor = backColor;
			System.Console.ForegroundColor = foreColor;

			System.Console.WriteLine(text);

			System.Console.BackgroundColor = bConsoleColor;
			System.Console.ForegroundColor = fConsoleColor;
		}

		public static void WriteWithForeColor(ConsoleColor foreColor, string text)
		{
			var fConsoleColor = System.Console.ForegroundColor;

			System.Console.ForegroundColor = foreColor;

			System.Console.WriteLine(text);

			System.Console.ForegroundColor = fConsoleColor;
		}

		public static void WriteRed(string text)
		{
			WriteWithForeColor(ConsoleColor.Red, text);
		}

		public static void WriteGreen(string text)
		{
			WriteWithForeColor(ConsoleColor.Green, text);
		}

		public static void WriteInfoYellow(string text)
		{
			WriteWithColors(ConsoleColor.DarkYellow, ConsoleColor.Black, text);
		}

		public static void WriteNormal(string text)
		{
			System.Console.WriteLine(text);
		}
	}
}
