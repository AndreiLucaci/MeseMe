using System;
using MeseMe.Client.Engine.Events;
using MeseMe.Models.Messages;
using MeseMe.Models.Users;

namespace MeseMe.Client.Engine.Notifier
{
	public interface INotifierEvents
	{
		event EventHandler<NotifierEventArgs<ConnectionEstablished>> ConnectedToServer;
		event EventHandler<NotifierEventArgs<Message>> MessageReceived;
		event EventHandler<NotifierEventArgs<User>> UserDisconnected;
		event EventHandler<NotifierEventArgs<User>> UserConnected;
	}
}
