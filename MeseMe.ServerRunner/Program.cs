﻿using System;
using MeseMe.ServerRunner.UnitiConfiguration;
using Unity;

namespace MeseMe.ServerRunner
{
	public class Program
	{
		static void Main(string[] args)
		{
			var unityContainer = new UnityContainer();
			unityContainer
				.WithSettings()
				.WithServer();

			var server = unityContainer.Resolve<Server.Server>();

			server.Start();

			Console.ReadKey();
		}
	}
}
