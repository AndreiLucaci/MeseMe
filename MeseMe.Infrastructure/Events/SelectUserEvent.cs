using MeseMe.Models.Users;
using Prism.Events;

namespace MeseMe.Infrastructure.Events
{
	public class SelectUserEvent : PubSubEvent<User>
	{
	}
}
