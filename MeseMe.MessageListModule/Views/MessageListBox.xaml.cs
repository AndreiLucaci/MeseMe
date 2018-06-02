using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using MeseMe.Infrastructure.Events;
using MeseMe.MessageListModule.ViewModels;
using MeseMe.Models.Messages;
using MeseMe.Models.Users;
using Prism.Events;

namespace MeseMe.MessageListModule.Views
{
	/// <summary>
	/// Interaction logic for MessageListBox.xaml
	/// </summary>
	public partial class MessageListBox : UserControl
	{
		private readonly Dictionary<SpecialPairViewModel, ObservableCollection<MessageViewModel>> _messages =
			new Dictionary<SpecialPairViewModel, ObservableCollection<MessageViewModel>>();

		private readonly IEventAggregator _eventAggregator;
		private User _me;

		private MessageListBox()
		{
			InitializeComponent();
		}

		public MessageListBox(IEventAggregator eventAggregator)
			: this()
		{
			_eventAggregator = eventAggregator;

			_eventAggregator.GetEvent<MessageReceivedEvent>().Subscribe(p =>
				{
					SetMessage(p, Brushes.DeepSkyBlue);
				}, ThreadOption.UIThread, true);

			_eventAggregator.GetEvent<MessageSentEvent>().Subscribe(m =>
			{
				SetMessage(m, Brushes.DeepPink);
			}, ThreadOption.UIThread, true);

			_eventAggregator.GetEvent<SelectUserEvent>().Subscribe(u =>
			{
				var specialPair = new SpecialPairViewModel(u, _me);
				if (!_messages.ContainsKey(specialPair))
				{
					_messages[specialPair] = new ObservableCollection<MessageViewModel>();
				}

				var collection = _messages[specialPair];
				MessagesList.ItemsSource = collection;
				MessagesList.SelectedIndex = collection.Count - 1;
			}, ThreadOption.UIThread, true);

			_eventAggregator.GetEvent<ConnectedToServerEvent>().Subscribe(c =>
			{
				if (c?.Me != null)
				{
					_me = c.Me;
				}
			}, ThreadOption.UIThread, true);
		}

		private void SetMessage(Message p, Brush color)
		{
			var pair = new SpecialPairViewModel(p.From, p.To);
			if (!_messages.ContainsKey(pair))
			{
				_messages[pair] = new ObservableCollection<MessageViewModel>();
			}

			var collection = _messages[pair];

			var messageViewModel = new MessageViewModel
			{
				Name = p.From.Name,
				Text = p.Text,
				Color = color
			};

			collection.Add(messageViewModel);

			MessagesList.SelectedIndex = collection.Count - 1;
		}
	}
}
