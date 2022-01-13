using LandmarkEmulator.AuthServer.Network.Message.Model.Shared;
using LandmarkEmulator.AuthServer.Zone.Static;
using LandmarkEmulator.Database.Auth.Model;
using LandmarkEmulator.Shared.GameTable.Text;
using LandmarkEmulator.Shared.Network.Message;
using System;

namespace LandmarkEmulator.AuthServer.Zone
{
    public class ZoneServer : IBuildable<Server>
    {
        public ulong Id { get; }
        public uint NameId { get; }
        public string ZoneName { get; } = "";
        public string Host { get; }
        public ushort Port { get; }
        public ServerFlags Flags { get; }

        /// <summary>
        /// Creates a new <see cref="ZoneServer"/> from a <see cref="ZoneServerModel"/>.
        /// </summary>
        public ZoneServer(ZoneServerModel model)
        {
            Id       = model.Id;
            NameId   = model.NameId;
            ZoneName = TextManager.Instance.GetTextForId(NameId);
            Host     = model.Host;
            Port     = model.Port;
            Flags    = (ServerFlags)model.Flags;
        }

        /// <summary>
        /// Returns a <see cref="Server"/> instance for use in Network messaging.
        /// </summary>
        public Server Build()
        {
            return new Server
            {
                ServerId = Id,
                AllowedAccess = true, // TODO: Contact ZoneServer to determine if user is allowed access
                IsLocked = false,     // TODO: Contact ZoneServer to determine if it is locked 
                NameId = NameId,
                Name = ZoneName,
                ServerInfo = $"<ServerInfo IsRecommended=\"{Convert.ToByte((Flags & ServerFlags.IsRecommended) != 0)}\"></ServerInfo>" // Region is supported, but weird in-game so leaving it out
            };
        }
    }
}
