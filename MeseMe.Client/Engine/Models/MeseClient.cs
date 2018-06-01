using System.Net.Sockets;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Models.Users;

namespace MeseMe.Client.Engine.Models
{
	public class MeseClient : IClient
	{
		public MeseClient(TcpClient tcpClient, User user)
		{
			TcpClient = tcpClient;
			User = user;
		}

		public User User { get; }
		public TcpClient TcpClient { get; }
	}
}
