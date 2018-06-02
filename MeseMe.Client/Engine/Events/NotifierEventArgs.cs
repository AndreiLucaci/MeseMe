using System;

namespace MeseMe.Client.Engine.Events
{
	public class NotifierEventArgs<T> : EventArgs
	{ 
		public NotifierEventArgs(T payload)
		{
			Payload = payload;
		}

		public T Payload { get; set; }
	}
}
