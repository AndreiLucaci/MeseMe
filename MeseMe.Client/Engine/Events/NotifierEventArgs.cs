using System;

namespace MeseMe.Client.Engine.Events
{
	public class NotifierEventArgs<T> : EventArgs
	{ 
		public NotifierEventArgs(T model)
		{
			Model = model;
		}

		public T Model { get; set; }
	}
}
