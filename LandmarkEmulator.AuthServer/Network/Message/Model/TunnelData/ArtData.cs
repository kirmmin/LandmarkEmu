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
                throw new NotImplementedException();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.WriteLE(Unknown0);
                writer.WriteLE(Unknown1);
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
                throw new NotImplementedException();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.WriteLE(Unknown0);
                writer.WriteLE(Unknown1);
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
                    throw new NotImplementedException();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.WriteLE(Unknown0);
                    writer.WriteLE(Unknown1);
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
                throw new NotImplementedException();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.WriteLE(Unknown0);
                writer.WriteLE(Unknown1);
                writer.WriteLE(Unknown2);

                writer.WriteLE((uint)Unknown3.Count);
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
                throw new NotImplementedException();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.WriteLE(Unknown0);
                writer.WriteLE(Unknown1);
                writer.WriteLE(Unknown2);
                writer.WriteLE(Unknown3);
                writer.WriteLE(Unknown4);
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
                throw new NotImplementedException();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.WriteLE(Unknown0);
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
                    throw new NotImplementedException();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.WriteLE(SemanticName);
                    writer.WriteLE(Unknown1);
                    writer.WriteLE(Unknown2);
                    writer.WriteLE(Unknown3);
                    writer.WriteLE(EditType);
                    writer.WriteLE(Unknown4);
                    writer.WriteLE(R);
                    writer.WriteLE(G);
                    writer.WriteLE(B);
                    writer.WriteLE(A);
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
                throw new NotImplementedException();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.WriteLE(Id);
                writer.WriteLE(AliasName);
                writer.WriteLE(SemanticGroup);
                writer.WriteLE((uint)ArtTints.Count);
                ArtTints.ForEach(x => x.Write(writer));
                writer.WriteLE(Unknown3);
                writer.WriteLE(Unknown4);
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
            throw new NotImplementedException();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteLE((uint)Unknown0.Count);
            Unknown0.ForEach(x => x.Write(writer));

            writer.WriteLE((uint)Unknown1.Count);
            Unknown1.ForEach(x => x.Write(writer));

            writer.WriteLE((uint)ArtTintOverrideGroups.Count);
            ArtTintOverrideGroups.ForEach(x => x.Write(writer));

            writer.WriteLE((uint)Unknown3.Count);
            Unknown3.ForEach(x => x.Write(writer));

            writer.WriteLE((uint)Unknown4.Count);
            Unknown4.ForEach(x => x.Write(writer));

            writer.WriteLE((uint)Unknown5.Count);
            Unknown5.ForEach(x => x.Write(writer));

            writer.WriteLE((uint)ArtTintGroups.Count);
            ArtTintGroups.ForEach(x => x.Write(writer));

            writer.WriteLE((uint)Unknown7.Count);
            Unknown7.ForEach(x => x.Write(writer));
        }
    }
}
