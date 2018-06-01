using System.Linq;
using System.Threading.Tasks;
using MeseMe.Communicator;
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
			if (messageProtocol != null && messageProtocol.Header == MessageType.ClientDisconnected)
			{
				var user = messageProtocol.GetDataAs<User>();

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
				await MessageCommunicator.BroadcastAsync(tcpClients, messageProtocol);
			}
		}
	}
}
