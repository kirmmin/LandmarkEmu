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
            public ulong Unknown1 { get; set; }

            public uint GetSize()
            {
                return 4u /*uint*/ + 8u /*ulong*/;
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

        public class UnknownStruct_142C355A0 : ITunnelData
        {
            public class UnknownStruct_142C38770 : ITunnelData
            {
                public uint Unknown0 { get; set; }
                public long Unknown1 { get; set; }

                public uint GetSize()
                {
                    return 4u /*uint*/ + 8u /*long*/;
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
            public List<UnknownStruct_142C38770> Unknown3 { get; set; } = new();

            public uint GetSize()
            {
                uint totalSize = 0;
                Unknown3.ForEach(x => totalSize += x.GetSize());
                return 4u + 4u + 4u + 4u + totalSize;
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

        public class UnknownStruct_142C37F00 : ITunnelData
        {
            public int Unknown0 { get; set; }
            public string Unknown1 { get; set; }
            public string Unknown2 { get; set; }
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }

            public uint GetSize()
            {
                return (uint)(4u + 4u + Unknown1.Length + 4u + Unknown2.Length + 4u + 4u);
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

        public List<UnknownStruct_142C36820> Unknown0 { get; set; } = new();
        public List<UnknownStruct_142C351D0> Unknown1 { get; set; } = new();
        public List<UnknownStruct_142C355A0> Unknown2 { get; set; } = new();
        public List<UnknownStruct_142C3FE00> Unknown3 { get; set; } = new();
        public List<UnknownStruct_142C3BAC0> Unknown4 { get; set; } = new();
        public List<UnknownStruct_142C3BAC0> Unknown5 { get; set; } = new();
        public List<UnknownStruct_142C37F00> Unknown6 { get; set; } = new();
        public List<UnknownStruct_142C351D0> Unknown7 { get; set; } = new();

        public uint GetSize()
        {
            uint totalSize = 0;

            totalSize += 4u; /*unknown0 arrayCount*/
            Unknown0.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown1 arrayCount*/
            Unknown1.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown2 arrayCount*/
            Unknown2.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown3 arrayCount*/
            Unknown3.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown4 arrayCount*/
            Unknown4.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown5 arrayCount*/
            Unknown5.ForEach(x => totalSize += x.GetSize());

            totalSize += 4u; /*unknown5 arrayCount*/
            Unknown6.ForEach(x => totalSize += x.GetSize());

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
            writer.Write((uint)Unknown0.Count);
            Unknown0.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown1.Count);
            Unknown1.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown2.Count);
            Unknown2.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown3.Count);
            Unknown3.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown4.Count);
            Unknown4.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown5.Count);
            Unknown5.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown6.Count);
            Unknown6.ForEach(x => x.Write(writer));

            writer.Write((uint)Unknown7.Count);
            Unknown7.ForEach(x => x.Write(writer));
        }
    }
}
