using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterSelectInfoReply, MessageDirection.Server)]
    public class CharacterSelectInfoReply : IWritable
    {
        public class Character : IWritable
        {
            public class CharacterPayload : IWritable, ISize
            {
                public class UnknownStruct_142C18710 : IWritable, ISize
                {
                    public List<Vector3> Vector3s { get; set; } = new();
                    public uint Unknown0 { get; set; } = 1u;
                    public string Unknown1 { get; set; } = "Char_Biped_HumanMale_Entities_PCNPC_Adventurer_000_Chest.adr";
                    public uint Unknown2 { get; set; } = 1u;
                    public uint Unknown3 { get; set; } = 1u;

                    public uint GetSize()
                    {
                        uint totalSize = 0u;

                        totalSize += 4u; // Vector3s Count
                        Vector3s.ForEach(x => totalSize += 12u); // 3x Float

                        return (uint)(totalSize + 4u + 4u + Unknown1.Length + 4u + 4u);
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        writer.WriteLE((uint)Vector3s.Count);
                        foreach (var v in Vector3s)
                        {
                            writer.WriteLE(v.X);
                            writer.WriteLE(v.Y);
                            writer.WriteLE(v.Z);
                        }
                        writer.WriteLE(Unknown0);
                        writer.WriteLE(Unknown1);
                        writer.WriteLE(Unknown2);
                        writer.WriteLE(Unknown3);
                    }
                }

                public class UnknownStruct_1430AE5D0 : IWritable, ISize
                {
                    public uint GetSize()
                    {
                        throw new NotImplementedException();
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        throw new NotImplementedException();
                    }
                }

                public class UnknownStruct_1430AE3A0 : IWritable, ISize
                {
                    public uint GetSize()
                    {
                        throw new NotImplementedException();
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        throw new NotImplementedException();
                    }
                }

                public string Name { get; set; } = "Kirmmin";
                public byte Unknown0 { get; set; } = 1;
                public uint Unknown1 { get; set; } = 1u;
                public uint Unknown2 { get; set; } = 1u;
                public uint Unknown3 { get; set; } = 1150u;
                public uint Unknown4 { get; set; } = 1u;
                public uint Unknown5 { get; set; } = 386u;
                public uint Unknown6 { get; set; } = 1u;
                public uint Unknown7 { get; set; } = 1u;
                public ulong Unknown8 { get; set; }
                public List<uint> Array142C34C30 { get; set; } = new();
                public UnknownStruct_142C18710 Unknown9 { get; set; } = new();
                public List<UnknownStruct_1430AE5D0> Array1430AE5D0s { get; set; } = new();
                public List<UnknownStruct_1430AE3A0> Array1430AE3A0s { get; set; } = new();
                public ulong UnknownQ { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.WriteLE(GetSize());
                    writer.WriteLE(Name);
                    writer.Write(Unknown0);
                    writer.WriteLE(Unknown1);
                    writer.WriteLE(Unknown2);
                    writer.WriteLE(Unknown3);
                    writer.WriteLE(Unknown4);
                    writer.WriteLE(Unknown5);
                    writer.WriteLE(Unknown6);
                    writer.WriteLE(Unknown7);
                    writer.WriteLE(Unknown8);

                    writer.WriteLE((uint)Array142C34C30.Count);
                    Array142C34C30.ForEach(x => writer.WriteLE(x));

                    Unknown9.Write(writer);

                    writer.WriteLE((uint)Array1430AE5D0s.Count);
                    Array1430AE5D0s.ForEach(x => x.Write(writer));

                    writer.WriteLE((uint)Array1430AE3A0s.Count);
                    Array1430AE3A0s.ForEach(x => x.Write(writer));

                    writer.WriteLE(UnknownQ);
                }

                public uint GetSize()
                {
                    uint totalSize = 0u;

                    totalSize += 4u; // Array142C34C30 Count
                    Array142C34C30.ForEach(x => totalSize += 4);

                    totalSize += Unknown9.GetSize();

                    totalSize += 4u; // Array1430AE5D0s Count
                    Array1430AE5D0s.ForEach(x => totalSize += x.GetSize());

                    totalSize += 4u; // Array1430AE3A0s Count
                    Array1430AE3A0s.ForEach(x => totalSize += x.GetSize());

                    return (uint)(
                        totalSize +
                        4u + Name.Length +
                        1u + 
                        4u +
                        4u +
                        4u +
                        4u +
                        4u + 
                        4u +
                        4u +
                        8u +
                        8u);
                }
            }

            public ulong CharacterId { get; set; }
            public ulong LastServerId { get; set; }
            public double LastLogin { get; set; }
            public uint Status { get; set; }
            public CharacterPayload CharacterData { get; set; } = new();

            public void Write(GamePacketWriter writer)
            {
                writer.WriteLE(CharacterId);
                writer.WriteLE(LastServerId);
                writer.WriteLE(LastLogin);
                writer.WriteLE(Status);
                CharacterData.Write(writer);
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
            Characters.ForEach(x => x.Write(writer));
        }
    }
}
