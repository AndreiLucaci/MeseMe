using MeseMe.Infrastructure.EventPayloads;
using Prism.Events;

namespace MeseMe.Infrastructure.Events
{
	public class UserConnectedEvent : PubSubEvent<UserConnectionPayload>
	{
	}
}
