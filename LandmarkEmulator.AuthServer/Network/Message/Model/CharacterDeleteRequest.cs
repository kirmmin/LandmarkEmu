﻿using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterDeleteRequest, MessageDirection.Client)]
    public class CharacterDeleteRequest : IReadable
    {
        public ulong CharacterId { get; set; }

        public void Read(GamePacketReader reader)
        {
            CharacterId = reader.ReadULongLE();
        }
    }
}
