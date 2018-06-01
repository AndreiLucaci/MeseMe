using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MeseMe.ConsoleClientRunner.UnityConfiguration;
using MeseMe.ConsoleLogger;
using Unity;

namespace MeseMe.ConsoleClientRunner
{
	class Program
	{
		private static bool _running;

		static void Main(string[] args)
		{
			var unityContainer = new UnityContainer();

			ClientContainerConfiguration.ConfigureClient(unityContainer);

			var client = unityContainer.Resolve<Client.Client>();

			RegisterEvents(client);

			StartAsync(client).Wait();
		}

		private static async Task StartAsync(Client.Client client)
		{
			await Connect(client);

			Console.ReadKey();

			await client.ShutDownAsync();

			//while (true)
			//{

			//}
		}

		static async Task Connect(Client.Client client)
		{
			Logger.WriteLine("Name: ", ConsoleColor.Red);
			var name = Console.ReadLine();

			await client.ConnectAsync(name);
		}

		static void RegisterEvents(Client.Client client)
		{
			client.MessageReceived += (sender, args) =>
			{
				var message = args.Model;

				Logger.Write($"Received message from", ConsoleColor.Blue);
				Logger.Info(message.From.ToString(), true);
				Logger.WriteLine(message.Text);
			};

			client.ConnectedToServer += (sender, args) =>
			{
				var connectionEstablished = args.Model;
				Logger.Info("Succesfully connected to the server");
				Logger.WriteLine($"My current information: {connectionEstablished.Me}");
				Logger.WriteLine($"Online users: {connectionEstablished.Others.Length}");
			};
		}
	}
}
