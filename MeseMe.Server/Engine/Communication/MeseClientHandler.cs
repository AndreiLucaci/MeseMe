using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using MeseMe.Communicator;
using MeseMe.Contracts.Implementations.Events;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Server.Engine.Exceptions;

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
				try
				{
					var messageProtocol = await MessageCommunicator.ReadAsync(_meseClient.TcpClient);

					MessageProtocolReceived?.Invoke(this, new MessageProtocolReceivedEventArgs(messageProtocol));
				}
				catch (IOException exception)
				{
					ThrowDisconnectException(exception);
				}
				catch (SocketException exception)
				{
					ThrowDisconnectException(exception);
				}
			}
		}

		private void ThrowDisconnectException(Exception exception)
		{
			throw new ClientDisconnectedException(_meseClient, exception);
		}
	}
}
