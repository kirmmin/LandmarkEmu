using LandmarkEmulator.Shared.Network.Packets;
using NLog;
using System;
using System.Collections.Generic;

namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.MutliPacket, useEncryption: true)]
    public class MultiPacket : IProtocol
    {
        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public List<ProtocolPacket> Packets { get; } = new();

        public void Read(ProtocolPacketReader reader, PacketOptions options)
        {
            byte[] data = reader.GetRemainingData();

            if (options.Compression)
                reader.ReadByte();

            while (reader.BytesRemaining > 2)
            {
                try
                {
                    ReadPacket(reader, options);
                }
                catch (Exception ex)
                {
                    log.Error($"{BitConverter.ToString(data)} : {BitConverter.ToString(reader.GetRemainingData())}");
                }
            }
        }

        private void ReadPacket(ProtocolPacketReader reader, PacketOptions options)
        {
            byte nextLength = reader.ReadByte(); // TODO: Calculate length of packet appropriately.
            if (nextLength > reader.BytesRemaining - 2)
                nextLength = (byte)(reader.BytesRemaining - 2);

            Packets.Add(new ProtocolPacket(reader.ReadBytes(nextLength), 
                new PacketOptions 
                { 
                    IsSubpacket = true, 
                    Compression = options.Compression 
                }));
        }

        public void Write(ProtocolPacketWriter writer, PacketOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}
