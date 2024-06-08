using System.Collections.Generic;

namespace LandmarkEmulator.Shared.Network.Message.Model.Shared
{
    public class CharacterModelInfo : IWritable, IReadable, ISize
    {
        public List<(uint, uint, uint)> Customisations { get; set; } = new();
        public uint SkinId { get; set; }
        public string Unknown1 { get; set; } = "";
        public uint Race { get; set; } = 1;
        public uint Gender { get; set; } = 1;

        public uint GetSize()
        {
            uint totalSize = 0u;

            totalSize += 4u; // Vector3s Count
            Customisations.ForEach(x => totalSize += 12u); // 3x Float

            return (uint)(totalSize + 4u + 4u + Unknown1.Length + 4u + 4u);
        }

        public void Read(GamePacketReader reader)
        {
            var customisationsCount = reader.ReadUInt();
            for (int i = 0; i < customisationsCount; i++)
            {
                var slot = reader.ReadUInt();
                var option = reader.ReadUInt();
                var tint = reader.ReadUInt();
                Customisations.Add(new(slot, option, tint));
            }
            SkinId = reader.ReadUInt();
            Unknown1 = reader.ReadString();
            Race = reader.ReadUInt();
            Gender = reader.ReadUInt();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write((uint)Customisations.Count);
            foreach (var v in Customisations)
            {
                writer.Write(v.Item1);
                writer.Write(v.Item2);
                writer.Write(v.Item3);
            }
            writer.Write(SkinId);
            writer.Write(Unknown1);
            writer.Write(Race);
            writer.Write(Gender);
        }
    }
}
