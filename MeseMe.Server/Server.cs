using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MeseMe.Communicator;
using MeseMe.ConsoleLogger;
using MeseMe.Contracts.Implementations.Models;
using MeseMe.Contracts.Interfaces.Communication;
using MeseMe.Contracts.Interfaces.DataStructure;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Contracts.Interfaces.Settings;
using MeseMe.Server.Engine.Communication;
using MeseMe.Server.Engine.Exceptions;
using MeseMe.Server.Engine.Models;

using static MeseMe.Communicator.Constants.Handshake;

namespace MeseMe.Server
{
	public class Server
	{
		private TcpListener _handshakeListener;
		private readonly IClientsPool _clientsPool;
		private readonly ISettings _settings;
		private readonly IClientService _clientService;
		private readonly IMessageProtocolProcessor _messageProtocolProcessor;

		private volatile bool _handshakeListening;

		public Server(IClientService clientService, IMessageProtocolProcessor protocolProcessor,
			IClientsPool clientsPool, ISettings settings)
		{
			_clientService = clientService;
			_messageProtocolProcessor = protocolProcessor;
			_clientsPool = clientsPool;
			_settings = settings;
		}

		~Server()
		{
			ShutDown();
		}

		public void Start()
		{
			_handshakeListening = true;
			try
			{
				new Thread(async () => await HandshakeAsync()) {IsBackground = true}.Start();
			}
			catch (Exception exception)
			{
				Logger.Error(exception.ToString());
			}
		}

		public async Task HandshakeAsync()
		{
			try
			{
				_handshakeListener = new TcpListener(Ip.Any, _settings.Port);
				_handshakeListener.Start();

				Logger.Info("Server is listening...");

				while (_handshakeListening)
				{
					var client = await _handshakeListener.AcceptTcpClientAsync();
					if (client != null)
					{
						new Thread(async () => { await HandleClientAsync(client); }).Start();
					}
				}
			}
			catch (SocketException exception)
			{
				Logger.Error($"Socket exception occured {exception}");
			}
		}

		public void ShutDown()
		{
			Logger.WriteLine("Server shutting down.", ConsoleColor.Red);

			if (_handshakeListening)
			{
				_handshakeListening = false;
				_handshakeListener.Stop();

				foreach (var connectedTcpClient in _clientsPool.ConnectedClients)
				{
					connectedTcpClient.Value.TcpClient.Close();
				}
			}

			Logger.WriteLine("Server shutdown succesfully.", ConsoleColor.Red);
		}
		
		private async Task HandleClientAsync(TcpClient client)
		{
			try
			{
				var meseClient = await GetMeseClientInformation(client);

				if (meseClient != null)
				{
					Logger.Info($"Client connected: {meseClient?.User}");
					_clientsPool.Add(meseClient);

					IClientHandler meseClientHandler;
					if ((meseClientHandler = await _clientService.CompleteHandshakeAsync(meseClient)) != null)
					{
						meseClientHandler.MessageProtocolReceived += async (sender, args) =>
						{
							if (args?.MessageProtocol == null) return;
							await _messageProtocolProcessor.ProcessAsync(args?.MessageProtocol);
						};

						await meseClientHandler.StartListeningAsync();
					}
				}
			}
			catch (ClientDisconnectedException ex)
			{
				await ForcelyDisconnectClient(ex.Client);
			}
			catch (Exception ex)
			{
				Logger.Error(ex.ToString());
			}
		}

		private async Task ForcelyDisconnectClient(IClient user)
		{
			if (user?.User != null && _clientsPool.Remove(user.User.Id))
			{
				var messageProtocol = new MessageProtocol
				{
					Header = MessageType.ClientDisconnected,
					Data = user.User
				};

				var tcpClients = _clientsPool.ConnectedClients.Select(i => i.Value.TcpClient).ToArray();
				if (tcpClients.Any())
				{
					await MessageCommunicator.BroadcastAsync(tcpClients, messageProtocol);
				}
			}

			Logger.Info($"Client disconnected abruptely. {user}");
		}

		private void WaitForClient(TcpClient client)
		{
			var maxTries = 10;
			var tries = 0;

			while (!client.Connected || tries++ < maxTries)
			{
				Thread.Sleep(100);
			}
		}

		private async Task<IClient> GetMeseClientInformation(TcpClient client)
		{
			MeseClient meseClient = null;

			WaitForClient(client);

			var messageProtocol = await MessageCommunicator.ReadAsync(client);

			var name = messageProtocol.GetDataAs<string>();

			if (!string.IsNullOrEmpty(name))
			{
				meseClient = new MeseClient(client, name);
			}

			return meseClient;
		}
	}
}
