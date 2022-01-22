using LandmarkEmulator.AuthServer.Network.Message.Static;
using LandmarkEmulator.Shared.Network;
using System;
using System.Collections.Generic;

namespace LandmarkEmulator.AuthServer.Network.Message.Model.TunnelData
{
    [TunnelData(TunnelDataType.ArtData)]
    public class ArtData : ITunnelData
    {
        public class UnknownStruct_142C36820 : ITunnelData
        {
            public uint Unknown0 { get; set; }
            public string Unknown1 { get; set; }

            public uint GetSize()
            {
                return (uint)(4u /*uint*/ + 4u /*stringHeader*/ + Unknown1.Length);
            }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
                Unknown1 = reader.ReadString();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);
            }
        }

        public class UnknownStruct_142C351D0 : ITunnelData
        {
            public uint Unknown0 { get; set; } 
            public uint Unknown1 { get; set; }

            public uint GetSize()
            {
                return 4u /*uint*/ + 4u /*ulong*/;
            }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
                Unknown1 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);
            }
        }

        public class ArtTintOverrideGroupEntry : ITunnelData
        {
            public class ArtTintOverrideEntry : ITunnelData
            {
                public uint Unknown0 { get; set; }
                public uint Unknown1 { get; set; }

                public uint GetSize()
                {
                    return 4u /*uint*/ + 4u /*long*/;
                }

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();
                    Unknown1 = reader.ReadUInt();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Unknown0);
                    writer.Write(Unknown1);
                }
            }

            public int Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
            public uint Unknown2 { get; set; }
            public List<ArtTintOverrideEntry> Unknown3 { get; set; } = new();

            public uint GetSize()
            {
                uint totalSize = 4u;
                Unknown3.ForEach(x => totalSize += x.GetSize());
                return 4u + 4u + 4u + totalSize;
            }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = (int)reader.ReadUInt();
                Unknown2 = reader.ReadUInt();
                Unknown2 = reader.ReadUInt();

                var overrideCount = reader.ReadUInt();
                for (int i = 0; i < overrideCount; i++)
                {
                    var ato = new ArtTintOverrideEntry();
                    ato.Read(reader);
                    Unknown3.Add(ato);
                }
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);
                writer.Write(Unknown2);

                writer.Write((uint)Unknown3.Count);
                Unknown3.ForEach(x => x.Write(writer));
            }
        }

        public class UnknownStruct_142C3FE00 : ITunnelData
        {
            public ulong Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
            public uint Unknown2 { get; set; }
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }

            public uint GetSize()
            {
                return (8u + 4u + 4u + 4u + 4u);
            }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadULong();
                Unknown1 = reader.ReadUInt();
                Unknown2 = reader.ReadUInt();
                Unknown3 = reader.ReadUInt();
                Unknown4 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
            }
        }

        public class UnknownStruct_142C3BAC0 : ITunnelData
        {
            public string Unknown0 { get; set; }

            public uint GetSize()
            {
                return (uint)(4u + Unknown0.Length);
            }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadString();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
            }
        }

        public class TintSemanticGroupEntry : ITunnelData
        {
            public class TintSemanticEntry : ITunnelData
            {
                public string SemanticName { get; set; }
                public uint Unknown1 { get; set; } // Id?
                public uint Unknown2 { get; set; } // TypeId
                public uint Unknown3 { get; set; } // 
                public string EditType { get; set; }
                public uint Unknown4 { get; set; }
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public uint GetSize()
                {
                    return (uint)(4u + SemanticName.Length + 4u + 4u + 4u + 4u + EditType.Length + 4u + 4u + 4u + 4u + 4u);
                }

                public void Read(GamePacketReader reader)
                {
                    SemanticName = reader.ReadString();
                    Unknown1 = reader.ReadUInt();
                    Unknown2 = reader.ReadUInt();
                    Unknown3 = reader.ReadUInt();
                    EditType = reader.ReadString();
                    Unknown4 = reader.ReadUInt();
                    R = reader.ReadSingle();
                    G = reader.ReadSingle();
                    B = reader.ReadSingle();
                    A = reader.ReadSingle();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(SemanticName);
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                    writer.Write(Unknown3);
                    writer.Write(EditType);
                    writer.Write(Unknown4);
                    writer.Write(R);
                    writer.Write(G);
                    writer.Write(B);
                    writer.Write(A);
                }
            }

            public int Id { get; set; }
            public string AliasName { get; set; }
            public string SemanticGroup { get; set; }
            public List<TintSemanticEntry> ArtTints { get; set; } = new();
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }

            public uint GetSize()
            {
                uint artTintSize = 4u;
                ArtTints.ForEach(x => artTintSize += x.GetSize());
                return (uint)(4u + 4u + AliasName.Length + 4u + SemanticGroup.Length + 4u + 4u + artTintSize);
            }

            public void Read(GamePacketReader reader)
            {
                Id = (int)reader.ReadUInt();
                AliasName = reader.ReadString();
                SemanticGroup = reader.ReadString();

                var tintCount = reader.ReadUInt();
                for (int i = 0; i < tintCount; i++)
                {
                    var tint = new TintSemanticEntry();
                    tint.Read(reader);
                    ArtTints.Add(tint);
                }


                Unknown3 = reader.ReadUInt();
                Unknown4 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Id);
                writer.Write(AliasName);
                writer.Write(SemanticGroup);
                writer.Write((uint)ArtTints.Count);
                ArtTints.ForEach(x => x.Write(writer));
                writer.Write(Unknown3);
                writer.Write(Unknown4);
            }
        }

        public List<UnknownStruct_142C36820> Unknown0 { get; set; } = new();
        public List<UnknownStruct_142C351D0> Unknown1 { get; set; } = new();
        public List<ArtTintOverrideGroupEntry> ArtTintOverrideGroups { get; set; } = new();
        public List<UnknownStruct_142C3FE00> Unknown3 { get; set; } = new();
        public List<UnknownStruct_142C3BAC0> Unknown4 { get; set; } = new();
        public List<UnknownStruct_142C3BAC0> Unknown5 { get; set; } = new();
        public List<TintSemanticGroupEntry> ArtTintGroups { get; set; } = new();
        public List<UnknownStruct_142C351D0> Unknown7 { get; set; } = new();

        public uint GetSize()
        {
            uint totalSize = 0;

            totalSize += 4u; /*unknown0 arrayCount*/
            Unknown0.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown1 arrayCount*/
            Unknown1.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown2 arrayCount*/
            ArtTintOverrideGroups.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown3 arrayCount*/
            Unknown3.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown4 arrayCount*/
            Unknown4.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown5 arrayCount*/
            Unknown5.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown5 arrayCount*/
            ArtTintGroups.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown7 arrayCount*/
            Unknown7.ForEach(x => totalSize += x.GetSize());

            return totalSize;
        }

        public void Read(GamePacketReader reader)
        {
            var unk0Count = reader.ReadUInt();
            for (int i = 0; i < unk0Count; i++)
            {
                var unk0 = new UnknownStruct_142C36820();
                unk0.Read(reader);
                Unknown0.Add(unk0);
            }

            var unk1Count = reader.ReadUInt();
            for (int i = 0; i < unk1Count; i++)
            {
                var unk1 = new UnknownStruct_142C351D0();
                unk1.Read(reader);
                Unknown1.Add(unk1);
            }

            var artTintORCount = reader.ReadUInt();
            for (int i = 0; i < artTintORCount; i++)
            {
                var atog = new ArtTintOverrideGroupEntry();
                atog.Read(reader);
                ArtTintOverrideGroups.Add(atog);
            }

            var unk3Count = reader.ReadUInt();
            for (int i = 0; i < unk3Count; i++)
            {
                var unk3 = new UnknownStruct_142C3FE00();
                unk3.Read(reader);
                Unknown3.Add(unk3);
            }

            var unk4Count = reader.ReadUInt();
            for (int i = 0; i < unk4Count; i++)
            {
                var unk4 = new UnknownStruct_142C3BAC0();
                unk4.Read(reader);
                Unknown4.Add(unk4);
            }

            var unk5Count = reader.ReadUInt();
            for (int i = 0; i < unk5Count; i++)
            {
                var unk5 = new UnknownStruct_142C3BAC0();
                unk5.Read(reader);
                Unknown5.Add(unk5);
            }

            var artTintGroupCount = reader.ReadUInt();
            for (int i = 0; i < artTintGroupCount; i++)
            {
                var tintG = new TintSemanticGroupEntry();
                tintG.Read(reader);
                ArtTintGroups.Add(tintG);
            }

            var unk7Count = reader.ReadUInt();
            for (int i = 0; i < unk7Count; i++)
            {
                var unk7 = new UnknownStruct_142C351D0();
                unk7.Read(reader);
                Unknown7.Add(unk7);
            }
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write((uint)Unknown0.Count);
            Unknown0.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown1.Count);
            Unknown1.ForEach(x => x.Write(writer));

            writer.Write((uint)ArtTintOverrideGroups.Count);
            ArtTintOverrideGroups.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown3.Count);
            Unknown3.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown4.Count);
            Unknown4.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown5.Count);
            Unknown5.ForEach(x => x.Write(writer));

            writer.Write((uint)ArtTintGroups.Count);
            ArtTintGroups.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown7.Count);
            Unknown7.ForEach(x => x.Write(writer));
        }
    }
}
