using System;
using System.Net;
using System.Net.Sockets;

namespace LandmarkEmulator.Shared.Network
{
    public abstract class NetworkSession : Session
    {
        public bool Disconnected { get; private set; }
        public SocketHeartbeat Heartbeat { get; } = new SocketHeartbeat();

        public IPEndPoint Endpoint { get; private set; }

        public delegate void SendEvent(IPEndPoint endpoint, byte[] message);

        /// <summary>
        /// Raised on <see cref="NetworkSession"/> creation for a new client.
        /// </summary>
        public event SendEvent OnSend;

        public virtual void OnAccept(IPEndPoint ep)
        {
            Endpoint = ep;
        }

        public override void Update(double lastTick)
        {
            base.Update(lastTick);

            Heartbeat.Update(lastTick);
            if (Heartbeat.Flatline)
                OnDisconnect();
        }

        protected void SendRaw(byte[] data)
        {
            try
            {
                OnSend(Endpoint, data);
            }
            catch (Exception ex)
            {
                log.Error($"{ex}");
                OnDisconnect();
            }
        }

        public virtual void OnData(byte[] data)
        {
        }

        protected virtual void OnDisconnect()
        {
            Disconnected = true;

            log.Trace("Client disconnected.");
        }
    }
}
