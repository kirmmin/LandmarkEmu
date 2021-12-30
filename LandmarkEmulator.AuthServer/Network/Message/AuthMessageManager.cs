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

namespace LandmarkEmulator.AuthServer.Network.Message
{
    public delegate void AuthMessageHandlerDelegate(NetworkSession session, IReadable message);

    public static class AuthMessageManager
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private delegate IReadable MessageFactoryDelegate();

        private static ImmutableDictionary<AuthMessageOpcode, MessageFactoryDelegate> clientMessageFactories;
        private static ImmutableDictionary<Type, AuthMessageOpcode> serverMessageOpcodes;

        private static ImmutableDictionary<AuthMessageOpcode, AuthMessageHandlerDelegate> clientMessageHandlers;

        public static void Initialise()
        {
            InitialiseGameMessages();
            InitialiseGameMessageHandlers();
        }

        private static void InitialiseGameMessages()
        {
            var messageFactories = new Dictionary<AuthMessageOpcode, MessageFactoryDelegate>();
            var messageOpcodes = new Dictionary<Type, AuthMessageOpcode>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                AuthMessageAttribute attribute = type.GetCustomAttribute<AuthMessageAttribute>();
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
            log.Info($"Initialised {clientMessageFactories.Count} Auth message {(clientMessageFactories.Count == 1 ? "factory" : "factories")}.");
            log.Info($"Initialised {serverMessageOpcodes.Count} Auth message(s).");
        }

        private static void InitialiseGameMessageHandlers()
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

            clientMessageHandlers = messageHandlers.ToImmutableDictionary();
            log.Info($"Initialised {clientMessageHandlers.Count} Auth message handler(s).");
        }

        public static bool GetOpcode(IWritable message, out AuthMessageOpcode opcode)
        {
            return serverMessageOpcodes.TryGetValue(message.GetType(), out opcode);
        }

        public static IReadable GetAuthMessage(AuthMessageOpcode opcode)
        {
            return clientMessageFactories.TryGetValue(opcode, out MessageFactoryDelegate factory)
                ? factory.Invoke() : null;
        }

        public static AuthMessageHandlerDelegate GetGameMessageHandler(AuthMessageOpcode opcode)
        {
            return clientMessageHandlers.TryGetValue(opcode, out AuthMessageHandlerDelegate handler)
                ? handler : null;
        }
    }
}
