﻿using System.Net;

namespace MeseMe.Communicator.Constants
{
    public static class Handshake
    {
	    public static class Ports
	    {
		    public const ushort HanshakePort = 34534;
	    }

	    public static class Buffers
	    {
		    public const ushort BufferSize = 8192;
		}

		public static class Ip
	    {
		    public static IPAddress ConnectionIpAddress = IPAddress.Parse("127.0.0.1");
			public static IPAddress Any = IPAddress.Any;

		    public static bool TryParse(string ip, out IPAddress ipAddress)
		    {
			    return IPAddress.TryParse(ip, out ipAddress);
		    }
	    }
	}
}
