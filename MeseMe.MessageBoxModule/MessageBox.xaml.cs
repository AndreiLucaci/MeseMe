using System.Windows.Controls;
using System.Windows.Input;
using MeseMe.Infrastructure.Events;
using Prism.Events;

namespace MeseMe.MessageBoxModule
{
	/// <summary>
	/// Interaction logic for MessageBox.xaml
	/// </summary>
	public partial class MessageBox : UserControl
	{
		private readonly IEventAggregator _eventAggregator;
		public string UserName { get; set; }

		private MessageBox()
		{
			InitializeComponent();

			MessageTextBox.KeyUp += (sender, args) =>
			{
				if (args.Key == Key.Enter)
				{
					var text = MessageTextBox.Text;
					if (!string.IsNullOrEmpty(text))
					{
						MessageTextBox.Clear();

						_eventAggregator.GetEvent<SendMessageTextEvent>().Publish(text);
					}
				}
			};

			NameTextBox.KeyUp += (sender, args) =>
			{
				if (args.Key == Key.Enter && !_connecting)
				{
					_connecting = true;
					AttemptConnection();
				}
			};

			NameTextBox.Focus();
		}

		private bool _connecting;

		public MessageBox(IEventAggregator eventAggregator)
			: this()
		{
			_eventAggregator = eventAggregator;

			_eventAggregator.GetEvent<ConnectedToServerEvent>().Subscribe(connectionEstablished =>
			{
				NameTextBox.IsEnabled = false;
				_connecting = false;
			}, ThreadOption.UIThread, true);

			_eventAggregator.GetEvent<SelectUserEvent>().Subscribe(user =>
			{
				MessageTextBox.IsEnabled = true;
				MessageTextBox.Focus();
			}, ThreadOption.UIThread, true);
		}

		private void AttemptConnection()
		{
			if (string.IsNullOrEmpty(NameTextBox.Text)) return;
			UserName = NameTextBox.Text;
			_eventAggregator.GetEvent<ConnectToServerEvent>().Publish(UserName);
		}
	}
}
