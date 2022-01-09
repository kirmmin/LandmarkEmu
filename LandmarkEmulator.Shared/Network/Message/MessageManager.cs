using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LandmarkEmulator.Shared.Network.Message
{
    public delegate void MessageHandlerDelegate(NetworkSession session, IProtocol message);

    public class MessageManager : Singleton<MessageManager>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private delegate IProtocol MessageFactoryDelegate();

        private ImmutableDictionary<ProtocolMessageOpcode, MessageFactoryDelegate> protocolMessagefactories;
        private ImmutableDictionary<ProtocolMessageOpcode, MessageHandlerDelegate> protocolMessageHandlers;
        private ImmutableDictionary<Type, (ProtocolMessageOpcode, bool)> protocolMessageOpcodes;

        public void Initialise()
        {
            InitialiseProtocolMessages();
            InitialiseProtocolMessageHandlers();
        }

        private void InitialiseProtocolMessages()
        {
            var messageFactories = new Dictionary<ProtocolMessageOpcode, MessageFactoryDelegate>();
            var messageOpcodes   = new Dictionary<Type, (ProtocolMessageOpcode, bool)>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()
                .Concat(Assembly.GetEntryAssembly().GetTypes()))
            {
                ProtocolMessageAttribute attribute = type.GetCustomAttribute<ProtocolMessageAttribute>();
                if (attribute == null)
                    continue;

                if (typeof(IProtocol).IsAssignableFrom(type))
                {
                    NewExpression @new = Expression.New(type.GetConstructor(Type.EmptyTypes));
                    messageFactories.Add(attribute.Opcode, Expression.Lambda<MessageFactoryDelegate>(@new).Compile());
                }
                messageOpcodes.Add(type, (attribute.Opcode, attribute.UseEncryption));
            }

            protocolMessagefactories = messageFactories.ToImmutableDictionary();
            protocolMessageOpcodes   = messageOpcodes.ToImmutableDictionary();
            log.Info($"Initialised {protocolMessagefactories.Count} protocol message {(protocolMessagefactories.Count == 1 ? "factory" : "factories")}.");
            log.Info($"Initialised {protocolMessageOpcodes.Count} protocol message(s).");
        }

        private void InitialiseProtocolMessageHandlers()
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
                    ParameterExpression messageParameter = Expression.Parameter(typeof(IProtocol));

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
                        Debug.Assert(typeof(IProtocol).IsAssignableFrom(parameterInfo[0].ParameterType));
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

        public bool GetOpcodeData(IProtocol message, out (ProtocolMessageOpcode, bool) opcode)
        {
            return protocolMessageOpcodes.TryGetValue(message.GetType(), out opcode);
        }

        public IProtocol GetProtocolMessage(ProtocolMessageOpcode opcode)
        {
            return protocolMessagefactories.TryGetValue(opcode, out MessageFactoryDelegate factory)
                ? factory.Invoke() : null;
        }

        public MessageHandlerDelegate GetProtocolMessageHandler(ProtocolMessageOpcode opcode)
        {
            return protocolMessageHandlers.TryGetValue(opcode, out MessageHandlerDelegate handler)
                ? handler : null;
        }
    }
}
