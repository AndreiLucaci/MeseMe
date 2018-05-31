using System;

namespace MeseMe.Models.Messages
{
	public class Message
	{
		public Guid FromId { get; set; }
		public Guid ToId { get; set; }
		public string Text { get; set; }
	}
}
