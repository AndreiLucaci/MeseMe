using System.Net;

namespace MeseMe.Handshake
{
    public static class HS
    {
	    public static class Ports
	    {
		    public const ushort HanshakePort = 34534;
		    public const ushort MessagingPort = 34535;
		}

	    public static class Message
	    {
		    public const ushort BufferSize = 1024;
		}

		public static class Ip
	    {
		    public static IPAddress ConnectionIpAddress = IPAddress.Parse("localhost");
			public static IPAddress Any = IPAddress.Any;
	    }
	}
}
