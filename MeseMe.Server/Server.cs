using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using MeseMe.Handshake;
using MeseMe.Models.Messages;
using MeseMe.Server.Engine;
using MeseMe.Server.Engine.Clients;
using MeseMe.Server.Engine.Communication;
using MeseMe.ServerLogger;
using Newtonsoft.Json;

namespace MeseMe.Server
{
	public class Server
	{
		private TcpListener _handshakeListener;
		private TcpListener _messageListener;
		private Thread _tcpListenerThread;
		private readonly ConcurrentBag<MeseClient> _connectedClients = new ConcurrentBag<MeseClient>();
		private volatile bool _handshakeListening;
		private volatile bool _messageListening;

		public void Start()
		{
			_handshakeListening = true;
			_messageListening = true;
			_tcpListenerThread = new Thread(() =>
			{
				new Thread(Handshake).Start();
				//new Thread(Converse).Start();
			}) {IsBackground = true};
			_tcpListenerThread.Start();
		}

		public void Handshake()
		{
			try
			{
				_handshakeListener = new TcpListener(HS.Ip.Any, HS.Ports.HanshakePort);
				_handshakeListener.Start();

				Logger.Info("Server is listening...");

				while (_handshakeListening)
				{
					var client = _handshakeListener.AcceptTcpClient();
					new Thread(() =>
					{
						var meseClient = new MeseClient(client);
						_connectedClients.Add(meseClient);
					}).Start();
				}
			}
			catch (SocketException exception)
			{
				Logger.Error($"Socket exception occured {exception}");
			}
		}

		public void ListenForMessages()
		{
			try
			{
				_messageListener = new TcpListener(HS.Ip.Any, HS.Ports.MessagingPort);
				_messageListener.Start();

				while (_messageListening)
				{
					var client = _messageListener.AcceptTcpClient();
				}
			}
		}

		public void StartConversation()
		{
			
		}

		public void CompleteHandshake(MeseClient meseClient)
		{
			try
			{
				using (var ns = meseClient.TcpClient.GetStream())
				{
					if (ns.CanWrite)
					{
						var connectionEstablished = new ConnectionEstablished
						{
							Me = meseClient.Id,
							Others = _connectedClients.Where(i => i.Id != meseClient.Id).Select(i => i.Id)
						};

						var encodedPart = JsonConvert.SerializeObject(connectionEstablished);
						var bytes = Encoding.UTF8.GetBytes(encodedPart);
						ns.Write(bytes, 0, bytes.Length);
						Logger.Info($"Handshake succedded with client {meseClient.Id}");
					}
				}
			}
			catch (SocketException sockedException)
			{
				Logger.Error($"Socked exception {sockedException}");
			}
		}

		public Message Read(TcpClient client)
		{
			var bytes = new byte[HS.Message.BufferSize];
			var messageBuilder = new MessageBuilder();

			using (var ns = client.GetStream())
			{
				int length;
				while ((length = ns.Read(bytes, 0, bytes.Length)) != 0)
				{
					var incommingData = new byte[length];
					Array.Copy(bytes, 0, incommingData, 0, length);

					messageBuilder.AddPart(incommingData);
				}
			}

			return messageBuilder.To<Message>();
		}

		public void Reply(Message message)
		{
			
		}

		public void ShutDown()
		{
			_handshakeListening = false;
			_handshakeListener.Stop();

			_messageListening = false;
			_messageListener.Stop();

			foreach (var connectedTcpClient in _connectedClients)
			{
				connectedTcpClient.TcpClient.Dispose();
			}
		}
	}
}
