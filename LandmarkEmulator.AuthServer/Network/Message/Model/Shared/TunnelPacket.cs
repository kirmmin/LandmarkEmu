using LandmarkEmulator.AuthServer.Network.Message.Static;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model.Shared
{
    public class TunnelPacket : IReadable, IWritable
    {
        public ulong ServerId { get; set; }
        public TunnelDataType Type { get; set; }
        public ITunnelData Data { get; set; }

        public void Read(GamePacketReader reader)
        {
            ServerId = reader.ReadULongLE();
            reader.ReadUIntLE(); // Size of TunnelData, but we don't need to use it. Just read, don't store.
            Type = (TunnelDataType)reader.ReadByte();

            Data = TunnelDataManager.Instance.NewEntityCommand(Type);
            if (Data == null)
                return;

            Data.Read(reader);
        }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteLE(ServerId);
            writer.WriteLE(Data.GetSize() + 1);
            writer.Write((byte)Type);
            Data.Write(writer);
        }
    }
}
