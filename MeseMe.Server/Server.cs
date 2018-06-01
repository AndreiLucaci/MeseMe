using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MeseMe.Communicator;
using MeseMe.Contracts.Interfaces.Communication;
using MeseMe.Contracts.Interfaces.DataStructure;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Server.Engine.Communication;
using MeseMe.Server.Engine.Models;
using MeseMe.ServerLogger;

using static MeseMe.Communicator.Constants.Handshake;


namespace MeseMe.Server
{
	public class Server
	{
		private TcpListener _handshakeListener;
		private readonly IClientsPool _clientsPool;
		private readonly IClientService _clientService;
		private readonly IMessageProtocolProcessor _messageProtocolProcessor;

		private volatile bool _handshakeListening;

		public Server(IClientService clientService, IMessageProtocolProcessor protocolProcessor,
			IClientsPool clientsPool)
		{
			_clientService = clientService;
			_messageProtocolProcessor = protocolProcessor;
			_clientsPool = clientsPool;
		}

		~Server()
		{
			ShutDown();
		}

		public void Start()
		{
			_handshakeListening = true;

			new Thread(async () => await HandshakeAsync() ) { IsBackground = true }.Start();
		}

		public async Task HandshakeAsync()
		{
			try
			{
				_handshakeListener = new TcpListener(Ip.Any, Ports.HanshakePort);
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
			if (_handshakeListening)
			{
				_handshakeListening = false;
				_handshakeListener.Stop();

				foreach (var connectedTcpClient in _clientsPool.ConnectedClients)
				{
					connectedTcpClient.Value.TcpClient.Close();
				}
			}
		}
		
		private async Task HandleClientAsync(TcpClient client)
		{
			var meseClient = await GetMeseClientInformation(client);

			_clientsPool.Add(meseClient);

			if (meseClient != null)
			{
				IClientHandler meseClientHandler;

				if ((meseClientHandler = await _clientService.CompleteHandshakeAsync(meseClient)) != null)
				{
					meseClientHandler.MessageProtocolReceived += async (sender, args) =>
					{
						await _messageProtocolProcessor.ProcessAsync(args?.MessageProtocol);
					};

					await meseClientHandler.StartListeningAsync();
				}
			}
		}

		private async Task<IClient> GetMeseClientInformation(TcpClient client)
		{
			MeseClient meseClient = null;

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
