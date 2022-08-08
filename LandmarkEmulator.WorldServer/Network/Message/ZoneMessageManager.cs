using LandmarkEmulator.Gateway.Network.Message;
using LandmarkEmulator.Shared;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LandmarkEmulator.WorldServer.Network.Message
{
    public delegate void ZoneMessageHandlerDelegate(NetworkSession session, IReadable message);

    public class ZoneMessageManager : Singleton<ZoneMessageManager>
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private delegate IReadable MessageFactoryDelegate();

        private ImmutableDictionary<ZoneMessageOpcode, MessageFactoryDelegate> clientMessageFactories;
        private ImmutableDictionary<Type, ZoneMessageOpcode> serverMessageOpcodes;

        private Dictionary<ClientProtocol, ImmutableDictionary<ZoneMessageOpcode, ZoneMessageHandlerDelegate>> clientMessageHandlers = new();

        public void Initialise()
        {
            InitialiseGameMessages();
            InitialiseGameMessageHandlers(ClientProtocol.LandmarkClientProtocol_709);
            log.Info($"Initialised {clientMessageHandlers.Values.Sum(x => x.Count)} Gateway message handler(s).");
        }

        private void InitialiseGameMessages()
        {
            var messageFactories = new Dictionary<ZoneMessageOpcode, MessageFactoryDelegate>();
            var messageOpcodes = new Dictionary<Type, ZoneMessageOpcode>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                ZoneMessageAttribute attribute = type.GetCustomAttribute<ZoneMessageAttribute>();
                if (attribute == null)
                    continue;

                if (typeof(IReadable).IsAssignableFrom(type))
                {
                    NewExpression @new = Expression.New(type.GetConstructor(Type.EmptyTypes));
                    messageFactories.Add(attribute.Opcode, Expression.Lambda<MessageFactoryDelegate>(@new).Compile());
                }
                messageOpcodes.Add(type, attribute.Opcode);
            }

            clientMessageFactories = messageFactories.ToImmutableDictionary();
            serverMessageOpcodes = messageOpcodes.ToImmutableDictionary();
            //log.Info($"Initialised {clientMessageFactories.Count} Gateway message {(clientMessageFactories.Count == 1 ? "factory" : "factories")}.");
            log.Info($"Initialised {serverMessageOpcodes.Count} Zone message(s).");
        }

        private void InitialiseGameMessageHandlers(ClientProtocol version)
        {
            var messageHandlers = new Dictionary<ZoneMessageOpcode, ZoneMessageHandlerDelegate>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.DeclaringType != type)
                        continue;

                    ZoneMessageHandlerAttribute attribute = method.GetCustomAttribute<ZoneMessageHandlerAttribute>();
                    if (attribute == null)
                        continue;

                    if (!attribute.ProtocolVersion.HasFlag(version))
                        continue;

                    ParameterExpression sessionParameter = Expression.Parameter(typeof(NetworkSession));
                    ParameterExpression messageParameter = Expression.Parameter(typeof(IReadable));

                    ParameterInfo[] parameterInfo = method.GetParameters();

                    if (method.IsStatic)
                    {
                        #region Debug
                        Debug.Assert(parameterInfo.Length == 2);
                        Debug.Assert(typeof(NetworkSession).IsAssignableFrom(parameterInfo[0].ParameterType));
                        Debug.Assert(typeof(IReadable).IsAssignableFrom(parameterInfo[1].ParameterType));
                        #endregion

                        MethodCallExpression call = Expression.Call(method,
                            Expression.Convert(sessionParameter, parameterInfo[0].ParameterType),
                            Expression.Convert(messageParameter, parameterInfo[1].ParameterType));

                        Expression<ZoneMessageHandlerDelegate> lambda =
                            Expression.Lambda<ZoneMessageHandlerDelegate>(call, sessionParameter, messageParameter);

                        messageHandlers.Add(attribute.Opcode, lambda.Compile());
                    }
                    else
                    {
                        #region Debug
                        Debug.Assert(parameterInfo.Length == 1);
                        Debug.Assert(typeof(NetworkSession).IsAssignableFrom(type));
                        Debug.Assert(typeof(IReadable).IsAssignableFrom(parameterInfo[0].ParameterType));
                        #endregion

                        MethodCallExpression call = Expression.Call(
                            Expression.Convert(sessionParameter, type),
                            method,
                            Expression.Convert(messageParameter, parameterInfo[0].ParameterType));

                        Expression<ZoneMessageHandlerDelegate> lambda =
                            Expression.Lambda<ZoneMessageHandlerDelegate>(call, sessionParameter, messageParameter);

                        messageHandlers.Add(attribute.Opcode, lambda.Compile());
                    }
                }
            }

            clientMessageHandlers.Add(version, messageHandlers.ToImmutableDictionary());
        }

        public bool GetOpcode(IWritable message, out ZoneMessageOpcode opcode)
        {
            return serverMessageOpcodes.TryGetValue(message.GetType(), out opcode);
        }

        public IReadable GetZoneMessage(ZoneMessageOpcode opcode, ClientProtocol version)
        {
            return clientMessageFactories.TryGetValue(opcode, out MessageFactoryDelegate factory)
                ? factory.Invoke() : null;
        }

        public ZoneMessageHandlerDelegate GetZoneMessageHandler(ZoneMessageOpcode opcode, ClientProtocol version)
        {
            if (clientMessageHandlers.TryGetValue(version, out var dict))
                return dict.TryGetValue(opcode, out ZoneMessageHandlerDelegate handler)
                ? handler : null;

            return null;
        }
    }
}
