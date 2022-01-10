using LandmarkEmulator.Shared;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace LandmarkEmulator.AuthServer.Network.Message
{
    public delegate void AuthMessageHandlerDelegate(NetworkSession session, IReadable message);

    public class AuthMessageManager : Singleton<AuthMessageManager>
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private delegate IReadable MessageFactoryDelegate();

        private ImmutableDictionary<AuthMessageOpcode, MessageFactoryDelegate> clientMessageFactoriesLogin9;
        private ImmutableDictionary<AuthMessageOpcode, MessageFactoryDelegate> clientMessageFactoriesLogin10;
        private ImmutableDictionary<Type, AuthMessageOpcode> serverMessageOpcodes;

        private Dictionary<ProtocolVersion, ImmutableDictionary<AuthMessageOpcode, AuthMessageHandlerDelegate>> clientMessageHandlers = new();

        public void Initialise()
        {
            InitialiseGameMessages();
            InitialiseGameMessageHandlers(ProtocolVersion.LoginUdp_9);
            InitialiseGameMessageHandlers(ProtocolVersion.LoginUdp_10);
        }

        private void InitialiseGameMessages()
        {
            var messageFactories9 = new Dictionary<AuthMessageOpcode, MessageFactoryDelegate>();
            var messageFactories10 = new Dictionary<AuthMessageOpcode, MessageFactoryDelegate>();
            var messageOpcodes = new Dictionary<Type, AuthMessageOpcode>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                AuthMessageAttribute attribute = type.GetCustomAttribute<AuthMessageAttribute>();
                if (attribute == null)
                    continue;

                if (typeof(IReadable).IsAssignableFrom(type))
                {
                    NewExpression @new = Expression.New(type.GetConstructor(Type.EmptyTypes));
                    if (attribute.Version.HasFlag(ProtocolVersion.LoginUdp_9))
                        messageFactories9.Add(attribute.Opcode, Expression.Lambda<MessageFactoryDelegate>(@new).Compile());
                    if (attribute.Version.HasFlag(ProtocolVersion.LoginUdp_10))
                        messageFactories10.Add(attribute.Opcode, Expression.Lambda<MessageFactoryDelegate>(@new).Compile());
                }
                messageOpcodes.Add(type, attribute.Opcode);
            }

            clientMessageFactoriesLogin9 = messageFactories9.ToImmutableDictionary();
            clientMessageFactoriesLogin10 = messageFactories10.ToImmutableDictionary();
            serverMessageOpcodes = messageOpcodes.ToImmutableDictionary();
            //log.Info($"Initialised {clientMessageFactories.Count} Auth message {(clientMessageFactories.Count == 1 ? "factory" : "factories")}.");
            log.Info($"Initialised {serverMessageOpcodes.Count} Auth message(s).");
        }

        private void InitialiseGameMessageHandlers(ProtocolVersion version)
        {
            var messageHandlers = new Dictionary<AuthMessageOpcode, AuthMessageHandlerDelegate>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.DeclaringType != type)
                        continue;

                    AuthMessageHandlerAttribute attribute = method.GetCustomAttribute<AuthMessageHandlerAttribute>();
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

                        Expression<AuthMessageHandlerDelegate> lambda =
                            Expression.Lambda<AuthMessageHandlerDelegate>(call, sessionParameter, messageParameter);

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

                        Expression<AuthMessageHandlerDelegate> lambda =
                            Expression.Lambda<AuthMessageHandlerDelegate>(call, sessionParameter, messageParameter);

                        messageHandlers.Add(attribute.Opcode, lambda.Compile());
                    }
                }
            }

            clientMessageHandlers.Add(version, messageHandlers.ToImmutableDictionary());
            log.Info($"Initialised {clientMessageHandlers.Count} Auth message handler(s).");
        }

        public bool GetOpcode(IWritable message, out AuthMessageOpcode opcode)
        {
            return serverMessageOpcodes.TryGetValue(message.GetType(), out opcode);
        }

        public IReadable GetAuthMessage(AuthMessageOpcode opcode, ProtocolVersion version)
        {
            if (version.HasFlag(ProtocolVersion.LoginUdp_10))
                return clientMessageFactoriesLogin10.TryGetValue(opcode, out MessageFactoryDelegate factory11)
                ? factory11.Invoke() : null;

            return clientMessageFactoriesLogin9.TryGetValue(opcode, out MessageFactoryDelegate factory)
                ? factory.Invoke() : null;
        }

        public AuthMessageHandlerDelegate GetGameMessageHandler(AuthMessageOpcode opcode, ProtocolVersion version)
        {
            if (clientMessageHandlers.TryGetValue(version, out var dict))
                return dict.TryGetValue(opcode, out AuthMessageHandlerDelegate handler)
                ? handler : null;

            return null;
        }
    }
}
