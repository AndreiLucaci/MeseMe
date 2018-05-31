using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using MeseMe.Handshake;
using MeseMe.Models.Messages;
using MeseMe.Server.Engine;
using MeseMe.Server.Engine.Clients;
using MeseMe.Server.Engine.Communication;
using MeseMe.ServerLogger;

namespace MeseMe.Server
{
	public class Server
	{
		private TcpListener _tcpListener;
		private Thread _tcpListenerThread;
		private readonly ConcurrentBag<MeseClient> _connectedTcpClients = new ConcurrentBag<MeseClient>();
		private volatile bool _listening;

		public void Start()
		{
			_listening = true;
			_tcpListenerThread = new Thread(() =>
			{
				new Thread(Listen).Start();
				//new Thread(Converse).Start();
			}) {IsBackground = true};
			_tcpListenerThread.Start();
		}

		public void Listen()
		{
			try
			{
				_tcpListener = new TcpListener(HS.Ip.Any, HS.Ports.HanshakePort);
				_tcpListener.Start();

				Logger.Info("Server is listening...");

				while (_listening)
				{
					var client = _tcpListener.AcceptTcpClient();
					new Thread(() =>
					{
						var meseClient = new MeseClient(client);
						_connectedTcpClients.Add(meseClient);
					}).Start();
				}
			}
			catch (SocketException exception)
			{
				Logger.Error($"Socket exception occured {exception}");
			}
		}

		public void StartConversation()
		{
			
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
			_listening = false;
			_tcpListener.Stop();

			foreach (var connectedTcpClient in _connectedTcpClients)
			{
				connectedTcpClient.TcpClient.Dispose();
			}
		}
	}
}
