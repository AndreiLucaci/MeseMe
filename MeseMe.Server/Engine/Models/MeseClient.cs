using System;
using System.Net.Sockets;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Models.Users;

namespace MeseMe.Server.Engine.Models
{
	public class MeseClient : IClient
	{
		public User User { get; }

		public TcpClient TcpClient { get; }

		public MeseClient(TcpClient client, string name)
		{
			User = new User
			{
				Id = Guid.NewGuid(),
				Name = name
			};
			TcpClient = client;
		}

		public override string ToString()
		{
			return User.ToString();
		}
	}
}
