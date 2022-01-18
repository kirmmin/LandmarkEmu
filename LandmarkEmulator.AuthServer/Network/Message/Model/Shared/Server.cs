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
            writer.Write(ServerId);
            writer.Write(IsLocked);
            writer.Write(Name);
            writer.Write(NameId);
            writer.Write(Description);
            writer.Write(DescriptionId);
            writer.Write(ReqFeatureId);
            writer.Write(ServerInfo);
            writer.Write(PopulationLevel);
            writer.Write(PopulationData);
            writer.Write(AccessExpression);
            writer.Write(AllowedAccess);
        }
    }
}
