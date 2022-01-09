using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model.Shared
{
    public class Server : IWritable
    {
        public ulong ServerId { get; set; }
        public bool IsLocked { get; set; }
        public string Name { get; set; } = "";
        public uint NameId { get; set; }
        public string Description { get; set; } = "";
        public uint DescriptionId { get; set; }
        public uint ReqFeatureId { get; set; }
        public string ServerInfo { get; set; } = "";
        public uint PopulationLevel { get; set; }
        public string PopulationData { get; set; } = "";
        public string AccessExpression { get; set; } = "";
        public bool AllowedAccess { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteLE(ServerId);
            writer.Write(IsLocked);
            writer.WriteLE(Name);
            writer.WriteLE(NameId);
            writer.WriteLE(Description);
            writer.WriteLE(DescriptionId);
            writer.WriteLE(ReqFeatureId);
            writer.WriteLE(ServerInfo);
            writer.WriteLE(PopulationLevel);
            writer.WriteLE(PopulationData);
            writer.WriteLE(AccessExpression);
            writer.Write(AllowedAccess);
        }
    }
}
