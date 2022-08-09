using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.ReferenceDataProfileDefinitions)]
    public class ReferenceDataProfileDefinitions : IReadable, IWritable
    {
        public class Profile : IReadable, IWritable
        {

            public void Read(GamePacketReader reader)
            {
                
            }

            public void Write(GamePacketWriter writer)
            {
                
            }
        }

        public List<Profile> Profiles { get; set; } = new();

        public void Read(GamePacketReader reader)
        {
            uint profileCount = reader.ReadUInt();

            for (uint i = 0; i < profileCount; i++)
            {
                var profile = new Profile();
                profile.Read(reader);
                Profiles.Add(profile);
            }
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write((uint)Profiles.Count);
        }
    }
}
