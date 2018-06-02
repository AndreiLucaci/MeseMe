using System;
using MeseMe.Client.Engine.Events;
using MeseMe.Contracts.Interfaces.Settings;
using MeseMe.Infrastructure.EventPayloads;
using MeseMe.Infrastructure.Events;
using MeseMe.Models.Messages;
using MeseMe.Models.Users;
using Prism.Events;

namespace MeseMe.WPF.Wrapper
{
	public class ClientWrapper
	{
		private readonly ISettings _settings;
		private readonly IEventAggregator _eventAggregator;
		private readonly Client.Client _client;

		private User _currentToUser;

		public ClientWrapper(IEventAggregator eventAggregator, Client.Client client, ISettings settings)
		{
			_settings = settings;
			_eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
			_client = client ?? throw new ArgumentNullException(nameof(client));

			InitializeEvents();
		}

		private void InitializeEvents()
		{
			_eventAggregator.GetEvent<ConnectToServerEvent>().Subscribe(async payload =>
			{
				_settings.Uri = payload.Host;
				await _client.ConnectAsync(payload.Name, _settings);
			});
			_eventAggregator.GetEvent<SendMessageTextEvent>().Subscribe(async text =>
			{
				if (_currentToUser != null)
				{
					_eventAggregator.GetEvent<MessageSentEvent>().Publish(new Message
					{
						From = _client.Me.User,
						To = _currentToUser,
						Text = text
					});

					await _client.SendMessageAsync(text, _currentToUser);
				}
			});
			_eventAggregator.GetEvent<SelectUserEvent>().Subscribe(u => _currentToUser = u);


			_client.ConnectedToServer += ClientOnConnectedToServer;
			_client.MessageReceived += ClientOnMessageReceived;
			_client.UserConnected += ClientOnUserConnected;
			_client.UserDisconnected += ClientOnUserDisconnected;
		}

		private void ClientOnUserDisconnected(object sender, NotifierEventArgs<User> notifierEventArgs)
		{
			_eventAggregator.GetEvent<UserDisconnectedEvent>().Publish(
				new UserConnectionPayload(notifierEventArgs.Payload, _client.Friends)
			);
		}

		private void ClientOnUserConnected(object sender, NotifierEventArgs<User> notifierEventArgs)
		{
			_eventAggregator.GetEvent<UserConnectedEvent>().Publish(
				new UserConnectionPayload(notifierEventArgs.Payload, _client.Friends)
			);
		}

		private void ClientOnMessageReceived(object sender, NotifierEventArgs<Message> notifierEventArgs)
		{
			if (Equals(_client.Me.User, notifierEventArgs.Payload.To))
			{
				_eventAggregator.GetEvent<MessageReceivedEvent>().Publish(notifierEventArgs.Payload);
			}
		}

		private void ClientOnConnectedToServer(object sender, NotifierEventArgs<ConnectionEstablished> notifierEventArgs)
		{
			_eventAggregator.GetEvent<ConnectedToServerEvent>().Publish(notifierEventArgs.Payload);
		}
	}
}
