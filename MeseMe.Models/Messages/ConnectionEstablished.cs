using System;
using System.Collections.Generic;

namespace MeseMe.Models.Messages
{
	public class ConnectionEstablished
	{
		public Guid Me { get; set; }
		public IEnumerable<Guid> Others { get; set; }
	}
}
