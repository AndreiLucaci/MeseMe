using System.Linq;
using System.Threading.Tasks;
using MeseMe.Communicator;
using MeseMe.ConsoleLogger;
using MeseMe.Contracts.Implementations.Models;
using MeseMe.Contracts.Interfaces.DataStructure;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Models.Users;

namespace MeseMe.Server.Engine.Processors
{
	public class MeseDisconnectMessageProtocolDecorator : IMessageProtocolProcessor
	{
		private readonly IMessageProtocolProcessor _innerProcessor;
		private readonly IClientsPool _clientsPool;

		public MeseDisconnectMessageProtocolDecorator(IMessageProtocolProcessor innerProcessor, 
			IClientsPool clientsPool)
		{
			_innerProcessor = innerProcessor;
			_clientsPool = clientsPool;
		}

		public async Task ProcessAsync(IMessageProtocol messageProtocol)
		{
			if (messageProtocol != null && Equals(messageProtocol.Header, MessageType.ClientDisconnected))
			{
				var user = messageProtocol.GetDataAs<User>();

				Logger.Info($"Client disconnected. {user}");

				await BroadcastClientDisconnectedAsync(user);

				return;
			}
			await _innerProcessor.ProcessAsync(messageProtocol);
		}

		private async Task BroadcastClientDisconnectedAsync(User user)
		{
			if (user != null && _clientsPool.Remove(user.Id))
			{
				var messageProtocol = new MessageProtocol
				{
					Header = MessageType.ClientDisconnected,
					Data = user
				};

				var tcpClients = _clientsPool.ConnectedClients.Select(i => i.Value.TcpClient).ToArray();
				if (tcpClients.Any())
				{
					await MessageCommunicator.BroadcastAsync(tcpClients, messageProtocol);
				}
			}
		}
	}
}
