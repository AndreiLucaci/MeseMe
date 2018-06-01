using System;
using System.Threading.Tasks;
using MeseMe.Communicator;
using MeseMe.Contracts.Implementations.Events;
using MeseMe.Contracts.Interfaces.Models;

namespace MeseMe.Server.Engine.Communication
{
	public class MeseClientHandler : IClientHandler
	{
		private readonly IClient _meseClient;

		public event EventHandler<MessageProtocolReceivedEventArgs> MessageProtocolReceived;

		public MeseClientHandler(IClient meseClient)
		{
			_meseClient = meseClient;
		}

		public async Task StartListeningAsync()
		{
			while (true)
			{
				var messageProtocol = await MessageCommunicator.ReadAsync(_meseClient.TcpClient);

				MessageProtocolReceived?.Invoke(this, new MessageProtocolReceivedEventArgs(messageProtocol));
			}
		}
	}
}
