using System;
using MeseMe.ConsoleLogger.Console;

namespace MeseMe.ConsoleLogger
{
	public static class Logger
    {
	    public static void ServerStart()
	    {
		    SConsole.WriteInfoYellow("Starting the server.");
	    }

	    public static void ClientConnected()
	    {
		    SConsole.WriteInfoYellow("Client has connected");
	    }

	    public static void Info(string text, bool continuously = false)
	    {
		    SConsole.WriteInfoYellow(text, continuously);
	    }

	    public static void WriteLine(string text)
	    {
		    SConsole.WriteNormal(text);
	    }

	    public static void Error(string text)
	    {
		    SConsole.WriteWithColors(ConsoleColor.Red, ConsoleColor.White, text);
	    }

	    public static void WriteLine(string text, ConsoleColor color)
	    {
		    SConsole.WriteWithForeColor(color, text);
	    }

	    public static void Write(string text)
	    {
		    SConsole.WriteNormalContinuously(text);
	    }

	    public static void Write(string text, ConsoleColor color)
	    {
		    SConsole.WriteWithForeColorContinuously(color, text);
	    }
    }
}
