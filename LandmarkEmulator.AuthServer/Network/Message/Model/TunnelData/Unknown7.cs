using LandmarkEmulator.AuthServer.Network.Message.Static;
using LandmarkEmulator.Shared.Network;
using System;

namespace LandmarkEmulator.AuthServer.Network.Message.Model.TunnelData
{
    [TunnelData(TunnelDataType.Unknown7)]
    internal class Unknown7 : ITunnelData
    {
        public string String { get; set; }

        public uint GetSize()
        {
            return (uint)(String.Length + 4); // Unknown0 + (uint)StringHeader
        }

        public void Read(GamePacketReader reader)
        {
            String = reader.ReadStringLE();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteLE(String);
        }
    }
}
