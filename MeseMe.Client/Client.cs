﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MeseMe.Client.Engine.Events;
using MeseMe.Client.Engine.Models;
using MeseMe.Client.Engine.Notifier;
using MeseMe.Communicator;
using MeseMe.Communicator.Excptions;
using MeseMe.Contracts.Implementations.Models;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Contracts.Interfaces.Settings;
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
		public IClient Me { get; private set; }

		public List<User> Friends { get; set; }

		private volatile bool _listening;

		public Client(ITwoWayNotifier notifier, IMessageProtocolProcessor protocolProcessor)
		{
			_notifier = notifier;
			_protocolProcessor = protocolProcessor;

			_tcpClient = new TcpClient();

			InitializeEvents();
		}

		public async Task ConnectAsync(string name, ISettings settings)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}

			var host = ResolveHostname(settings);
			await ConnectAsyncWithTimeout(_tcpClient, host, settings.Port);

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
				From = Me.User,
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
				Data = Me.User
			};

			await MessageCommunicator.WriteAsync(_tcpClient, messageProtocol);

			_tcpClient.Close();
		}

		private async Task ConnectAsyncWithTimeout(TcpClient client, string host, ushort port)
		{
			var timeOut = TimeSpan.FromSeconds(5);
			var cancellationCompletionSource = new TaskCompletionSource<bool>();
			try
			{
				using (var cts = new CancellationTokenSource(timeOut))
				{
					var task = client.ConnectAsync(host, port);

					using (cts.Token.Register(() => cancellationCompletionSource.TrySetResult(true)))
					{
						if (task != await Task.WhenAny(task, cancellationCompletionSource.Task))
						{
							throw new OperationCanceledException(cts.Token);
						}
					}
				}
			}
			catch (OperationCanceledException)
			{
				throw new ConnectionTimeoutException("Timeout.");
			}
		}

		private string ResolveHostname(ISettings settings)
		{
			return !settings.UseUri && !Ip.TryParse(settings.Uri, out var ipAddress) ? ipAddress.ToString() : settings.Uri;
		}

		private void Listen()
		{
			new Thread(
				async () =>
				{
					while (_listening)
					{
						while (_tcpClient.Available <= 0) Thread.Sleep(100);

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
			_notifier.UserConnected += NotifierOnUserConnected;
		}

		private void NotifierOnUserConnected(object sender, NotifierEventArgs<User> notifierEventArgs)
		{
			if (notifierEventArgs?.Payload != null)
			{
				var user = notifierEventArgs.Payload;

				Friends.Add(user);

				UserConnected?.Invoke(this, notifierEventArgs);
			}
		}

		private void NotifierOnUserDisconnected(object sender, NotifierEventArgs<User> notifierEventArgs)
		{
			if (notifierEventArgs?.Payload != null)
			{
				var user = notifierEventArgs.Payload;

				Friends.Remove(user);

				UserDisconnected?.Invoke(this, notifierEventArgs);
			}
		}

		private void DeregisterEvents()
		{
			_notifier.ConnectedToServer -= NotifierOnConnectedToServer;
			_notifier.MessageReceived -= NotifierOnMessageReceived;
			_notifier.UserDisconnected -= NotifierOnUserDisconnected;
			_notifier.UserConnected -= NotifierOnUserConnected;
		}

		private void NotifierOnMessageReceived(object sender, NotifierEventArgs<Message> notifierEventArgs)
		{
			MessageReceived?.Invoke(this, notifierEventArgs);
		}

		private void NotifierOnConnectedToServer(object sender,
			NotifierEventArgs<ConnectionEstablished> notifierEventArgs)
		{
			if (notifierEventArgs?.Payload != null)
			{
				Me = new MeseClient(_tcpClient, notifierEventArgs.Payload.Me);
				Friends = notifierEventArgs.Payload.Others.ToList();

				ConnectedToServer?.Invoke(this, notifierEventArgs);
			}
		}

		public event EventHandler<NotifierEventArgs<ConnectionEstablished>> ConnectedToServer;
		public event EventHandler<NotifierEventArgs<Message>> MessageReceived;
		public event EventHandler<NotifierEventArgs<User>> UserDisconnected;
		public event EventHandler<NotifierEventArgs<User>> UserConnected;
	}
}
