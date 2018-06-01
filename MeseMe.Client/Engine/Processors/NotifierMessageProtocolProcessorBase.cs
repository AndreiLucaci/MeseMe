using System.Threading.Tasks;
using MeseMe.Client.Engine.Notifier;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Contracts.Interfaces.Processors;

namespace MeseMe.Client.Engine.Processors
{
	public abstract class NotifierMessageProtocolProcessorBase 
		: IMessageProtocolProcessor
	{
		protected INotifierActions Notifier;

		protected NotifierMessageProtocolProcessorBase(INotifierActions notifier)
		{
			Notifier = notifier;
		}

		public abstract Task ProcessAsync(IMessageProtocol messageProtocol);
	}
}
