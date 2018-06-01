using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MeseMe.Client.Engine.Events;
using MeseMe.Client.Engine.Models;
using MeseMe.Client.Engine.Notifier;
using MeseMe.Communicator;
using MeseMe.Contracts.Implementations.Models;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Models.Messages;
using MeseMe.Models.Users;

using static MeseMe.Communicator.Constants.Handshake;

namespace MeseMe.Client
{
	public class Client : INotifierEvents
	{
		private readonly ITwoWayNotifier _notifier;
		private readonly IMessageProtocolProcessor _protocolProcessor;
		private readonly TcpClient _tcpClient;
		private IClient _me;

		public List<User> Friends { get; set; }

		private volatile bool _listening;

		public Client(ITwoWayNotifier notifier, IMessageProtocolProcessor protocolProcessor)
		{
			_notifier = notifier;
			_protocolProcessor = protocolProcessor;

			_tcpClient = new TcpClient();

			InitializeEvents();
		}

		public async Task ConnectAsync(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}

			await _tcpClient.ConnectAsync(Ip.ConnectionIpAddress, Ports.HanshakePort);

			var messageProtocol = new MessageProtocol
			{
				Header = MessageType.ClientConnection,
				Data = name
			};

			await MessageCommunicator.WriteAsync(_tcpClient, messageProtocol);

			_listening = true;

			Listen();
		}

		public async Task SendMessageAsync(string text, User to)
		{
			if (string.IsNullOrEmpty(text) || to == null)
			{
				return;
			}

			var message = new Message
			{
				To = to,
				From = _me.User,
				Text = text
			};

			var messageProtocol = new MessageProtocol
			{
				Header = MessageType.Message,
				Data = message
			};

			await MessageCommunicator.WriteAsync(_tcpClient, messageProtocol);
		}

		public async Task ShutDownAsync()
		{
			_listening = false;

			DeregisterEvents();

			var messageProtocol = new MessageProtocol
			{
				Header = MessageType.ClientDisconnected,
				Data = _me.User
			};

			await MessageCommunicator.WriteAsync(_tcpClient, messageProtocol);

			_tcpClient.Close();
		}

		private  void Listen()
		{
			new Thread(
				async () =>
				{
					while (_listening)
					{
						while(_tcpClient.Available <= 0) Thread.Sleep(100);

						var messageProtocol = await MessageCommunicator.ReadAsync(_tcpClient);
						await _protocolProcessor.ProcessAsync(messageProtocol);
					}
				}).Start();
		}

		private void InitializeEvents()
		{
			_notifier.ConnectedToServer += NotifierOnConnectedToServer;
			_notifier.MessageReceived += NotifierOnMessageReceived;
			_notifier.UserDisconnected += NotifierOnUserDisconnected;
		}

		private void NotifierOnUserDisconnected(object sender, NotifierEventArgs<User> notifierEventArgs)
		{
			if (notifierEventArgs?.Model != null)
			{
				var user = notifierEventArgs.Model;

				Friends.Remove(user);

				UserDisconnected?.Invoke(sender, notifierEventArgs);
			}
		}

		private void DeregisterEvents()
		{
			_notifier.ConnectedToServer -= NotifierOnConnectedToServer;
			_notifier.MessageReceived -= NotifierOnMessageReceived;
			_notifier.UserDisconnected -= NotifierOnUserDisconnected;
		}

		private void NotifierOnMessageReceived(object sender, NotifierEventArgs<Message> notifierEventArgs)
		{
			MessageReceived?.Invoke(this, notifierEventArgs);
		}

		private void NotifierOnConnectedToServer(object sender,
			NotifierEventArgs<ConnectionEstablished> notifierEventArgs)
		{
			if (notifierEventArgs?.Model != null)
			{
				_me = new MeseClient(_tcpClient, notifierEventArgs.Model.Me);
				Friends = notifierEventArgs.Model.Others.ToList();

				ConnectedToServer?.Invoke(this, notifierEventArgs);
			}
		}

		public event EventHandler<NotifierEventArgs<ConnectionEstablished>> ConnectedToServer;
		public event EventHandler<NotifierEventArgs<Message>> MessageReceived;
		public event EventHandler<NotifierEventArgs<User>> UserDisconnected;
		public event EventHandler<NotifierEventArgs<User>> UserConnected;
	}
}
