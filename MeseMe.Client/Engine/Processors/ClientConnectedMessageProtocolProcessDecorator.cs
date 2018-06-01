using System.Threading.Tasks;
using MeseMe.Client.Engine.Events;
using MeseMe.Client.Engine.Notifier;
using MeseMe.Contracts.Implementations.Models;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Models.Users;

namespace MeseMe.Client.Engine.Processors
{
	public class ClientConnectedMessageProtocolProcessDecorator : NotifierMessageProtocolProcessorBase
	{
		private readonly IMessageProtocolProcessor _innerProcessor;

		public ClientConnectedMessageProtocolProcessDecorator(IMessageProtocolProcessor innerProcessor,
			INotifierActions notifier)
			: base(notifier)
		{
			_innerProcessor = innerProcessor;
		}

		public override async Task ProcessAsync(IMessageProtocol messageProtocol)
		{
			if (messageProtocol.Header.Equals(MessageType.ClientConnectedOthers))
			{
				var user = messageProtocol.GetDataAs<User>();

				Notifier.OnUserConnected(this, new NotifierEventArgs<User>(user));

				return;
			}

			await _innerProcessor.ProcessAsync(messageProtocol);
		}
	}
}
