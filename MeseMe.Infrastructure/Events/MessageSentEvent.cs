using MeseMe.Models.Messages;
using Prism.Events;

namespace MeseMe.Infrastructure.Events
{
	public class MessageSentEvent : PubSubEvent<Message>
	{
	}
}
