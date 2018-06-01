using System.Threading.Tasks;
using MeseMe.Client.Engine.Events;
using MeseMe.Client.Engine.Notifier;
using MeseMe.Contracts.Implementations.Models;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Models.Messages;

namespace MeseMe.Client.Engine.Processors
{
	public class ConnectionEstablishedMessageProtocolProcessorDecorator
		: NotifierMessageProtocolProcessorBase
	{
		private readonly IMessageProtocolProcessor _innerProcessor;

		public ConnectionEstablishedMessageProtocolProcessorDecorator(
			IMessageProtocolProcessor innerProcessor, INotifierActions notifier)
			: base(notifier)
		{
			_innerProcessor = innerProcessor;
		}

		public override async Task ProcessAsync(IMessageProtocol messageProtocol)
		{
			if (messageProtocol != null && Equals(messageProtocol.Header, MessageType.ClientConnectedSelf))
			{
				var connectionEstablished = messageProtocol.GetDataAs<ConnectionEstablished>();

				Notifier.OnConnectedToServer(this, new NotifierEventArgs<ConnectionEstablished>(connectionEstablished));

				return;
			}

			await _innerProcessor.ProcessAsync(messageProtocol);
		}
	}
}
