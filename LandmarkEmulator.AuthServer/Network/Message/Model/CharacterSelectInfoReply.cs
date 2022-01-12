using LandmarkEmulator.Shared.Game.Entity.Static;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterSelectInfoReply, ProtocolVersion.LoginUdp_10)]
    public class CharacterSelectInfoReply : IWritable
    {
        public class Character : IWritable
        {
            public class CharacterPayload : IWritable, ISize
            {
                public class UnknownStruct_142C18710 : IWritable, ISize
                {
                    public List<Vector3> Vector3s { get; set; } = new();
                    public uint Unknown0 { get; set; } = 2;
                    public string Unknown1 { get; set; } = "2.adr";
                    public uint Unknown2 { get; set; } = 2;
                    public uint Unknown3 { get; set; } = 2;

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

                public class CharacterAttachment : IWritable, ISize
                {
                    public string ModelName { get; set; } = "";
                    public string TextureAlias { get; set; } = "";
                    public string Unknown2 { get; set; } = "";  // TintAlias?
                    public string Unknown3 { get; set; } = "";  // DecalAlias?
                    public uint Unknown4 { get; set; }          // TintId?
                    public uint Unknown5 { get; set; }          // CompositeId?
                    public AttachmentSlot Slot { get; set; }    // "ActorUsage"
                    public uint Unknown7 { get; set; }
                    public bool Unknown8 { get; set; }

                    public uint GetSize()
                    {
                        return (uint)(4u + ModelName.Length + 4u + TextureAlias.Length + 4u + Unknown2.Length + 4u + Unknown3.Length + 4u + 4u + 4u + 4u + 1u);
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        writer.WriteLE(ModelName);
                        writer.WriteLE(TextureAlias);
                        writer.WriteLE(Unknown2);
                        writer.WriteLE(Unknown3);
                        writer.WriteLE(Unknown4);
                        writer.WriteLE(Unknown5);
                        writer.WriteLE((uint)Slot);
                        writer.WriteLE(Unknown7);
                        writer.Write(Unknown8);
                    }
                }

                public class Friend : IWritable, ISize
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
                public uint Unknown1 { get; set; } = 100u;
                public uint Unknown2 { get; set; } = 1u;
                public uint Unknown3 { get; set; } = 393u;
                public uint Unknown4 { get; set; } = 2u;
                public uint Unknown5 { get; set; } = 35u;
                public uint Unknown6 { get; set; }
                public uint Unknown7 { get; set; }
                public ulong Unknown8 { get; set; }
                public List<uint> Array142C34C30 { get; set; } = new();
                public UnknownStruct_142C18710 Unknown9 { get; set; } = new();
                public List<CharacterAttachment> CharacterAttachments { get; set; } = new();
                public List<Friend> Friends { get; set; } = new();
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

                    writer.WriteLE((uint)CharacterAttachments.Count);
                    CharacterAttachments.ForEach(x => x.Write(writer));

                    writer.WriteLE((uint)Friends.Count);
                    Friends.ForEach(x => x.Write(writer));

                    writer.WriteLE(UnknownQ);
                }

                public uint GetSize()
                {
                    uint totalSize = 0u;

                    totalSize += 4u; // Array142C34C30 Count
                    Array142C34C30.ForEach(x => totalSize += 4);

                    totalSize += Unknown9.GetSize();

                    totalSize += 4u; // Array1430AE5D0s Count
                    CharacterAttachments.ForEach(x => totalSize += x.GetSize());

                    totalSize += 4u; // Array1430AE3A0s Count
                    Friends.ForEach(x => totalSize += x.GetSize());

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

    [AuthMessage(AuthMessageOpcode.CharacterSelectInfoReply, ProtocolVersion.LoginUdp_9)]
    public class CharacterSelectInfoReply9 : IWritable
    {
        public class Character : IWritable
        {
            public class CharacterPayload : IWritable, ISize
            {
                public class CharacterAttachment : IWritable, ISize
                {
                    public string ModelName { get; set; } = "";
                    public string TextureAlias { get; set; } = "";
                    public string Unknown2 { get; set; } = "";  // TintAlias?
                    public string Unknown3 { get; set; } = "";  // DecalAlias?
                    public uint Unknown4 { get; set; }          // TintId?
                    public uint Unknown5 { get; set; }          // CompositeId?
                    public AttachmentSlot Slot { get; set; }    // "ActorUsage"
                    public uint Unknown7 { get; set; }

                    public uint GetSize()
                    {
                        return (uint)(4u + ModelName.Length + 4u + TextureAlias.Length + 4u + Unknown2.Length + 4u + Unknown3.Length + 4u + 4u + 4u + 4u);
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        writer.WriteLE(ModelName);
                        writer.WriteLE(TextureAlias);
                        writer.WriteLE(Unknown2);
                        writer.WriteLE(Unknown3);
                        writer.WriteLE(Unknown4);
                        writer.WriteLE(Unknown5);
                        writer.WriteLE((uint)Slot);
                        writer.WriteLE(Unknown7);
                    }
                }

                public class Friend : IWritable, ISize
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
                public byte Unknown0 { get; set; }
                public uint Unknown1 { get; set; }
                public uint HeadId { get; set; }
                public uint ModelId { get; set; }
                public uint Gender { get; set; }
                public uint Unknown5 { get; set; }
                public uint Unknown6 { get; set; }
                public ulong Unknown7 { get; set; }
                public List<uint> Array142C34C30 { get; set; } = new();
                public List<(uint, uint, uint)> Customizations { get; set; } = new();
                public uint Unknown8 { get; set; }
                public List<CharacterAttachment> CharacterAttachments { get; set; } = new();
                public List<Friend> Friends { get; set; } = new();
                public ulong Unknown9 { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.WriteLE(GetSize());
                    writer.WriteLE(Name);
                    writer.Write(Unknown0);
                    writer.WriteLE(Unknown1);
                    writer.WriteLE(HeadId);
                    writer.WriteLE(ModelId);
                    writer.WriteLE(Gender);
                    writer.WriteLE(Unknown5);
                    writer.WriteLE(Unknown6);
                    writer.WriteLE(Unknown7);

                    writer.WriteLE((uint)Array142C34C30.Count);
                    Array142C34C30.ForEach(x => writer.WriteLE(x));

                    writer.WriteLE((uint)Customizations.Count);
                    foreach (var v in Customizations)
                    {
                        writer.WriteLE(v.Item1);
                        writer.WriteLE(v.Item2);
                        writer.WriteLE(v.Item3);
                    }

                    writer.WriteLE(Unknown8);

                    writer.WriteLE((uint)CharacterAttachments.Count);
                    CharacterAttachments.ForEach(x => x.Write(writer));

                    writer.WriteLE((uint)Friends.Count);
                    Friends.ForEach(x => x.Write(writer));

                    writer.WriteLE(Unknown9);
                }

                public uint GetSize()
                {
                    uint totalSize = 0u;

                    totalSize += 4u; // Array142C34C30 Count
                    Array142C34C30.ForEach(x => totalSize += 4);

                    totalSize += 4u; // Vector3s Count
                    Customizations.ForEach(x => totalSize += 12u); // 3x Float

                    totalSize += 4u; // CharacterAttachments Count
                    CharacterAttachments.ForEach(x => totalSize += x.GetSize());

                    totalSize += 4u; // Friends Count
                    Friends.ForEach(x => totalSize += x.GetSize());

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
                        8u +
                        4u +
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
