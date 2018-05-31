using System;
using MeseMe.ServerLogger.Console;

namespace MeseMe.ServerLogger
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

	    public static void Info(string text)
	    {
		    SConsole.WriteInfoYellow(text);
	    }

	    public static void Write(string text)
	    {
		    SConsole.WriteNormal(text);
	    }

	    public static void Error(string text)
	    {
		    SConsole.WriteWithColors(ConsoleColor.Red, ConsoleColor.White, text);
	    }
    }
}
