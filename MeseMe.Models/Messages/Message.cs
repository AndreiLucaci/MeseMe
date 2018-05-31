using System;

namespace MeseMe.Models.Messages
{
	public class Message
	{
		#region not used atm
		public Guid FromId { get; set; }
		public Guid ToId { get; set; }
		#endregion

		public string FromName { get; set; }
		public string ToName { get; set; }
		public string Text { get; set; }
	}
}
