using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.GameTimeSync)]
    public class GameTimeSync : IWritable, IReadable
    {
        public double Time { get; set; }
        public float CycleSpeed { get; set; }
        public bool Unknown0 { get; set; }

        public void Read(GamePacketReader reader)
        {
            Time = reader.ReadDouble();
            CycleSpeed = reader.ReadSingle();
            Unknown0 = reader.ReadBool();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Time);
            writer.Write(CycleSpeed);
            writer.Write(Unknown0);
        }
    }
}
