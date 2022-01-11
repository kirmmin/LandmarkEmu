using LandmarkEmulator.Shared.Game.Events;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;

namespace LandmarkEmulator.Shared.Network
{
    public abstract class NetworkSession : IUpdate
    {
        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public bool Disconnected { get; private set; }
        public SocketHeartbeat Heartbeat { get; } = new SocketHeartbeat();
        public EndPoint Endpoint { get; private set; }

        /// <summary>
        /// <see cref="IEvent"/> queue that will be processed during <see cref="NetworkSession"/> update.
        /// </summary>
        public Queue<IEvent> Events { get; } = new();

        public delegate void SendEvent(EndPoint endpoint, byte[] message);

        /// <summary>
        /// Raised on <see cref="NetworkSession"/> creation for a new client.
        /// </summary>
        public event SendEvent OnSend;

        public virtual void OnAccept(EndPoint ep)
        {
            Endpoint = ep;
        }

        /// <summary>
        /// Invoked each server tick with the delta since the previous tick occurred.
        /// </summary>
        public virtual void Update(double lastTick)
        {
            Heartbeat.Update(lastTick);
            if (Heartbeat.Flatline)
                OnDisconnect();

            while (Events.TryPeek(out IEvent @event))
            {
                if (!@event.CanExecute())
                    continue;

                Events.Dequeue();

                try
                {
                    @event.Execute();
                }
                catch (Exception exception)
                {
                    log.Error(exception);
                }
            }
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

        public virtual void OnDisconnect()
        {
            Disconnected = true;

            log.Trace("Client disconnected.");
        }
    }
}
