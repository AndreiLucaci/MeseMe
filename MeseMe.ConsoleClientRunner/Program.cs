﻿using System;
using System.Threading.Tasks;
using MeseMe.ConsoleClientRunner.UnityConfiguration;
using MeseMe.ConsoleLogger;
using MeseMe.Contracts.Interfaces.Settings;
using Unity;

namespace MeseMe.ConsoleClientRunner
{
	class Program
	{
		static void Main(string[] args)
		{
			var unityContainer = new UnityContainer();

			unityContainer
				.WithClient()
				.WithSettings();

			var client = unityContainer.Resolve<Client.Client>();
			var settings = unityContainer.Resolve<ISettings>();

			RegisterEvents(client);

			StartAsync(client, settings).Wait();
		}

		private static async Task StartAsync(Client.Client client, ISettings settings)
		{
			await Connect(client, settings);

			Console.ReadKey();

			await client.ShutDownAsync();
		}

		static async Task Connect(Client.Client client, ISettings settings)
		{
			Logger.WriteLine("Name: ", ConsoleColor.Red);
			var name = Console.ReadLine();

			await client.ConnectAsync(name, settings);
		}

		static void RegisterEvents(Client.Client client)
		{
			client.MessageReceived += (sender, args) =>
			{
				var message = args.Payload;

				Logger.Write($"Received message from", ConsoleColor.Blue);
				Logger.Info(message.From.ToString(), true);
				Logger.WriteLine(message.Text);
			};

			client.ConnectedToServer += (sender, args) =>
			{
				var connectionEstablished = args.Payload;
				Logger.Info("Succesfully connected to the server");
				Logger.WriteLine($"My current information: {connectionEstablished.Me}");
				Logger.WriteLine($"Online users: {connectionEstablished.Others.Length}");
			};

			client.UserDisconnected += (sender, args) =>
			{
				var user = args.Payload;
				Logger.Write("Friend disconnected: ");
				Logger.WriteLine(user.ToString(), ConsoleColor.Cyan);
			};

			client.UserConnected += (sender, args) =>
			{
				var user = args.Payload;
				Logger.Write("Friend connected: ");
				Logger.WriteLine(user.ToString(), ConsoleColor.Green);
			};
		}
	}
}
