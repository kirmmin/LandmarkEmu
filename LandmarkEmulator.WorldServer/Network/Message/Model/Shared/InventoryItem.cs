using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;
using System.Collections.Generic;
using NLog;
using LandmarkEmulator.Shared.Game;

namespace LandmarkEmulator.WorldServer.Network.Message.Model.Shared
{
    public class InventoryItem : IReadable, IWritable
    {
        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        // sub_142C20F60
        public class UnknownData0 : IReadable, IWritable
        {
            // sub_142C20E60
            public class UnknownData0SubData : IReadable, IWritable
            {
                // sub_142C2FC70
                public class UnknownStruct142C2FC70 : IReadable, IWritable
                {
                    public uint Unknown0 { get; set; }
                    public LandmarkText Unknown1 { get; set; } = new();
                    public uint Unknown2 { get; set; }
                    public uint Unknown3 { get; set; }
                    public bool Unknown4 { get; set; }
                    public uint Unknown5 { get; set; }
                    public uint Unknown6 { get; set; }
                    public uint Unknown7 { get; set; }

                    public void Read(GamePacketReader reader)
                    {
                        Unknown0 = reader.ReadUInt();
                        Unknown1 = new LandmarkText(reader.ReadUInt());
                        Unknown2 = reader.ReadUInt();
                        Unknown3 = reader.ReadUInt();
                        Unknown4 = reader.ReadBool();
                        Unknown5 = reader.ReadUInt();
                        Unknown6 = reader.ReadUInt();
                        Unknown7 = reader.ReadUInt();
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        writer.Write(Unknown0);
                        Unknown1.Write(writer);
                        writer.Write(Unknown2);
                        writer.Write(Unknown3);
                        writer.Write(Unknown4);
                        writer.Write(Unknown5);
                        writer.Write(Unknown6);
                        writer.Write(Unknown7);
                    }
                }

                // sub_142C18530
                public class UnknownStruct142C18530 : IReadable, IWritable
                {
                    public uint Unknown0 { get; set; }
                    public uint Unknown1 { get; set; }
                    public uint Unknown2 { get; set; }

                    public void Read(GamePacketReader reader)
                    {
                        Unknown0 = reader.ReadUInt();
                        Unknown1 = reader.ReadUInt();
                        Unknown2 = reader.ReadUInt();
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        writer.Write(Unknown0);
                        writer.Write(Unknown1);
                        writer.Write(Unknown2);
                    }
                }

                public uint Unknown0 { get; set; }
                public uint Unknown1 { get; set; }
                public uint Unknown2 { get; set; }
                public uint Unknown3 { get; set; }
                public uint Unknown4 { get; set; }
                public List<UnknownStruct142C2FC70> Unknown5 { get; set; } = new();
                public List<UnknownStruct142C18530> Unknown6 { get; set; } = new();

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();
                    Unknown1 = reader.ReadUInt();
                    Unknown2 = reader.ReadUInt();
                    Unknown3 = reader.ReadUInt();
                    Unknown4 = reader.ReadUInt();

                    uint unknown5Count = reader.ReadUInt();
                    for (int i = 0; i < unknown5Count; i++)
                    {
                        UnknownStruct142C2FC70 obj = new();
                        obj.Read(reader);
                        Unknown5.Add(obj);
                    }

                    uint unknown6Count = reader.ReadUInt();
                    for (int i = 0; i < unknown6Count; i++)
                    {
                        UnknownStruct142C18530 obj = new();
                        obj.Read(reader);
                        Unknown6.Add(obj);
                    }
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Unknown0);
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                    writer.Write(Unknown3);
                    writer.Write(Unknown4);

                    writer.Write((uint)Unknown5.Count);
                    foreach (UnknownStruct142C2FC70 unknown5 in Unknown5)
                        unknown5.Write(writer);

                    writer.Write((uint)Unknown6.Count);
                    foreach (UnknownStruct142C18530 unknown6 in Unknown6)
                        unknown6.Write(writer);

                }
            }

            public uint Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
            public UnknownData0SubData Unknown2 { get; set; } = new();
            public UnknownData0SubData Unknown3 { get; set; } = new();
            public uint Unknown4 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
                Unknown1 = reader.ReadUInt();

                Unknown2.Read(reader);
                Unknown3.Read(reader);

