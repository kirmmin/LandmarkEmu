using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;
using System.Collections.Generic;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterSelectInfoReply, MessageDirection.Server)]
    public class CharacterSelectInfoReply : IWritable
    {
        public class Character : IWritable
        {
            public void Write(GamePacketWriter writer)
            {
                throw new NotImplementedException();
            }
        }
        
        public uint Status { get; set; }
        public bool CanBypassServerLock { get; set; }
        public List<Character> Characters { get; set; } = new();

        public void Write(GamePacketWriter writer)
        {
            writer.WriteLE(Status);
            writer.Write(CanBypassServerLock);
            writer.WriteLE((uint)Characters.Count);
            Characters.ForEach(i => i.Write(writer));
        }
    }
}
