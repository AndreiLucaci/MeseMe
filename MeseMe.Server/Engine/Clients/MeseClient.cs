using System;
using System.Net.Sockets;

namespace MeseMe.Server.Engine.Clients
{
	public class MeseClient
	{
		public Guid Id { get; }

		public TcpClient TcpClient { get; }

		public MeseClient(TcpClient client)
		{
			Id = Guid.NewGuid();
			TcpClient = client;
		}
	}
}
