using MeseMe.Infrastructure.EventPayloads;
using Prism.Events;

namespace MeseMe.Infrastructure.Events
{
	public class ConnectToServerEvent : PubSubEvent<HandshakeConnectionPayload>
	{
	}
}
