namespace MeseMe.Contracts.Implementations.Models
{
	public class MessageType
	{
		public ushort Type { get;set; }

		public static MessageType ClientConnectedSelf = new MessageType {Type = MessageTypes.ClientConnectedSelf};
		public static MessageType ClientConnectedOthers = new MessageType {Type = MessageTypes.ClientConnectedOthers};
		public static MessageType Message = new MessageType {Type = MessageTypes.Message};
		public static MessageType ClientConnection = new MessageType {Type = MessageTypes.ClientConnection};
		public static MessageType ClientDisconnected = new MessageType {Type = MessageTypes.ClientDisconnected};
	}
}
