using NLog;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.Network
{
    public class Connection
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public class UdpState
        {
            public UdpState (UdpClient c, IPEndPoint e)
            {
                client = c;
                endpoint = e;
            }

            public UdpClient client;
            public IPEndPoint endpoint;
        }
        UdpState connection;

        public delegate void MessageEvent(UdpState state, byte[] message);
        /// <summary>
        /// Raised on <see cref="NetworkSession"/> creation for a new client.
        /// </summary>
        public event MessageEvent OnMessage;

        private volatile bool shutdownRequested;

        public Connection(IPAddress host, uint port)
        {
            var connection = SetupUdpClient(host, port);

            _ = Task.Run(() =>
            {
                log.Info($"Listening on port {port}");
                while (!shutdownRequested)
                {
                    this.connection.client.BeginReceive(new AsyncCallback(OnReceive), connection);
                }
            });
        }

        private UdpState SetupUdpClient(IPAddress host, uint port)
        {
            IPEndPoint localEP = new IPEndPoint(host, (int)port);
            UdpClient udpClient = new UdpClient();
            udpClient.ExclusiveAddressUse = false;
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Client.Bind(localEP);

            connection = new UdpState(udpClient, localEP);
            return new UdpState(udpClient, localEP);
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                var state = (UdpState)ar.AsyncState;

                byte[] receiveBytes = state.client.EndReceive(ar, ref state.endpoint);
                OnMessage(state, receiveBytes);
            }
            catch (Exception e)
            {
                //You may also get a SocketException if you close it in a separate thread.
                if (e is ObjectDisposedException || e is SocketException)
                {
                    //Log it as a trace here
                    return;
                }
                //Wasn't an exception we were looking for so rethrow it.
                throw;
            }
        }

        public void SendBytes(IPEndPoint endPoint, byte[] data)
        {
            var sender = new UdpClient();
            sender.ExclusiveAddressUse = false;
            sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            sender.Client.Bind(connection.endpoint);
            sender.Send(data, data.Length, endPoint);
            sender.Close();
        }

        public void Shutdown()
        {
            shutdownRequested = true;
        }
    }
}
