using System;
using MeseMe.Client.Engine.Events;
using MeseMe.Models.Messages;
using MeseMe.Models.Users;

namespace MeseMe.Client.Engine.Notifier
{
	public class TwoWayNotifier : ITwoWayNotifier
	{
		public event EventHandler<NotifierEventArgs<ConnectionEstablished>> ConnectedToServer;
		public event EventHandler<NotifierEventArgs<Message>> MessageReceived;
		public event EventHandler<NotifierEventArgs<User>> UserDisconnected;
		public event EventHandler<NotifierEventArgs<User>> UserConnected;

		public void OnConnectedToServer(object sender, NotifierEventArgs<ConnectionEstablished> eventArgs)
		{
			ConnectedToServer?.Invoke(sender, eventArgs);
		}

		public void OnMessageReceived(object sender, NotifierEventArgs<Message> eventArgs)
		{
			MessageReceived?.Invoke(sender, eventArgs);
		}

		public void OnUserDisconnected(object sender, NotifierEventArgs<User> eventArgs)
		{
			UserDisconnected?.Invoke(sender, eventArgs);
		}

		public void OnUserConnected(object sender, NotifierEventArgs<User> eventArgs)
		{
			UserConnected?.Invoke(sender, eventArgs);
		}
	}
}
