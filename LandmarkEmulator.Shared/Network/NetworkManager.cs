using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LandmarkEmulator.Shared.Network
{
    public static class NetworkManager<T> where T : NetworkSession, new()
    {
        private static Connection connection;

        private static readonly ConcurrentQueue<T> pendingAdd = new();
        private static readonly ConcurrentQueue<T> pendingRemove = new();

        private static readonly Dictionary<EndPoint, T> sessions = new();

        public static void Initialise(string host, uint port)
        {
            connection = new Connection(IPAddress.Parse(host), port);
            connection.OnMessage += (state, message) =>
            {
                if (sessions.TryGetValue(state.endpoint, out T session))
                    session.OnData(message);
                else
                {
                    var newSession = new T();
                    newSession.OnAccept(state.endpoint);

                    pendingAdd.Enqueue(newSession);
                }
            };
        }

        public static T GetSession(Func<T, bool> func)
        {
            return sessions.Values.SingleOrDefault(func);
        }

        public static void Shutdown()
        {
            //connection?.Shutdown();
        }

        public static void Update(double lastTick)
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

        public static void SendData(IPEndPoint ep, byte[] data)
        {
            connection.SendBytes(ep, data);
        }
    }
}
