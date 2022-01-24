using LandmarkEmulator.Shared.Game.Entity.Static;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterSelectInfoReply, ProtocolVersion.LoginUdp_10)]
    public class CharacterSelectInfoReply : IWritable, IReadable
    {
        public class Character : IWritable, IReadable
        {
            public class CharacterPayload : IWritable, IReadable, ISize
            {
                public class ModelInfo : IWritable, IReadable, ISize
                {
                    public List<(uint, uint, uint)> Customisations { get; set; } = new();
                    public uint SkinId { get; set; }
                    public string Unknown1 { get; set; } = "";
                    public uint Unknown2 { get; set; } = 1;
                    public uint Unknown3 { get; set; } = 1;

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
                        Unknown2 = reader.ReadUInt();
                        Unknown3 = reader.ReadUInt();
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
                        writer.Write(Unknown2);
                        writer.Write(Unknown3);
                    }
                }

                public class CharacterAttachment : IWritable, IReadable, ISize
                {
                    public string ModelName { get; set; } = "";
                    public string TextureAlias { get; set; } = "";
                    public string SemanticTint { get; set; } = "";  // TintAlias?
                    public string Unknown3 { get; set; } = "";  // DecalAlias?
                    public uint Unknown4 { get; set; }          // TintId?
                    public uint Unknown5 { get; set; }          // CompositeId?
                    public AttachmentSlot Slot { get; set; }    // "ActorUsage"
                    public uint Unknown7 { get; set; }
                    public bool Unknown8 { get; set; }

                    public uint GetSize()
                    {
                        return (uint)(4u + ModelName.Length + 4u + TextureAlias.Length + 4u + SemanticTint.Length + 4u + Unknown3.Length + 4u + 4u + 4u + 4u + 1u);
                    }

                    public void Read(GamePacketReader reader)
                    {
                        ModelName = reader.ReadString();
                        TextureAlias = reader.ReadString();
                        SemanticTint = reader.ReadString();
                        Unknown3 = reader.ReadString();
                        Unknown4 = reader.ReadUInt();
                        Unknown5 = reader.ReadUInt();
                        Slot = (AttachmentSlot)reader.ReadUInt();
                        Unknown7 = reader.ReadUInt();
                        Unknown8 = reader.ReadBool();
                        
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        writer.Write(ModelName);
                        writer.Write(TextureAlias);
                        writer.Write(SemanticTint);
                        writer.Write(Unknown3);
                        writer.Write(Unknown4);
                        writer.Write(Unknown5);
                        writer.Write((uint)Slot);
                        writer.Write(Unknown7);
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
                public byte Unknown0 { get; set; }
                public uint Unknown1 { get; set; } = 1;
                public uint HeadId { get; set; }
                public uint ModelId { get; set; }
                public uint Gender { get; set; }
                public uint Unknown5 { get; set; } = 1;
                public uint Unknown6 { get; set; }
                public uint Unknown7 { get; set; }
                public ulong Unknown8 { get; set; }
                public List<uint> Array142C34C30 { get; set; } = new();
                public ModelInfo Model { get; set; } = new();
                public List<CharacterAttachment> CharacterAttachments { get; set; } = new();
                public List<Friend> Friends { get; set; } = new();
                public ulong UnknownQ { get; set; }

                public void Read(GamePacketReader reader)
                {
                    var size = reader.ReadUInt();
                    Name = reader.ReadString();
                    Unknown0 = reader.ReadByte();
                    Unknown1 = reader.ReadUInt();
                    HeadId = reader.ReadUInt();
                    ModelId = reader.ReadUInt();
                    Gender = reader.ReadUInt();
                    Unknown5 = reader.ReadUInt();
                    Unknown6 = reader.ReadUInt();
                    Unknown7 = reader.ReadUInt();
                    Unknown8 = reader.ReadULong();

                    var arr142Count = reader.ReadUInt();
                    for (int i = 0; i < arr142Count; i++)
                        Array142C34C30.Add(reader.ReadUInt());

                    Model.Read(reader);

                    var attachmentCount = reader.ReadUInt();
                    for (int i = 0; i < attachmentCount; i++)
                    {
                        var attachment = new CharacterAttachment();
                        attachment.Read(reader);
                        CharacterAttachments.Add(attachment);
                    }
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(GetSize());
                    writer.Write(Name);
                    writer.Write(Unknown0);
                    writer.Write(Unknown1);
                    writer.Write(HeadId);
                    writer.Write(ModelId);
                    writer.Write(Gender);
                    writer.Write(Unknown5);
                    writer.Write(Unknown6);
                    writer.Write(Unknown7);
                    writer.Write(Unknown8);

                    writer.Write((uint)Array142C34C30.Count);
                    Array142C34C30.ForEach(x => writer.Write(x));

                    Model.Write(writer);

                    writer.Write((uint)CharacterAttachments.Count);
                    CharacterAttachments.ForEach(x => x.Write(writer));

                    writer.Write((uint)Friends.Count);
                    Friends.ForEach(x => x.Write(writer));

                    writer.Write(UnknownQ);
                }

                public uint GetSize()
                {
                    uint totalSize = 0u;

                    totalSize += 4u; // Array142C34C30 Count
                    Array142C34C30.ForEach(x => totalSize += 4);

                    totalSize += Model.GetSize();

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
                writer.Write(CharacterId);
                writer.Write(LastServerId);
                writer.Write(LastLogin);
                writer.Write(Status);
                CharacterData.Write(writer);
            }

            public void Read(GamePacketReader reader)
            {
                CharacterId = reader.ReadULong();
                LastServerId = reader.ReadULong();
                LastLogin = reader.ReadDouble();
                Status = reader.ReadUInt();
                CharacterData.Read(reader);
            }
        }
        
        public uint Status { get; set; }
        public bool CanBypassServerLock { get; set; }
        public List<Character> Characters { get; set; } = new();

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Status);
            writer.Write(CanBypassServerLock);

            writer.Write((uint)Characters.Count);
            Characters.ForEach(x => x.Write(writer));
        }

        public void Read(GamePacketReader reader)
        {
            Status = reader.ReadUInt();
            CanBypassServerLock = reader.ReadBool();

            var charCount = reader.ReadUInt();
            for (int i = 0; i < charCount; i++)
            {
                var character = new Character();
                character.Read(reader);
                Characters.Add(character);
            }
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
                    public string SemanticTint { get; set; } = "";  // TintAlias?
                    public string Unknown3 { get; set; } = "";  // DecalAlias?
                    public uint Unknown4 { get; set; }          // TintId?
                    public uint Unknown5 { get; set; }          // CompositeId?
                    public AttachmentSlot Slot { get; set; }    // "ActorUsage"
                    public uint Unknown7 { get; set; }

                    public uint GetSize()
                    {
                        return (uint)(4u + ModelName.Length + 4u + TextureAlias.Length + 4u + SemanticTint.Length + 4u + Unknown3.Length + 4u + 4u + 4u + 4u);
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        writer.Write(ModelName);
                        writer.Write(TextureAlias);
                        writer.Write(SemanticTint);
                        writer.Write(Unknown3);
                        writer.Write(Unknown4);
                        writer.Write(Unknown5);
                        writer.Write((uint)Slot);
                        writer.Write(Unknown7);
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
                public Gender Gender { get; set; }
                public uint Unknown5 { get; set; }
                public uint Unknown6 { get; set; }
                public ulong Unknown7 { get; set; }
                public List<uint> Array142C34C30 { get; set; } = new();
                public List<(uint, uint, uint)> Customizations { get; set; } = new();
                public uint SkinTint { get; set; }
                public List<CharacterAttachment> CharacterAttachments { get; set; } = new();
                public List<Friend> Friends { get; set; } = new();
                public ulong Unknown9 { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(GetSize());
                    writer.Write(Name);
                    writer.Write(Unknown0);
                    writer.Write(Unknown1);
                    writer.Write(HeadId);
                    writer.Write(ModelId);
                    writer.Write((uint)Gender);
                    writer.Write(Unknown5);
                    writer.Write(Unknown6);
                    writer.Write(Unknown7);

                    writer.Write((uint)Array142C34C30.Count);
                    Array142C34C30.ForEach(x => writer.Write(x));

                    writer.Write((uint)Customizations.Count);
                    foreach (var v in Customizations)
                    {
                        writer.Write(v.Item1);
                        writer.Write(v.Item2);
                        writer.Write(v.Item3);
                    }

                    writer.Write(SkinTint);

                    writer.Write((uint)CharacterAttachments.Count);
                    CharacterAttachments.ForEach(x => x.Write(writer));

                    writer.Write((uint)Friends.Count);
                    Friends.ForEach(x => x.Write(writer));

                    writer.Write(Unknown9);
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
                writer.Write(CharacterId);
                writer.Write(LastServerId);
                writer.Write(LastLogin);
                writer.Write(Status);
                CharacterData.Write(writer);
            }
        }

        public uint Status { get; set; }
        public bool CanBypassServerLock { get; set; }
        public List<Character> Characters { get; set; } = new();

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Status);
            writer.Write(CanBypassServerLock);

            writer.Write((uint)Characters.Count);
            Characters.ForEach(x => x.Write(writer));
        }
    }
}
