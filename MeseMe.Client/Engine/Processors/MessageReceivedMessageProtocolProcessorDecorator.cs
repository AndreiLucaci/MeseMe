using System.Threading.Tasks;
using MeseMe.Client.Engine.Events;
using MeseMe.Client.Engine.Notifier;
using MeseMe.Contracts.Implementations.Models;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Models.Messages;

namespace MeseMe.Client.Engine.Processors
{
	public class MessageReceivedMessageProtocolProcessorDecorator 
		: NotifierMessageProtocolProcessorBase
	{
		private readonly IMessageProtocolProcessor _innerProcessor;

		public MessageReceivedMessageProtocolProcessorDecorator(IMessageProtocolProcessor innerProcessor,
			INotifierActions notifier)
			: base(notifier)
		{
			_innerProcessor = innerProcessor;
		}

		public override async Task ProcessAsync(IMessageProtocol messageProtocol)
		{
			if (messageProtocol != null && Equals(messageProtocol.Header, MessageType.Message))
			{
				var message = messageProtocol.GetDataAs<Message>();

				Notifier.OnMessageReceived(this, new NotifierEventArgs<Message>(message));

				return;
			}

			await _innerProcessor.ProcessAsync(messageProtocol);
		}
	}
}
