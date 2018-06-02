using System.Windows.Controls;
using System.Windows.Input;
using MeseMe.Communicator.Constants;
using MeseMe.Contracts.Interfaces.Settings;
using MeseMe.Infrastructure.EventPayloads;
using MeseMe.Infrastructure.Events;
using MeseMe.MessageBoxModule.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;

namespace MeseMe.MessageBoxModule
{
	/// <summary>
	/// Interaction logic for MessageBox.xaml
	/// </summary>
	public partial class MessageBox : UserControl
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly MessageBoxViewModel _viewModel;
		public string UserName { get; set; }

		public static string DefaultIp;

		public MessageBox()
		{
			InitializeComponent();

			if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
			{
				var svc = ServiceLocator.Current;
				_eventAggregator = svc.GetInstance<IEventAggregator>();
				_viewModel = new MessageBoxViewModel
				{
					Host = svc.GetInstance<ISettings>().Uri
				};

				DataContext = _viewModel;
			}

			InitializeEvents();

			NameTextBox.Focus();
		}

		private void InitializeEvents()
		{
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

		private bool _connecting;

		private void AttemptConnection()
		{
			if (string.IsNullOrEmpty(NameTextBox.Text) ||
				string.IsNullOrEmpty(HostTextBox.Text))
			{
				return;
			}


			UserName = _viewModel.Name;

			var connectionPayload = new HandshakeConnectionPayload
			{
				Host = HostTextBox.Text,
				Name = UserName
			};

			_eventAggregator.GetEvent<ConnectToServerEvent>().Publish(connectionPayload);
		}
	}
}