                Unknown4 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);

                Unknown2.Write(writer);
                Unknown3.Write(writer);

                writer.Write(Unknown4);
            }
        }

        public class SubData : IReadable, IWritable
        {
            public ulong Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
            public uint Unknown2 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadULong();
                Unknown1 = reader.ReadUInt();
                Unknown2 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
            }
        }

        public class SubData2 : IReadable, IWritable
        {
            public ulong Unknown0 { get; set; }
            public string Unknown1 { get; set; }
            public string Unknown2 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadULong();
                Unknown1 = reader.ReadString();
                Unknown2 = reader.ReadString();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
            }
        }

        public class SubData3 : IReadable, IWritable
        {
            public byte Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
            public SubData Unknown2 { get; set; } = new();

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadByte();
                if (Unknown0 == 0)
                    return;

                Unknown1 = reader.ReadUInt();
                if (Unknown0 != 1)
                    return;

                Unknown2.Read(reader);
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                if (Unknown0 == 0)
                    return;

                writer.Write(Unknown1);
                if (Unknown0 != 1)
                    return;

                Unknown2.Write(writer);
            }
        }

        // sub_14361AAD0
        public class ExtraData : IReadable, IWritable
        {
            public byte Unknown0 { get; set; }

            public void Read(GamePacketReader reader)
            {
                // This data may parse different based on whether it's a weapon or not
                Unknown0 = reader.ReadByte();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);

                // TODO: Handle ExtraData not being 0.
                //if (Unknown0 != 0)
                //    throw new NotImplementedException();
            }
        }

        public UnknownData0 UnknownData { get; set; } = new();
        public ulong Guid { get; set; }
        public uint Count { get; set; }
        public uint Unknown2 { get; set; }
        public uint Unknown3 { get; set; }
        public byte Unknown4 { get; set; }
        /// <summary>
        /// 0 = Inventory, 2 = Equipment, 5 = Collection
        /// </summary>
        public uint ContainerId { get; set; }
        public ulong Unknown6 { get; set; }
        public uint SlotId { get; set; }
        public uint Unknown8 { get; set; }
        public ulong Unknown9 { get; set; }
        public ulong Unknown10 { get; set; }
        public uint Unknown11 { get; set; }
        public uint Unknown12 { get; set; }
        public byte Unknown13 { get; set; }
        public uint Unknown14 { get; set; }
        public byte subData1Flag { get; set; }
        public SubData SubDataItem { get; set; }
        public SubData2 SubData2Item { get; set; }
        public SubData3 SubData3Item { get; set; } = new();
        public uint Unknown15 { get; set; }
        public ExtraData _ExtraData { get; set; } = new();

        public void Read(GamePacketReader reader)
        {
            UnknownData.Read(reader);

            Guid = reader.ReadULong();
            Count = reader.ReadUInt();
            Unknown2 = reader.ReadUInt();
            Unknown3 = reader.ReadUInt();
            Unknown4 = reader.ReadByte();
            ContainerId = reader.ReadUInt();
            Unknown6 = reader.ReadULong();
            SlotId = reader.ReadUInt();
            Unknown8 = reader.ReadUInt();
            Unknown9 = reader.ReadULong();
            Unknown10 = reader.ReadULong();
            Unknown11 = reader.ReadUInt();
            Unknown12 = reader.ReadUInt();
            Unknown13 = reader.ReadByte();
            Unknown14 = reader.ReadUInt();

            byte hasSubData = reader.ReadByte();
            subData1Flag = hasSubData;
            if (hasSubData != 0)
            {
                SubDataItem = new();
                SubDataItem.Read(reader);
            }

            byte hasSubData2 = reader.ReadByte();
            if (hasSubData2 != 0)
            {
                SubData2Item = new();
                SubData2Item.Read(reader);
            }

            SubData3Item.Read(reader);

            Unknown15 = reader.ReadUInt();

            _ExtraData.Read(reader);
        }

        public void Write(GamePacketWriter writer)
        {
            UnknownData.Write(writer);

            writer.Write(Guid);
            writer.Write(Count);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(ContainerId);
            writer.Write(Unknown6);
            writer.Write(SlotId);
            writer.Write(Unknown8);
            writer.Write(Unknown9);
            writer.Write(Unknown10);
            writer.Write(Unknown11);
            writer.Write(Unknown12);
            writer.Write(Unknown13);
            writer.Write(Unknown14);

            if (SubDataItem == null)
                writer.Write((byte)0);
            else
            {
                // TODO: Writer flag to indicate to client there is SubData1
                writer.Write((byte)1);
                SubDataItem.Write(writer);
            }

            if (SubData2Item == null)
                writer.Write((byte)0);
            else
            {
                // TODO: Writer flag to indicate to client there is SubData1
                writer.Write((byte)1);
                SubData2Item.Write(writer);
            }

            SubData3Item.Write(writer);

            writer.Write(Unknown15);

            _ExtraData.Write(writer);
        }
    }
}
