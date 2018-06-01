using System.Collections.Generic;
using MeseMe.Models.Users;

namespace MeseMe.Models.Messages
{
	public class ConnectionEstablished
	{
		public User Me { get; set; }
		public IEnumerable<User> Others { get; set; }
	}
}
