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

        public void Initialise(string host, uint port)
        {
            connection = new Connection(IPAddress.Parse(host), port);
            connection.OnMessage += (remoteEP, message) =>
            {
                if (sessions.TryGetValue(remoteEP, out T session))
                    session.OnData(message);
                else
                {
                    var newSession = new T();
                    newSession.OnAccept(remoteEP);

                    pendingAdd.Enqueue(newSession);
                }
            };
        }

        public T GetSession(Func<T, bool> func)
        {
            return sessions.Values.SingleOrDefault(func);
        }

        public void Shutdown()
        {
            //connection?.Shutdown();
        }

        public void Update(double lastTick)
        {
            //
            while (pendingAdd.TryDequeue(out T session))
            {
                sessions.Add(session.Endpoint, session);
                session.OnSend += (endpoint, data) =>
                {
                    connection.SendBytes(endpoint, data);
                };
            }

            //
            foreach (T session in sessions.Values)
            {
                if (session.Disconnected || session.Heartbeat.Flatline)
                    pendingRemove.Enqueue(session);
                else
                    session.Update(lastTick);
            }

            //
            while (pendingRemove.TryDequeue(out T session))
                sessions.Remove(session.Endpoint);
        }

        public void SendData(IPEndPoint ep, byte[] data)
        {
            connection.SendBytes(ep, data);
        }
    }
}
