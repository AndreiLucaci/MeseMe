using System.Threading.Tasks;
using MeseMe.Client.Engine.Notifier;
using MeseMe.Contracts.Interfaces.Models;

namespace MeseMe.Client.Engine.Processors
{
	public class DoNothingMessageProtocolProcessor : NotifierMessageProtocolProcessorBase
	{
		public DoNothingMessageProtocolProcessor(INotifierActions notifier) 
			: base(notifier)
		{
		}

		public override Task ProcessAsync(IMessageProtocol messageProtocol)
		{
			return Task.CompletedTask;
		}
	}
}
