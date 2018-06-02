using System;
using Prism.Events;

namespace MeseMe.WPF.ViewModels.Subscribers
{
	public class ConnectToServerSubscriber
	{
		private readonly IEventAggregator _eventAggregator;

		public ConnectToServerSubscriber(IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
		}
	}
}
