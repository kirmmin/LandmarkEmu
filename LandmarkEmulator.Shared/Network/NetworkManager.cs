using LandmarkEmulator.Shared.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LandmarkEmulator.Shared.Network
{
    public class NetworkManager<T> : Singleton<NetworkManager<T>> where T : NetworkSession, new()
    {
        private Connection connection;

        private readonly ConcurrentQueue<T> pendingAdd = new();
        private readonly ConcurrentQueue<T> pendingRemove = new();

        private readonly Dictionary<EndPoint, T> sessions = new();

        /// <summary>
        /// Initialises a new <see cref="NetworkManager{T}"/>.
        /// </summary>
        public void Initialise(NetworkConfig config)
        {
            connection = new Connection(IPAddress.Parse(config.Host), config.Port);
            connection.OnMessage += (remoteEP, message) =>
            {
                if (sessions.TryGetValue(remoteEP, out T session))
                    session.OnData(message);
                else
                {
                    var newSession = new T();
                    newSession.OnAccept(remoteEP);

                    pendingAdd.Enqueue(newSession);
                    newSession.OnData(message);
                }
            };
        }

        /// <summary>
        /// Returns <see cref="{T}"/> that matches the predicate. Must be a Single match, or error.
        /// </summary>
        public T GetSession(Func<T, bool> func)
        {
            return sessions.Values.SingleOrDefault(func);
        }

        /// <summary>
        /// Shutdown this <see cref="NetworkManager"/>, releasing any connections and ending all sessions.
        /// </summary>
        public void Shutdown()
        {
            foreach (T session in sessions.Values)
                session.OnDisconnect();

            connection?.Shutdown();
        }

        /// <summary>
        /// Invoked each server tick with the delta since the previous tick occurred.
        /// </summary>
        public void Update(double lastTick)
        {
            // Add new sessions to be operated on.
            while (pendingAdd.TryDequeue(out T session))
            {
                sessions.Add(session.Endpoint, session);
                session.OnSend += (endpoint, data) =>
                {
                    connection.SendBytes(endpoint, data);
                };
            }

            // Operate on existing sessions.
            foreach (T session in sessions.Values)
            {
                if (session.Disconnected || session.Heartbeat.Flatline)
                    pendingRemove.Enqueue(session);
                else
                    session.Update(lastTick);
            }

            // Remove any sessions that are pending removal.
            while (pendingRemove.TryDequeue(out T session))
                sessions.Remove(session.Endpoint);
        }

        /// <summary>
        /// Using the <see cref="Connection"/> for this <see cref="NetworkManager{T}"/>, send data to the given <see cref="IPEndPoint"/>.
        /// </summary>
        public void SendData(IPEndPoint ep, byte[] data)
        {
            connection.SendBytes(ep, data);
        }
    }
}
