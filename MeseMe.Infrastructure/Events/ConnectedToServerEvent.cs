using MeseMe.Models.Messages;
using Prism.Events;

namespace MeseMe.Infrastructure.Events
{
	public class ConnectedToServerEvent : PubSubEvent<ConnectionEstablished>
	{
	}
}
