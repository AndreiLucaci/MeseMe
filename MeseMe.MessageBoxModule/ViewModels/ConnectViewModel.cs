using System;
using MeseMe.Infrastructure.Events;
using Prism.Commands;
using Prism.Events;

namespace MeseMe.MessageBoxModule.ViewModels
{
	public class ConnectViewModel
	{
		private readonly IEventAggregator _eventAggregator;

		public DelegateCommand Connect { get; set; }

		public ConnectViewModel(IEventAggregator eventAggregator, string name)
		{
			_eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

			Connect = new DelegateCommand(() => Execute(name));
		}

		private void Execute(string name)
		{
			_eventAggregator.GetEvent<ConnectToServerEvent>().Publish(name);
		}
	}
}
