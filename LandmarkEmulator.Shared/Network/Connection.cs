using NLog;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.Network
{
    public class Connection
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public delegate void MessageEvent(EndPoint state, byte[] message);
        /// <summary>
        /// Raised on <see cref="NetworkSession"/> creation for a new client.
        /// </summary>
        public event MessageEvent OnMessage;

        private IPAddress _host;
        private uint _port;
        private Thread sampleUdpThread;

        /// <summary>
        /// Creates a new <see cref="Connection"/> for the provided details that will listen to data receives and emit <see cref="MessageEvent"/>.
        /// </summary>
        /// <param name="host">Host IP Address to listed on</param>
        /// <param name="port">Port to listen on</param>
        public Connection(IPAddress host, uint port)
        {
            _host = host;
            _port = port;

            try
            {
                //Starting the UDP Server thread.
                sampleUdpThread = new Thread(new ThreadStart(StartReceiving));
                sampleUdpThread.Start();
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                sampleUdpThread.Abort();
            }
        }

        private void StartReceiving()
        {
            try
            {
                //Create a UDP socket.
                Socket soUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint localIpEndPoint = new IPEndPoint(_host, (int)_port);
                
                // We don't bind exclusively because we need to use the same socket address to send data in SendBytes.
                soUdp.ExclusiveAddressUse = false;
                soUdp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                soUdp.Bind(localIpEndPoint);
                while (true)
                {
                    // TODO: Make the UdpBufferSize something that is passed in.
                    byte[] buffer = new byte[512];
                    IPEndPoint tmpIpEndPoint = new IPEndPoint(_host, (int)_port);
                    EndPoint remoteEP = (tmpIpEndPoint);
                    int bytesReceived = soUdp.ReceiveFrom(buffer, ref remoteEP); // This is a blocking call. The thread will wait to hear back from the Socket
                    if (bytesReceived > 0)
                    {
                        Span<byte> data = new Span<byte>(buffer, 0, bytesReceived); // Remove the extra bytes that were unused in this packet.
                        OnMessage(remoteEP, data.ToArray()); // Emit a MessageEvent to subscribers.
                    }
                }
            }
            catch (SocketException se)
            {
                log.Error("A Socket Exception has occurred!" + se.ToString());
            }
        }

        /// <summary>
        /// Sends the given <see cref="byte[]"/> to the target <see cref="EndPoint"/>.
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="data"></param>
        public void SendBytes(EndPoint endPoint, byte[] data)
        {
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint localIpEndPoint = new IPEndPoint(_host, (int)_port);
            sender.ExclusiveAddressUse = false;
            sender.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            sender.Bind(localIpEndPoint);
            sender.SendTo(data, endPoint);
            sender.Close(); // Close the Socket so another may be opened.
        }
    }
}
