using LandmarkEmulator.AuthServer.Network.Message.Static;
using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

namespace LandmarkEmulator.AuthServer.Network.Message
{
    public static class TunnelDataManager
    {
        private delegate ITunnelData TunnelDataFactoryDelegate();
        private static ImmutableDictionary<TunnelDataType, TunnelDataFactoryDelegate> tunnelDataFactories;
        private static ImmutableDictionary<Type, TunnelDataType> tunnelDataTypes;

        public static void Initialise()
        {
            var factoryBuilder = ImmutableDictionary.CreateBuilder<TunnelDataType, TunnelDataFactoryDelegate>();
            var commandBuilder = ImmutableDictionary.CreateBuilder<Type, TunnelDataType>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                TunnelDataAttribute attribute = type.GetCustomAttribute<TunnelDataAttribute>();
                if (attribute == null)
                    continue;

                NewExpression @new = Expression.New(type.GetConstructor(Type.EmptyTypes));
                factoryBuilder.Add(attribute.Type, Expression.Lambda<TunnelDataFactoryDelegate>(@new).Compile());
                commandBuilder.Add(type, attribute.Type);
            }

            tunnelDataFactories = factoryBuilder.ToImmutable();
            tunnelDataTypes = commandBuilder.ToImmutable();
        }

        /// <summary>
        /// Return a new <see cref="IEntityCommandModel"/> of supplied <see cref="EntityCommand"/>.
        /// </summary>
        public static ITunnelData NewEntityCommand(TunnelDataType type)
        {
            return tunnelDataFactories.TryGetValue(type, out TunnelDataFactoryDelegate factory) ? factory.Invoke() : null;
        }

        /// <summary>
        /// Returns the <see cref="EntityCommand"/> for supplied <see cref="Type"/>.
        /// </summary>
        public static TunnelDataType? GetType(Type type)
        {
            if (tunnelDataTypes.TryGetValue(type, out TunnelDataType tunnelDataType))
                return tunnelDataType;

            return null;
        }
    }
}
