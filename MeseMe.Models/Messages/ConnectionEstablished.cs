using System;
using MeseMe.Models.Users;

namespace MeseMe.Models.Messages
{
	[Serializable]
	public class ConnectionEstablished
	{
		public User Me { get; set; }
		public User[] Others { get; set; }
	}
}
