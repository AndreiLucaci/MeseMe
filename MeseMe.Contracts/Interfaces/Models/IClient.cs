using System.Net.Sockets;
using MeseMe.Models.Users;

namespace MeseMe.Contracts.Interfaces.Models
{
	public interface IClient
	{
		User User { get; }
		TcpClient TcpClient { get; }
	}
}