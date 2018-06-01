using System;

namespace MeseMe.ConsoleLogger.Console
{
	public static class SConsole
	{
		public static void WriteWithColors(ConsoleColor backColor, ConsoleColor foreColor, string text)
		{
			WriteWithColors(() => System.Console.WriteLine(text), backColor, foreColor);
		}

		public static void WriteWithForeColor(ConsoleColor foreColor, string text)
		{
			WriteWithForeColor(() => System.Console.WriteLine(text), foreColor);
		}

		public static void WriteWithForeColorContinuously(ConsoleColor foreColor, string text)
		{
			WriteWithForeColor(() => System.Console.Write(text), foreColor);
		}

		public static void WriteWithColorsContinuously(ConsoleColor backColor, ConsoleColor foreColor, string text)
		{
			WriteWithColors(() => System.Console.Write(text), backColor, foreColor);
		}

		public static void WriteRed(string text)
		{
			WriteWithForeColor(ConsoleColor.Red, text);
		}

		public static void WriteGreen(string text)
		{
			WriteWithForeColor(ConsoleColor.Green, text);
		}

		public static void WriteInfoYellow(string text, bool continuously = false)
		{
			if (continuously)
			{
				WriteWithColorsContinuously(ConsoleColor.DarkYellow, ConsoleColor.Black, text);
			}
			else
			{
				WriteWithColors(ConsoleColor.DarkYellow, ConsoleColor.Black, text);
			}
		}

		public static void WriteNormal(string text)
		{
			System.Console.WriteLine(text);
		}

		public static void WriteNormalContinuously(string text)
		{
			System.Console.Write(text);
		}

		private static void WriteWithColors(Action action, ConsoleColor backColor, ConsoleColor foreColor)
		{
			var bConsoleColor = System.Console.BackgroundColor;
			var fConsoleColor = System.Console.ForegroundColor;

			System.Console.BackgroundColor = backColor;
			System.Console.ForegroundColor = foreColor;

			action();

			System.Console.BackgroundColor = bConsoleColor;
			System.Console.ForegroundColor = fConsoleColor;
		}

		private static void WriteWithForeColor(Action action, ConsoleColor foreColor)
		{
			var fConsoleColor = System.Console.ForegroundColor;

			System.Console.ForegroundColor = foreColor;

			action();

			System.Console.ForegroundColor = fConsoleColor;
		}
	}
}
