using System;
using System.Threading.Tasks;
using MeseMe.Communicator;
using MeseMe.ConsoleLogger;
using MeseMe.Contracts.Implementations.Models;
using MeseMe.Contracts.Interfaces.DataStructure;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Models.Messages;

namespace MeseMe.Server.Engine.Processors
{
	public class MeseMessageProtocolProcessorDecorator : IMessageProtocolProcessor
	{
		private readonly IMessageProtocolProcessor _innerProcessor;
		private readonly IClientsPool _clientsPool;

		public MeseMessageProtocolProcessorDecorator(IMessageProtocolProcessor innerProcessor,
			IClientsPool clientsPool)
		{
			_innerProcessor = innerProcessor;
			_clientsPool = clientsPool;
		}

		public async Task ProcessAsync(IMessageProtocol messageProtocol)
		{
			if (Equals(messageProtocol.Header, MessageType.Message))
			{
				var message = messageProtocol.GetDataAs<Message>();

				try
				{
					var meseClient = _clientsPool.FindClient(message.To.Id);

					if (meseClient != null)
					{
						await MessageCommunicator.WriteAsync(meseClient.TcpClient, messageProtocol);
					}
					else
					{
						Logger.Error($"Client {message.To.Name} not found. ");
					}
				}
				catch (Exception ex)
				{
					Logger.Error($"Exception: {ex}.");
				}
				return;
			}
			await _innerProcessor.ProcessAsync(messageProtocol);
		}
	}
}
