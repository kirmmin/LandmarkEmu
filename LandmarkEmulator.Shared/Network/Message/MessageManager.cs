using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.Network.Message
{
    public delegate void MessageHandlerDelegate(NetworkSession session, IReadable message);

    public static class MessageManager
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private delegate IReadable MessageFactoryDelegate();

        private static ImmutableDictionary<ProtocolMessageOpcode, MessageFactoryDelegate> protocolMessagefactories;
        private static ImmutableDictionary<ProtocolMessageOpcode, MessageHandlerDelegate> protocolMessageHandlers;
        private static ImmutableDictionary<Type, ProtocolMessageOpcode> protocolMessageOpcodes;

        private static ImmutableDictionary<GameMessageOpcode, MessageFactoryDelegate> clientMessageFactories;
        private static ImmutableDictionary<Type, GameMessageOpcode> serverMessageOpcodes;

        private static ImmutableDictionary<GameMessageOpcode, MessageHandlerDelegate> clientMessageHandlers;

        public static void Initialise()
        {
            InitialiseProtocolMessages();
            InitialiseProtocolMessageHandlers();
            InitialiseGameMessages();
            InitialiseGameMessageHandlers();
        }

        private static void InitialiseProtocolMessages()
        {
            var messageFactories = new Dictionary<ProtocolMessageOpcode, MessageFactoryDelegate>();
            var messageOpcodes   = new Dictionary<Type, ProtocolMessageOpcode>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()
                .Concat(Assembly.GetEntryAssembly().GetTypes()))
            {
                ProtocolMessageAttribute attribute = type.GetCustomAttribute<ProtocolMessageAttribute>();
                if (attribute == null)
                    continue;

                if (typeof(IReadable).IsAssignableFrom(type))
                {
                    NewExpression @new = Expression.New(type.GetConstructor(Type.EmptyTypes));
                    messageFactories.Add(attribute.Opcode, Expression.Lambda<MessageFactoryDelegate>(@new).Compile());
                }
                messageOpcodes.Add(type, attribute.Opcode);
            }

            protocolMessagefactories = messageFactories.ToImmutableDictionary();
            protocolMessageOpcodes   = messageOpcodes.ToImmutableDictionary();
            log.Info($"Initialised {protocolMessagefactories.Count} protocol message {(protocolMessagefactories.Count == 1 ? "factory" : "factories")}.");
            log.Info($"Initialised {protocolMessageOpcodes.Count} protocol message(s).");
        }

        private static void InitialiseProtocolMessageHandlers()
        {
            var messageHandlers = new Dictionary<ProtocolMessageOpcode, MessageHandlerDelegate>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()
                .Concat(Assembly.GetEntryAssembly().GetTypes()))
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.DeclaringType != type)
                        continue;

                    ProtocolMessageHandlerAttribute attribute = method.GetCustomAttribute<ProtocolMessageHandlerAttribute>();
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

                        Expression<MessageHandlerDelegate> lambda =
                            Expression.Lambda<MessageHandlerDelegate>(call, sessionParameter, messageParameter);

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

                        Expression<MessageHandlerDelegate> lambda =
                            Expression.Lambda<MessageHandlerDelegate>(call, sessionParameter, messageParameter);

                        messageHandlers.Add(attribute.Opcode, lambda.Compile());
                    }
                }
            }

            protocolMessageHandlers = messageHandlers.ToImmutableDictionary();
            log.Info($"Initialised {protocolMessageHandlers.Count} protocol message handler(s).");
        }

        private static void InitialiseGameMessages()
        {
            var messageFactories = new Dictionary<GameMessageOpcode, MessageFactoryDelegate>();
            var messageOpcodes = new Dictionary<Type, GameMessageOpcode>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()
                .Concat(Assembly.GetEntryAssembly().GetTypes()))
            {
                GameMessageAttribute attribute = type.GetCustomAttribute<GameMessageAttribute>();
                if (attribute == null)
                    continue;

                if ((attribute.Direction & MessageDirection.Client) != 0)
                {
                    NewExpression @new = Expression.New(type.GetConstructor(Type.EmptyTypes));
                    messageFactories.Add(attribute.Opcode, Expression.Lambda<MessageFactoryDelegate>(@new).Compile());
                }
                if ((attribute.Direction & MessageDirection.Server) != 0)
                    messageOpcodes.Add(type, attribute.Opcode);
            }

            clientMessageFactories = messageFactories.ToImmutableDictionary();
            serverMessageOpcodes = messageOpcodes.ToImmutableDictionary();
            log.Info($"Initialised {clientMessageFactories.Count} game message {(clientMessageFactories.Count == 1 ? "factory" : "factories")}.");
            log.Info($"Initialised {serverMessageOpcodes.Count} game message(s).");
        }

        private static void InitialiseGameMessageHandlers()
        {
            var messageHandlers = new Dictionary<GameMessageOpcode, MessageHandlerDelegate>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()
                .Concat(Assembly.GetEntryAssembly().GetTypes()))
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.DeclaringType != type)
                        continue;

                    GameMessageHandlerAttribute attribute = method.GetCustomAttribute<GameMessageHandlerAttribute>();
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

                        Expression<MessageHandlerDelegate> lambda =
                            Expression.Lambda<MessageHandlerDelegate>(call, sessionParameter, messageParameter);

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

                        Expression<MessageHandlerDelegate> lambda =
                            Expression.Lambda<MessageHandlerDelegate>(call, sessionParameter, messageParameter);

                        messageHandlers.Add(attribute.Opcode, lambda.Compile());
                    }
                }
            }

            clientMessageHandlers = messageHandlers.ToImmutableDictionary();
            log.Info($"Initialised {clientMessageHandlers.Count} game message handler(s).");
        }

        public static bool GetOpcode(IWritable message, out ProtocolMessageOpcode opcode)
        {
            return protocolMessageOpcodes.TryGetValue(message.GetType(), out opcode);
        }

        public static IReadable GetProtocolMessage(ProtocolMessageOpcode opcode)
        {
            return protocolMessagefactories.TryGetValue(opcode, out MessageFactoryDelegate factory)
                ? factory.Invoke() : null;
        }

        public static MessageHandlerDelegate GetProtocolMessageHandler(ProtocolMessageOpcode opcode)
        {
            return protocolMessageHandlers.TryGetValue(opcode, out MessageHandlerDelegate handler)
                ? handler : null;
        }

        public static bool GetOpcode(IWritable message, out GameMessageOpcode opcode)
        {
            return serverMessageOpcodes.TryGetValue(message.GetType(), out opcode);
        }

        public static IReadable GetGameMessage(GameMessageOpcode opcode)
        {
            return clientMessageFactories.TryGetValue(opcode, out MessageFactoryDelegate factory)
                ? factory.Invoke() : null;
        }

        public static MessageHandlerDelegate GetGameMessageHandler(GameMessageOpcode opcode)
        {
            return clientMessageHandlers.TryGetValue(opcode, out MessageHandlerDelegate handler)
                ? handler : null;
        }
    }
}
