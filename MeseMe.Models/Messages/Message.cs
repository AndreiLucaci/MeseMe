using MeseMe.Models.Users;

namespace MeseMe.Models.Messages
{
	public class Message
	{
		public User From { get; set; }
		public User To { get; set; }
		public string Text { get; set; }
	}
}
