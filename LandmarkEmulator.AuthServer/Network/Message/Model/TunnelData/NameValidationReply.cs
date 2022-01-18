using LandmarkEmulator.AuthServer.Network.Message.Static;
using LandmarkEmulator.Shared.Network;

namespace LandmarkEmulator.AuthServer.Network.Message.Model.TunnelData
{
    [TunnelData(TunnelDataType.NameValidationReply)]
    public class NameValidationReply : ITunnelData
    {
        public NameValidationResult Result { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public uint GetSize()
        {
            return (uint)(4u + 4u + FirstName.Length + 4u + LastName.Length);
        }

        public void Read(GamePacketReader reader)
        {
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(FirstName);
            writer.Write(LastName);
            writer.Write((uint)Result);
        }
    }
}
