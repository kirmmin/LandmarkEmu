using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.GameTimeSync)]
    public class GameTimeSync : IWritable, IReadable
    {
        public double Time { get; set; }
        public float Unknown0 { get; set; } = 12f;
        public bool Unknown1 { get; set; }

        public void Read(GamePacketReader reader)
        {
            Time = reader.ReadDouble();
            Unknown0 = reader.ReadSingle();
            Unknown1 = reader.ReadBool();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Math.Floor(DateTime.Now.Ticks / 1000d));
            writer.Write(Unknown0);
            writer.Write(Unknown1);
        }
    }
}
