using MeseMe.Client.Engine.Events;
using MeseMe.Models.Messages;
using MeseMe.Models.Users;

namespace MeseMe.Client.Engine.Notifier
{
	public interface INotifierActions
	{
		void OnConnectedToServer(object sender, NotifierEventArgs<ConnectionEstablished> eventArgs);
		void OnMessageReceived(object sender, NotifierEventArgs<Message> eventArgs);
		void OnUserDisconnected(object sender, NotifierEventArgs<User> eventArgs);
		void OnUserConnected(object sender, NotifierEventArgs<User> eventArgs);
	}
}
