using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using MeseMe.Infrastructure.Events;
using MeseMe.Models.Users;
using Prism.Events;
using Prism.Mvvm;

namespace MeseMe.FriendsListModule.ViewModels
{
	public class ListViewModel : BindableBase
	{
		private readonly IEventAggregator _eventAggregator;

		public ObservableCollection<User> Friends { get; set; }

		public ListViewModel()
		{
			//Friends.CurrentChanged += Friends_CurrentChanged;
		}

		private void Friends_CurrentChanged(object sender, System.EventArgs e)
		{
			//var user = (User)Friends.CurrentItem;

			//_eventAggregator.GetEvent<SelectUserEvent>().Publish(user);
		}

		public ListViewModel(IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;

			_eventAggregator.GetEvent<ConnectedToServerEvent>().Subscribe(connectionEnabled =>
			{
				Friends = new ObservableCollection<User>(connectionEnabled.Others);
			}, ThreadOption.UIThread, true);

			_eventAggregator.GetEvent<UserConnectedEvent>().Subscribe(payload =>
			{
				Friends.Clear();
				Friends.AddRange(payload.Friends);
			}, ThreadOption.UIThread, true);

			_eventAggregator.GetEvent<UserDisconnectedEvent>().Subscribe(payload =>
			{
				Friends.Clear();
				Friends.AddRange(payload.Friends);
			}, ThreadOption.UIThread, true);
		}

		private ICollectionView CreateCollectionView(IEnumerable<User> friends)
		{
			return new ListCollectionView(new ObservableCollection<User>(friends));
		}
	}
}
