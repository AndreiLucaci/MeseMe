using System.Collections.ObjectModel;
using System.Windows.Controls;
using MeseMe.Infrastructure.Events;
using MeseMe.Models.Users;
using Prism.Events;

namespace MeseMe.FriendsListModule.Views
{
	/// <summary>
	/// Interaction logic for FriendsListBox.xaml
	/// </summary>
	public partial class FriendsListBox : UserControl
	{
		private readonly IEventAggregator _eventAggregator;
		private ObservableCollection<User> _loadedFriends;

		private FriendsListBox()
		{
			InitializeComponent();

			FriendsList.MouseLeftButtonUp += (sender, args) =>
			{
				var item = (User) FriendsList.SelectedItem;

				if (item != null)
				{
					_eventAggregator.GetEvent<SelectUserEvent>().Publish(item);
				}
			};
		}

		public FriendsListBox(IEventAggregator eventAggregator)
			: this()
		{
			_eventAggregator = eventAggregator;

			_eventAggregator.GetEvent<ConnectedToServerEvent>().Subscribe(connectionEnabled =>
			{
				_loadedFriends = new ObservableCollection<User>(connectionEnabled.Others);
				FriendsList.ItemsSource = _loadedFriends;
			}, ThreadOption.UIThread, true);

			_eventAggregator.GetEvent<UserConnectedEvent>().Subscribe(payload =>
			{
				_loadedFriends.Clear();
				_loadedFriends.AddRange(payload.Friends);
			}, ThreadOption.UIThread, true);

			_eventAggregator.GetEvent<UserDisconnectedEvent>().Subscribe(payload =>
			{
				_loadedFriends.Clear();
				_loadedFriends.AddRange(payload.Friends);
			}, ThreadOption.UIThread, true);
		}
	}
}
