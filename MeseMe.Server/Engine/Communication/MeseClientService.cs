using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeseMe.Communicator;
using MeseMe.Contracts.Implementations.Models;
using MeseMe.Contracts.Interfaces.Communication;
using MeseMe.Contracts.Interfaces.DataStructure;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Models.Messages;
using MeseMe.Models.Users;

namespace MeseMe.Server.Engine.Communication
{
	public class MeseClientService : IClientService
	{
		private readonly IClientsPool _clientsPool;

		public MeseClientService(IClientsPool clientsPool)
		{
			_clientsPool = clientsPool;
		}

		public async Task<IClientHandler> CompleteHandshakeAsync(IClient meseClient)
		{
			var others = _clientsPool.ComputeOthersAsClients(meseClient.User).ToArray();

			await SendConnectionConfirmationAsync(meseClient, others);
			await BroadcastClientConnectedAsync(meseClient.User, others);

			var meseClientHandler = new MeseClientHandler(meseClient);
			return meseClientHandler;
		}

		private async Task SendConnectionConfirmationAsync(IClient meseClient, IEnumerable<IClient> others)
		{
			var connectionEstablished = new ConnectionEstablished
			{
				Me = meseClient.User,
				Others = others.Select(i => i.User)
			};

			var messageProtocol = new MessageProtocol
			{
				Header = MessageType.ClientConnectedSelf,
				Data = connectionEstablished
			};

			await MessageCommunicator.WriteAsync(meseClient.TcpClient, messageProtocol);
		}

		private async Task BroadcastClientConnectedAsync(User user, IEnumerable<IClient> others)
		{
			var messageProtocol = new MessageProtocol
			{
				Header = MessageType.ClientConnectedOthers,
				Data = user
			};

			var tcpClients = others.Select(i => i.TcpClient).ToArray();
			await MessageCommunicator.BroadcastAsync(tcpClients, messageProtocol);
		}
	}
}
