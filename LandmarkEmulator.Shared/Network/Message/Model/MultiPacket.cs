using LandmarkEmulator.Shared.Network.Packets;
using System.Collections.Generic;

namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.MutliPacket, useEncryption: true)]
    public class MultiPacket : IProtocol
    {
        public List<ProtocolPacket> Packets { get; } = new();

        public void Read(GamePacketReader reader, PacketOptions options)
        {
            if (options.Compression)
                reader.ReadByte();

            while (reader.BytesRemaining > 2)
            {
                ReadPacket(reader, options);
            }
        }

        private void ReadPacket(GamePacketReader reader, PacketOptions options)
        {
            byte nextLength = reader.ReadByte(); // TODO: Calculate length of packet appropriately.
            Packets.Add(new ProtocolPacket(reader.ReadBytes(nextLength), 
                new PacketOptions 
                { 
                    IsSubpacket = true, 
                    Compression = options.Compression 
                }));
        }

        public void Write(GamePacketWriter writer, PacketOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}
