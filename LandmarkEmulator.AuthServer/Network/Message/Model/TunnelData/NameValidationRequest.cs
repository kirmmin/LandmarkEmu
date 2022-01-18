using LandmarkEmulator.AuthServer.Network.Message.Static;
using LandmarkEmulator.Shared.Network;

namespace LandmarkEmulator.AuthServer.Network.Message.Model.TunnelData
{
    [TunnelData(TunnelDataType.NameValidationRequest)]
    public class NameValidationRequest : ITunnelData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public uint GetSize()
        {
            return (uint)(4u + FirstName.Length + 4u + LastName.Length);
        }

        public void Read(GamePacketReader reader)
        {
            FirstName = reader.ReadString();
            LastName  = reader.ReadString();
        }

        public void Write(GamePacketWriter writer)
        {
        }
    }
}
