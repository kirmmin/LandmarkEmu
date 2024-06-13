using LandmarkEmulator.Shared.Game;
using LandmarkEmulator.Shared.Game.Inventory.Static;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using LandmarkEmulator.WorldServer.Network.Message.Model.Shared;
using NLog;
using System.Collections.Generic;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.CommandItemDefinitions, prependSize: true)]
    public class CommandItemDefinitions : IReadable, IWritable
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public class ItemDefinition : IReadable, IWritable
        {
            // sub_142C203F0
            public class Ability : IReadable, IWritable
            {
                public uint Id { get; set; }
                public uint Unknown1 { get; set; }
                public string Unknown2 { get; set; }
                public string Unknown3 { get; set; }
                public uint Unknown4 { get; set; }
                public byte Unknown5 { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Id       = reader.ReadUInt();
                    Unknown1 = reader.ReadUInt();
                    Unknown2 = reader.ReadString();
                    Unknown3 = reader.ReadString();
                    Unknown4 = reader.ReadUInt();
                    Unknown5 = reader.ReadByte();
                }

                public void Write(GamePacketWriter writer)
                {
                    throw new System.NotImplementedException();
                }
            }

            public uint Id { get; set; }
            public byte Flags1 { get; set; }
            public byte Flags2 { get; set; }
            public byte Flags3 { get; set; }
            public LandmarkText NameId { get; set; } = new();
            public LandmarkText DescriptionId { get; set; } = new();
            public uint ImageSetId { get; set; }
            public uint Unknown7 { get; set; }
            public uint Unknown8 { get; set; }
            public uint Unknown9 { get; set; }
            public uint Unknown10 { get; set; }
            public uint EquipSlotId { get; set; }
            public string ModelFile { get; set; }
            public string Unknown13 { get; set; }
            public uint Unknown14 { get; set; }
            public uint Unknown15 { get; set; }
            public uint Unknown16 { get; set; }
            public uint Unknown17 { get; set; }
            public uint Unknown18 { get; set; }
            public uint MinStackSize { get; set; }
            public uint Unknown20 { get; set; }
            public uint MaxStackSize { get; set; }
            public string TintAlias { get; set; }
            public uint Unknown23 { get; set; }
            public uint Unknown24 { get; set; }
            public uint Unknown25 { get; set; }
            public uint Unknown26 { get; set; }
            public uint Unknown27 { get; set; }
            public uint Unknown28 { get; set; }
            public uint Unknown29 { get; set; }
            public uint Unknown30 { get; set; }
            public uint Unknown31 { get; set; }
            public uint Unknown32 { get; set; }
            public string Unknown33 { get; set; }
            public string Unknown34 { get; set; }
            public uint Unknown35 { get; set; }
            public uint Unknown36 { get; set; }
            public uint Unknown37 { get; set; }
            public uint Unknown38 { get; set; }
            public uint Unknown39 { get; set; }
            public ItemQuality Quality { get; set; }
            public uint Unknown41 { get; set; }
            public uint Unknown42 { get; set; }
            public uint Unknown43 { get; set; }
            public uint Unknown44 { get; set; }
            public uint Unknown45 { get; set; }
            public uint Unknown46 { get; set; }
            public List<uint> Unknown47 { get; set; } = new();
            public uint Unknown48 { get; set; }
            public uint Unknown49 { get; set; }
            public uint Unknown50 { get; set; }
            public uint Unknown51 { get; set; }
            public uint Unknown52 { get; set; }
            public uint Unknown53 { get; set; }
            public List<Ability> Abilities { get; set; } = new();
            public string SecondaryModelFile { get; set; }
            public string Unknown56 { get; set; }
            public string SecondaryTintAlias { get; set; }
            public uint Unknown58 { get; set; }
            public List<uint> Unknown59 { get; set; } = new();
            public uint GrantId { get; set; }
            public uint GrantAmount { get; set; }
            public Dictionary<uint, (StatData, uint)> Stats { get; set; } = new();
            public uint BytesRemaining { get; set; }

            public void Read(GamePacketReader reader)
            {
                Id = reader.ReadUInt();
                Flags1 = reader.ReadByte();
                Flags2 = reader.ReadByte();
                Flags3 = reader.ReadByte();
                NameId.Read(reader);
                DescriptionId.Read(reader);
                ImageSetId = reader.ReadUInt();
                Unknown7 = reader.ReadUInt();
                Unknown8 = reader.ReadUInt();
                Unknown9 = reader.ReadUInt();
                Unknown10 = reader.ReadUInt();
                EquipSlotId = reader.ReadUInt();
                ModelFile = reader.ReadString();
                Unknown13 = reader.ReadString();
                Unknown14 = reader.ReadUInt();
                Unknown15 = reader.ReadUInt();
                Unknown16 = reader.ReadUInt();
                Unknown17 = reader.ReadUInt();
                Unknown18 = reader.ReadUInt();
                MinStackSize = reader.ReadUInt();
                Unknown20 = reader.ReadUInt();
                MaxStackSize = reader.ReadUInt();
                TintAlias = reader.ReadString();
                Unknown23 = reader.ReadUInt();
                Unknown24 = reader.ReadUInt();
                Unknown25 = reader.ReadUInt();
                Unknown26 = reader.ReadUInt();
                Unknown27 = reader.ReadUInt();
                Unknown28 = reader.ReadUInt();
                Unknown29 = reader.ReadUInt();
                Unknown30 = reader.ReadUInt();
                Unknown31 = reader.ReadUInt();
                Unknown32 = reader.ReadUInt();
                Unknown33 = reader.ReadString();
                Unknown34 = reader.ReadString();
                Unknown35 = reader.ReadUInt();
                Unknown36 = reader.ReadUInt();
                Unknown37 = reader.ReadUInt();
                Unknown38 = reader.ReadUInt();
                Unknown39 = reader.ReadUInt();
                Quality = (ItemQuality)reader.ReadUInt();
                Unknown41 = reader.ReadUInt();
                Unknown42 = reader.ReadUInt();
                Unknown43 = reader.ReadUInt();
                Unknown44 = reader.ReadUInt();
                Unknown45 = reader.ReadUInt();
                Unknown46 = reader.ReadUInt();

                var unknown47Count = reader.ReadUInt();
                for (int i = 0; i < unknown47Count; i++)
                    Unknown47.Add(reader.ReadUInt());

                Unknown48 = reader.ReadUInt();
                Unknown49 = reader.ReadUInt();
                Unknown50 = reader.ReadUInt();
                Unknown51 = reader.ReadUInt();
                Unknown52 = reader.ReadUInt();
                Unknown53 = reader.ReadUInt();

                var abilityCount = reader.ReadUInt();
                for (int i = 0; i < abilityCount; i++)
                {
                    Ability ability = new();
                    ability.Read(reader);
                    Abilities.Add(ability);
                }

                SecondaryModelFile = reader.ReadString();
                Unknown56 = reader.ReadString();
                SecondaryTintAlias = reader.ReadString();
                Unknown58 = reader.ReadUInt();

                var unknown59Count = reader.ReadUInt();
                for (int i = 0; i < unknown59Count; i++)
                    Unknown59.Add(reader.ReadUInt());

                GrantId     = reader.ReadUInt();
                GrantAmount = reader.ReadUInt();

                var statCount = reader.ReadUInt();
                for (int i = 0; i < statCount; i++)
                {
                    uint statId = reader.ReadUInt();

                    StatData statData = new();
                    statData.Read(reader);

                    uint unknown0 = reader.ReadUInt();

                    Stats.Add(statId, (statData, unknown0));
                }

                BytesRemaining = reader.BytesRemaining;
            }

            public void Write(GamePacketWriter writer)
            {
                throw new System.NotImplementedException();
            }
        }

        public uint Size { get; set; }
        public uint ItemCount { get; set; }
        public List<ItemDefinition> Items { get; set; } = new();

        public void Read(GamePacketReader reader)
        {
            Size = reader.ReadUInt();
            ItemCount = reader.ReadUInt();

            for (int i = 0; i < ItemCount; i++)
            {
                var input = reader.ReadUShort(); // Compressed Byte Size
                var output = reader.ReadUShort(); // Decompressed Byte Size

                var itemBytes = reader.ReadBytes(input); // Collects the compressed bytes to be decompressed and parsed

                // Parse Item from compressed bytes
                var itemReader = new GamePacketReader(LZ4.LZ4Codec.Decode(itemBytes, 0, input, output)); // Decompress the bytes before sending to reader
                ItemDefinition item = new();
                item.Read(itemReader);
                Items.Add(item);
            }
        }

        public void Write(GamePacketWriter writer)
        {

        }
    }
}
