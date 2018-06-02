using System.Collections.Generic;
using MeseMe.Models.Users;

namespace MeseMe.Infrastructure.EventPayloads
{
	public class UserConnectionPayload
	{
		public User User { get; set; }
		public IEnumerable<User> Friends { get; set; }

		public UserConnectionPayload(User user, IEnumerable<User> friends)
		{
			User = user;
			Friends = friends;
		}
	}
}
