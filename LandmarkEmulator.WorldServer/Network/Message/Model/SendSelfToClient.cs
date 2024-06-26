﻿using LandmarkEmulator.Shared.Game;
using LandmarkEmulator.Shared.Game.Entity.Static;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using LandmarkEmulator.Shared.Network.Message.Model.Shared;
using LandmarkEmulator.WorldServer.Network.Message.Model.Shared;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.SendSelfToClient, prependSize: true)]
    public class SendSelfToClient : IReadable, IWritable
    {
        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public class CharacterIdentity : IReadable, IWritable
        {
            public uint Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
            public uint Unknown2 { get; set; }
            public string FirstName { get; set; } = "Player";
            public string LastName { get; set; } = "";

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
                Unknown1 = reader.ReadUInt();
                Unknown2 = reader.ReadUInt();
                FirstName = reader.ReadString();
                LastName = reader.ReadString();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(FirstName);
                writer.Write(LastName);
            }
        }

        // Seems replicated here, or vey close at least: https://github.com/H1emu/h1z1-server/blob/master/src/packets/ClientProtocol/ClientProtocol_860/shared.ts#L1449-L1484
        public class Profile : IReadable, IWritable
        {
            public uint Unknown0 { get; set; }
            public LandmarkText NameId { get; set; } = new();
            public uint Unknown2 { get; set; }
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }
            public uint Unknown5 { get; set; }
            public uint Unknown6 { get; set; }
            public uint Unknown7 { get; set; }
            public byte Unknown8 { get; set; }
            public uint Unknown9 { get; set; }
            public List<(uint, uint, uint)> Unknown10 { get; set; } = new();
            public byte Unknown11 { get; set; }
            public uint Unknown12 { get; set; }
            public uint Unknown13 { get; set; }
            public byte Unknown14 { get; set; }
            public bool Unknown15 { get; set; }
            public byte Unknown16 { get; set; }
            public byte Unknown17 { get; set; }
            public byte Unknown18 { get; set; }

            public float Unknown19 { get; set; }
            public float Unknown20 { get; set; }
            public float Unknown21 { get; set; }
            public uint Unknown22 { get; set; }
            public float Unknown23 { get; set; }
            public float Unknown24 { get; set; }
            public uint Unknown25 { get; set; }
            public uint Unknown26 { get; set; }
            public uint Unknown27 { get; set; }
            public uint Unknown28 { get; set; }
            public uint Unknown29 { get; set; }
            public uint Unknown30 { get; set; }
            public uint Unknown31 { get; set; }
            public uint Unknown32 { get; set; }
            public uint Unknown33 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
                NameId = new LandmarkText(reader.ReadUInt());
                Unknown2 = reader.ReadUInt();
                Unknown3 = reader.ReadUInt();
                Unknown4 = reader.ReadUInt();
                Unknown5 = reader.ReadUInt();
                Unknown6 = reader.ReadUInt();
                Unknown7 = reader.ReadUInt();
                Unknown8 = reader.ReadByte();
                Unknown9 = reader.ReadUInt();

                var unknown10Count = reader.ReadUInt();
                for (int i = 0; i < unknown10Count; i++)
                    Unknown10.Add((reader.ReadUInt(), reader.ReadUInt(), reader.ReadUInt()));

                Unknown11 = reader.ReadByte();
                Unknown12 = reader.ReadUInt();
                Unknown13 = reader.ReadUInt();
                Unknown14 = reader.ReadByte();
                Unknown15 = reader.ReadBool();
                Unknown16 = reader.ReadByte();
                Unknown17 = reader.ReadByte();
                Unknown18 = reader.ReadByte();

                Unknown19 = reader.ReadUInt();
                Unknown20 = reader.ReadUInt();
                Unknown21 = reader.ReadUInt();
                Unknown22 = reader.ReadUInt();
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
                Unknown33 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                NameId.Write(writer);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
                writer.Write(Unknown5);
                writer.Write(Unknown6);
                writer.Write(Unknown7);
                writer.Write(Unknown8);
                writer.Write(Unknown9);

                writer.Write((uint)Unknown10.Count);
                foreach (var item in Unknown10)
                {
                    writer.Write(item.Item1);
                    writer.Write(item.Item2);
                    writer.Write(item.Item3);
                }

                writer.Write(Unknown11);
                writer.Write(Unknown12);
                writer.Write(Unknown13);
                writer.Write(Unknown14);
                writer.Write(Unknown15);
                writer.Write(Unknown16);
                writer.Write(Unknown17);
                writer.Write(Unknown18);
                
                writer.Write(Unknown19);
                writer.Write(Unknown20);
                writer.Write(Unknown21);
                writer.Write(Unknown22);
                writer.Write(Unknown23);
                writer.Write(Unknown24);
                writer.Write(Unknown25);
                writer.Write(Unknown26);
                writer.Write(Unknown27);
                writer.Write(Unknown28);
                writer.Write(Unknown29);
                writer.Write(Unknown30);
                writer.Write(Unknown31);
                writer.Write(Unknown32);
                writer.Write(Unknown33);
            }
        }

        public class Collection : IReadable, IWritable
        {
            public uint Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
            public uint Unknown2 { get; set; }
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }
            public uint Unknown5 { get; set; }
            public uint Unknown6 { get; set; }
            public RewardBundle RewardBundleData { get; set; } = new();
            // TODO: Add Extra Func

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
                Unknown1 = reader.ReadUInt();
                Unknown2 = reader.ReadUInt();
                Unknown3 = reader.ReadUInt();
                Unknown4 = reader.ReadUInt();
                Unknown5 = reader.ReadUInt();
                Unknown6 = reader.ReadUInt();
                RewardBundleData.Read(reader);
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
                writer.Write(Unknown5);
                writer.Write(Unknown6);
                RewardBundleData.Write(writer);
            }
        }

        public class Buff : IReadable, IWritable
        {
            public ulong Unknown0 { get; set; }
            public ulong UnitId { get; set; }
            public uint Unknown2 { get; set; }
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }
            public uint Unknown5 { get; set; }
            public uint Unknown6 { get; set; }
            public uint Unknown7 { get; set; }
            public uint Unknown8 { get; set; }
            public ulong UnitId2 { get; set; }
            public ulong UnitId3 { get; set; }
            public uint Unknown11 { get; set; }
            public ulong Unknown12 { get; set; }
            public uint Unknown13 { get; set; }
            public uint Unknown14 { get; set; }
            public Vector4 Unknown15 { get; set; } = new();
            public uint Unknown16 { get; set; }
            public uint Unknown17 { get; set; }
            public uint Unknown18 { get; set; }
            public uint Unknown19 { get; set; }
            public uint Unknown20 { get; set; }
            public Vector4 Unknown21 { get; set; } = new();
            public LandmarkText NameId { get; set; } = new();
            public LandmarkText DescriptionId { get; set; } = new();
            public uint IconId { get; set; }
            public uint Unknown25 { get; set; }
            public bool Unknown26 { get; set; }
            public uint Unknown27 { get; set; }
            public uint Unknown28 { get; set; }
            public uint Unknown29 { get; set; }
            public uint Unknown30 { get; set; }
            public uint Unknown31 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadULong();
                UnitId = reader.ReadULong();
                Unknown2 = reader.ReadUInt();
                Unknown3 = reader.ReadUInt();
                Unknown4 = reader.ReadUInt();
                Unknown5 = reader.ReadUInt();
                Unknown6 = reader.ReadUInt();
                Unknown7 = reader.ReadUInt();
                Unknown8 = reader.ReadUInt();
                UnitId2 = reader.ReadULong();
                UnitId3 = reader.ReadULong();
                Unknown11 = reader.ReadUInt();
                Unknown12 = reader.ReadULong();
                Unknown13 = reader.ReadUInt();
                Unknown14 = reader.ReadUInt();

                Unknown15 = new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                Unknown16 = reader.ReadUInt();
                Unknown17 = reader.ReadUInt();
                Unknown18 = reader.ReadUInt();
                Unknown19 = reader.ReadUInt();
                Unknown20 = reader.ReadUInt();

                Unknown21 = new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                NameId    = new LandmarkText(reader.ReadUInt());
                DescriptionId = new LandmarkText(reader.ReadUInt());
                IconId    = reader.ReadUInt();
                Unknown25 = reader.ReadUInt();
                Unknown26 = reader.ReadBool();
                Unknown27 = reader.ReadUInt();
                Unknown28 = reader.ReadUInt();
                Unknown29 = reader.ReadUInt();
                Unknown30 = reader.ReadUInt();
                Unknown31 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(UnitId);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
                writer.Write(Unknown5);
                writer.Write(Unknown6);
                writer.Write(Unknown7);
                writer.Write(Unknown8);
                writer.Write(UnitId2);
                writer.Write(UnitId3);
                writer.Write(Unknown11);
                writer.Write(Unknown12);
                writer.Write(Unknown13);
                writer.Write(Unknown14);

                writer.Write(Unknown15);

                writer.Write(Unknown16);
                writer.Write(Unknown17);
                writer.Write(Unknown18);
                writer.Write(Unknown19);
                writer.Write(Unknown20);
                
                writer.Write(Unknown21);

                NameId.Write(writer);
                DescriptionId.Write(writer);
                writer.Write(IconId);
                writer.Write(Unknown26);
                writer.Write(Unknown27);
                writer.Write(Unknown28);
                writer.Write(Unknown29);
                writer.Write(Unknown30);
                writer.Write(Unknown31);
            }
        }

        public class InventoryData : IReadable, IWritable
        {
            /// <summary>
            /// Seems to default to current number of items in container 0 for maxxSize on Containers 7+. Fized size of 10 containers.
            /// </summary>
            public List<(uint /*maxSize*/, bool /*visible?*/)> Containers { get; set; } = new()
            {
                ( 40, false ),
                ( 60, false ),
                ( 10, false ),
                ( 10, false ),
                ( 10, false ),
                ( 250, false ),
                ( 250, false ),
                ( 0, true),
                ( 0, true),
                ( 0, true),
                ( 0, true),
                ( 0, true)
            };
            public List<InventoryItem> Items { get; set; } = new();

            public void Read(GamePacketReader reader)
            {
                Containers = new();
                for (int i = 0; i < 12; i++)
                    Containers.Add((reader.ReadUInt(), reader.ReadBool()));

                uint itemCount = reader.ReadUInt();
                for (int i = 0; i < itemCount; i++)
                {
                    InventoryItem item = new();
                    item.Read(reader);
                    Items.Add(item);
                }
            }

            public void Write(GamePacketWriter writer)
            {
                log.Info($"Writing {Containers.Count} containers");
                foreach (var item in Containers)
                {
                    writer.Write(item.Item1);
                    writer.Write(item.Item2);
                }

                writer.Write((uint)Items.Count);
                foreach (InventoryItem item in Items)
                    item.Write(writer);
            }
        }

        public class UnknownStruct34 : IReadable, IWritable
        {
            public ulong Unknown0 { get; private set; }
            public string Unknown1 { get; private set; } = "";

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadULong();
                Unknown1 = reader.ReadString();
            }

            public void Write(GamePacketWriter writer)
            {
                throw new NotImplementedException();
            }
        }

        public class QuestData : IReadable, IWritable
        {
            public uint QuestList { get; private set; }
            public uint Unknown0 { get; private set; }
            public uint Unknown1 { get; private set; }
            public bool Unknown2 { get; private set; }
            public uint Unknown3 { get; private set; }
            public uint Unknown4 { get; private set; }

            public void Read(GamePacketReader reader)
            {
                uint questCount = reader.ReadUInt();
                QuestList = questCount;
                for (int i = 0; i < questCount; i++)
                {
                    // Do This
                    throw new NotImplementedException();
                }

                Unknown0 = reader.ReadUInt();
                Unknown1 = reader.ReadUInt();
                Unknown2 = reader.ReadBool();
                Unknown3 = reader.ReadUInt();
                Unknown4 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                // TODO: Add support for Quest packet
                writer.Write(QuestList);
                if (QuestList > 0)
                    throw new NotImplementedException();
                writer.Write(Unknown0);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
            }
        }

        // sub_142C34B60
        public class UnknownStruct142C34B60 : IReadable, IWritable
        {
            public List<uint> Unknown0 { get; set; } = new();

            public void Read(GamePacketReader reader)
            {
                uint count = reader.ReadUInt();

                for (int i = 0; i < count; i++)
                    Unknown0.Add(reader.ReadUInt());
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write((uint)Unknown0.Count);
                foreach (uint item in Unknown0)
                    writer.Write(item);
            }
        }

        // sub_142C363D0
        public class UnknownStruct142C363D0 : IReadable, IWritable
        {
            public class UnknownStruct142C39F80 : IReadable, IWritable
            {
                public class UnknownStruct142C23120 : IReadable, IWritable
                {
                    public uint Unknown0 { get; set; } = new();
                    public uint Unknown1 { get; set; } = new();
                    public uint Unknown2 { get; set; } = new();
                    public uint Unknown3 { get; set; } = new();

                    public void Read(GamePacketReader reader)
                    {
                        Unknown0 = reader.ReadUInt();
                        Unknown1 = reader.ReadUInt();
                        Unknown2 = reader.ReadUInt();
                        Unknown3 = reader.ReadUInt();
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        writer.Write(Unknown0);
                        writer.Write(Unknown1);
                        writer.Write(Unknown2);
                        writer.Write(Unknown3);
                    }
                }

                public List<UnknownStruct142C23120> Unknown0 { get; set; } = new();

                public void Read(GamePacketReader reader)
                {
                    uint count = reader.ReadUInt();

                    for (int i = 0; i < count; i++)
                    {
                        UnknownStruct142C23120 unknown0Item = new();
                        unknown0Item.Read(reader);
                        Unknown0.Add(unknown0Item);
                    }
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write((uint)Unknown0.Count);
                    foreach (UnknownStruct142C23120 unknown0 in Unknown0)
                        unknown0.Write(writer);
                }
            }

            public List<(uint, UnknownStruct142C39F80)> Unknown0 { get; set; } = new();

            public void Read(GamePacketReader reader)
            {
                uint count = reader.ReadUInt();

                for (int i = 0; i < count; i++)
                {
                    uint unknown0 = reader.ReadUInt();
                    UnknownStruct142C39F80 unknown1 = new();
                    unknown1.Read(reader);
                    Unknown0.Add((unknown0, unknown1));
                }
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write((uint)Unknown0.Count);
                foreach (var item in Unknown0)
                {
                    writer.Write(item.Item1);
                    item.Item2.Write(writer);
                }
            }
        }

        // sub_142C34F90
        public class UnknownStruct142C34F90 : IReadable, IWritable
        {
            public List<(uint, uint)> Unknown0 { get; private set; } = new();

            public void Read(GamePacketReader reader)
            {
                uint unknown0Count = reader.ReadUInt();

                for (int i = 0; i < unknown0Count; i++)
                    Unknown0.Add((reader.ReadUInt(), reader.ReadUInt()));
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write((uint)Unknown0.Count);
                foreach (var item in Unknown0)
                {
                    writer.Write(item.Item1);
                    writer.Write(item.Item2);
                }
            }
        }

        public class RecipeData : IReadable, IWritable
        {
            // sub_142C1B810
            public class StructUnknown142C1B810 : IReadable, IWritable
            {
                public uint Unknown0 { get; private set; }
                public uint Unknown1 { get; private set; }
                public ulong Unknown2 { get; private set; }

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();
                    Unknown1 = reader.ReadUInt();
                    Unknown2 = reader.ReadULong();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Unknown0);
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                }
            }

            // sub_142C2E640
            public class Recipe : IReadable, IWritable
            {
                // sub_142C2E4E0
                public class RecipeComponent : IReadable, IWritable
                {
                    public uint Index { get; private set; }
                    public uint Unknown1 { get; private set; }
                    public uint ItemId { get; private set; }
                    public uint Quantity { get; private set; }
                    public LandmarkText NameId { get; private set; } = new();
                    public uint IconId { get; private set; }
                    public uint Unknown6 { get; private set; }
                    public uint Unknown7 { get; private set; }

                    public void Read(GamePacketReader reader)
                    {
                        Index = reader.ReadUInt();
                        Unknown1 = reader.ReadUInt();
                        ItemId = reader.ReadUInt();
                        Quantity = reader.ReadUInt();
                        NameId = new LandmarkText(reader.ReadUInt());
                        IconId = reader.ReadUInt();
                        Unknown6 = reader.ReadUInt();
                        Unknown7 = reader.ReadUInt();
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        writer.Write(Index);
                        writer.Write(Unknown1);
                        writer.Write(ItemId);
                        writer.Write(Quantity);
                        NameId.Write(writer);
                        writer.Write(IconId);
                        writer.Write(Unknown6);
                        writer.Write(Unknown7);
                    }
                }

                public uint RecipeId { get; private set; }
                public uint Unknown1 { get; private set; }
                public uint Unknown2 { get; private set; }
                public uint Itemid { get; private set; }
                public uint Quantity { get; private set; }
                public LandmarkText NameId { get; private set; } = new();
                public LandmarkText DescriptionId { get; private set; } = new();
                public uint IconId { get; private set; }
                public uint Unknown8 { get; private set; }
                public uint Unknown9 { get; private set; }
                public List<RecipeComponent> Components { get; private set; } = new();
                public bool MemberOnly { get; private set; }
                public uint Unknown12 { get; private set; }
                public uint Unknown13 { get; private set; }
                public uint Unknown14 { get; private set; }
                public uint Unknown15 { get; private set; }
                public List<uint> Unknown16 { get; private set; } = new();
                public uint Unknown17 { get; private set; }

                public void Read(GamePacketReader reader)
                {
                    RecipeId = reader.ReadUInt();
                    Unknown1 = reader.ReadUInt();
                    Unknown2 = reader.ReadUInt();
                    Itemid = reader.ReadUInt();
                    Quantity = reader.ReadUInt();
                    NameId = new LandmarkText(reader.ReadUInt());
                    DescriptionId = new LandmarkText(reader.ReadUInt());
                    IconId = reader.ReadUInt();
                    Unknown8 = reader.ReadUInt();
                    Unknown9 = reader.ReadUInt(); // Looks like some form of timestamp? Might be timestamp recipe was acquired or duration remaining.

                    uint recipeComponentsCount = reader.ReadUInt();
                    for (int i = 0; i < recipeComponentsCount; i++)
                    {
                        RecipeComponent component = new();
                        component.Read(reader);
                        Components.Add(component);
                    }

                    MemberOnly = reader.ReadBool();
                    Unknown12 = reader.ReadUInt(); // Could be crafting station or something. Lots of common values.
                    Unknown13 = reader.ReadUInt();
                    Unknown14 = reader.ReadUInt();
                    Unknown15 = reader.ReadUInt();

                    uint unknown16Count = reader.ReadUInt();
                    for (int i = 0; i < unknown16Count; i++)
                        Unknown16.Add(reader.ReadUInt());

                    Unknown17 = reader.ReadUInt();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(RecipeId);
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                    writer.Write(Itemid);
                    writer.Write(Quantity);
                    NameId.Write(writer);
                    DescriptionId.Write(writer);
                    writer.Write(IconId);
                    writer.Write(Unknown8);
                    writer.Write(Unknown9);

                    writer.Write((uint)Components.Count);
                    foreach (RecipeComponent component in Components)
                        component.Write(writer);

                    writer.Write(MemberOnly);
                    writer.Write(Unknown12);
                    writer.Write(Unknown13);
                    writer.Write(Unknown14);
                    writer.Write(Unknown15);

                    writer.Write((uint)Unknown16.Count);
                    foreach (uint item in Unknown16)
                        writer.Write(item);

                    writer.Write(Unknown17);
                }
            }

            public List<uint> Unknown0 { get; set; } = new();
            public List<StructUnknown142C1B810> Unknown1 { get; set; } = new();
            public List<uint> Unknown2 { get; set; } = new();
            public List<Recipe> RecipeList { get; set; } = new();

            public void Read(GamePacketReader reader)
            {
                uint unknown0Count = reader.ReadUInt();
                for (int i = 0; i < unknown0Count; i++)
                    Unknown0.Add(reader.ReadUInt());

                uint unknown1Count = reader.ReadUInt();
                for (int i = 0; i < unknown1Count; i++)
                {
                    StructUnknown142C1B810 unknown1 = new();
                    unknown1.Read(reader);
                    Unknown1.Add(unknown1);
                }

                uint unknown2Count = reader.ReadUInt();
                for (int i = 0; i < unknown2Count; i++)
                    Unknown2.Add(reader.ReadUInt());

                uint recipeCount = reader.ReadUInt();
                for (int i = 0; i < recipeCount; i++)
                {
                    Recipe recipe = new();
                    recipe.Read(reader);
                    RecipeList.Add(recipe);
                }
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write((uint)Unknown0.Count);
                foreach (uint unknown0 in Unknown0)
                    writer.Write(unknown0);

                writer.Write((uint)Unknown1.Count);
                foreach (StructUnknown142C1B810 unknown1 in Unknown1)
                    unknown1.Write(writer);

                writer.Write((uint)Unknown2.Count);
                foreach (uint unknown2 in Unknown2)
                    writer.Write(unknown2);

                writer.Write((uint)RecipeList.Count);
                foreach (Recipe recipe in RecipeList)
                    recipe.Write(writer);
            }
        }

        public class Hotbar : IReadable, IWritable
        {
            // sub_142C16C60
            public class HotbarButton : IReadable, IWritable
            {
                public uint HotbarId { get; set; }
                public uint SlotId { get; set; }
                /// <summary>
                /// 0 = None, 1 = ItemId, 2 = ItemGuid
                /// </summary>
                public uint ButtonType { get; set; }
                public ulong ItemId { get; set; }

                public void Read(GamePacketReader reader)
                {
                    HotbarId   = reader.ReadUInt();
                    SlotId     = reader.ReadUInt();
                    ButtonType = reader.ReadUInt();
                    ItemId     = reader.ReadULong();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(HotbarId);
                    writer.Write(SlotId);
                    writer.Write(ButtonType);
                    writer.Write(ItemId);
                }
            }

            public uint HotbarId { get; private set; }
            public List<HotbarButton> HotbarButtons { get; private set; } = new();

            public void Read(GamePacketReader reader)
            {
                HotbarId = reader.ReadUInt();

                uint hotbarButtonsCount = reader.ReadUInt();
                for (int i = 0; i < hotbarButtonsCount; i++)
                {
                    HotbarButton hotbarButton = new();
                    hotbarButton.Read(reader);
                    HotbarButtons.Add(hotbarButton);
                }
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(HotbarId);

                writer.Write((uint)HotbarButtons.Count);
                foreach (HotbarButton button in HotbarButtons)
                    button.Write(writer);
            }
        }

        public class TitleData : IReadable, IWritable
        {
            public class Title : IReadable, IWritable
            {
                public uint TitleId { get; set; }
                public uint TitleType { get; set; }
                /// <summary>
                /// This is an ID reference to a string in the localisation files
                /// </summary>
                public LandmarkText NameId { get; set; } = new();

                public void Read(GamePacketReader reader)
                {
                    TitleId = reader.ReadUInt();
                    TitleType = reader.ReadUInt();
                    NameId = new LandmarkText(reader.ReadUInt());
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(TitleId);
                    writer.Write(TitleType);
                    NameId.Write(writer);
                }
            }

            public List<Title> TitleList { get; set; } = new();
            public uint CurrentTitleId { get; set; }

            public void Read(GamePacketReader reader)
            {
                uint titleListCount = reader.ReadUInt();
                for (int i = 0; i < titleListCount; i++)
                {
                    Title title = new();
                    title.Read(reader);
                    TitleList.Add(title);
                }

                CurrentTitleId = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write((uint)TitleList.Count);
                foreach (Title title in TitleList)
                    title.Write(writer);

                writer.Write(CurrentTitleId);
            }
        }

        public class UnknownStruct50 : IReadable, IWritable
        {
            public uint Unknown0 { get; set; }
            public uint Unknown1 { get; set; }

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

        public class UnknownStruct56 : IReadable, IWritable
        {
            // sub_142C3BF00
            public class UnknownStruct142C3BF00 : IReadable, IWritable
            {
                public uint Unknown0 { get; set; }
                public uint Unknown1 { get; set; }
                public uint Unknown2 { get; set; }
                public ulong Unknown3 { get; set; }
                public uint Unknown4 { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();
                    Unknown1 = reader.ReadUInt();
                    Unknown2 = reader.ReadUInt();
                    Unknown3 = reader.ReadULong();
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

            // sub_142C3F7F0
            public class UnknownStruct142C3F7F0 : IReadable, IWritable
            {
                public uint Unknown0 { get; set; }
                public byte Unknown1 { get; set; }
                public uint Unknown2 { get; set; }
                public uint Unknown3 { get; set; }
                public ulong Unknown4 { get; set; }
                public uint Unknown5 { get; set; }
                public byte Unknown6 { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();
                    Unknown1 = reader.ReadByte();
                    Unknown2 = reader.ReadUInt();
                    Unknown3 = reader.ReadUInt();
                    Unknown4 = reader.ReadULong();
                    Unknown5 = reader.ReadUInt();
                    Unknown6 = reader.ReadByte();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Unknown0);
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                    writer.Write(Unknown3);
                    writer.Write(Unknown4);
                    writer.Write(Unknown5);
                    writer.Write(Unknown6);
                }
            }

            public List<UnknownStruct142C3BF00> Unknown0 { get; set; } = new();
            public List<UnknownStruct142C3F7F0> Unknown1 { get; set; } = new();
            public byte Unknown2 { get; set; } = 128;

            public void Read(GamePacketReader reader)
            {
                uint unknown0Count = reader.ReadUInt();
                for (int i = 0; i < unknown0Count; i++)
                {
                    UnknownStruct142C3BF00 unknown0 = new();
                    unknown0.Read(reader);
                    Unknown0.Add(unknown0);
                }

                uint unknown1Count = reader.ReadUInt();
                for (int i = 0; i < unknown1Count; i++)
                {
                    UnknownStruct142C3F7F0 unknown1 = new();
                    unknown1.Read(reader);
                    Unknown1.Add(unknown1);
                }

                Unknown2 = reader.ReadByte();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write((uint)Unknown0.Count);
                foreach (UnknownStruct142C3BF00 unknown0 in Unknown0)
                    unknown0.Write(writer);

                writer.Write((uint)Unknown1.Count);
                foreach (UnknownStruct142C3F7F0 unknown1 in Unknown1)
                    unknown1.Write(writer);

                writer.Write(Unknown2);
            }
        }

        public class UnknownStruct57 : IReadable, IWritable
        {
            public uint Unknown0 { get; set; }
            public uint Unknown1 { get; set; }

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

        public class UnknownStruct58 : IReadable, IWritable
        {
            public uint Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
            public uint Unknown2 { get; set; }
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
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

        public class UnknownStruct59 : IReadable, IWritable
        {
            public uint Unknown0 { get; set; }
            public ulong Unknown1 { get; set; }
            public uint Unknown2 { get; set; }
            public uint Unknown3 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
                Unknown1 = reader.ReadULong();
                Unknown2 = reader.ReadUInt();
                Unknown3 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
            }
        }

        public class UnknownStruct60 : IReadable, IWritable
        {
            // sub_142C34E50
            public class UnknownStruct142C34E50 : IReadable, IWritable
            {
                public uint Unknown0 { get; set; }
                public uint Unknown1 { get; set; }

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

            public uint Unknown0 { get; set; }
            public ulong Unknown1 { get; set; }
            public List<UnknownStruct142C34E50> Unknown2 { get; set; } = new();

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
                Unknown1 = reader.ReadULong();

                uint unknown2Count = reader.ReadUInt();
                for (int i = 0; i < unknown2Count; i++)
                {
                    UnknownStruct142C34E50 unknown2 = new();
                    unknown2.Read(reader);
                    Unknown2.Add(unknown2);
                }
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);

                writer.Write((uint)Unknown2.Count);
                foreach (UnknownStruct142C34E50 unknown2 in Unknown2)
                    unknown2.Write(writer);
            }
        }

        public class UnknownStruct61 : IReadable, IWritable
        {
            // sub_142C187F0
            public class UnknownStruct142C187F0 : IReadable, IWritable
            {
                // sub_142C3BB80
                public class UnknownStruct142C3BB80 : IReadable, IWritable
                {
                    // sub_142C1F840
                    public class UnknownStruct142C1F840 : IReadable, IWritable
                    {
                        public uint Unknown0 { get; set; }
                        public ulong Unknown1 { get; set; }
                        public string Unknown2 { get; set; }
                        public string Unknown3 { get; set; }

                        public void Read(GamePacketReader reader)
                        {
                            Unknown0 = reader.ReadUInt();
                            Unknown1 = reader.ReadULong();
                            Unknown2 = reader.ReadString();
                            Unknown3 = reader.ReadString();
                        }

                        public void Write(GamePacketWriter writer)
                        {
                            writer.Write(Unknown0);
                            writer.Write(Unknown1);
                            writer.Write(Unknown2);
                            writer.Write(Unknown3);
                        }
                    }

                    public uint Unknown0 { get; set; }
                    public UnknownStruct142C1F840 Unknown1 { get; set; } = new();

                    public void Read(GamePacketReader reader)
                    {
                        Unknown0 = reader.ReadUInt();

                        Unknown1.Read(reader);
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        writer.Write(Unknown0);

                        Unknown1.Write(writer);
                    }
                }

                public uint Unknown0 { get; set; }
                public string Unknown1 { get; set; }
                public string Unknown2 { get; set; }
                public List<UnknownStruct142C3BB80> Unknown3 { get; set; } = new();

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();
                    Unknown1 = reader.ReadString();
                    Unknown2 = reader.ReadString();

                    uint unknown3Count = reader.ReadUInt();
                    for (int i = 0; i < unknown3Count; i++)
                    {
                        UnknownStruct142C3BB80 unknown3 = new();
                        unknown3.Read(reader);
                        Unknown3.Add(unknown3);
                    }
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Unknown0);
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);

                    writer.Write((uint)Unknown3.Count);
                    foreach (UnknownStruct142C3BB80 unknown3 in Unknown3)
                        unknown3.Write(writer);
                }
            }

            public uint Unknown0 { get; set; }
            public UnknownStruct142C187F0 Unknown1 { get; set; } = new();
            public List<UnknownStruct142C187F0> Unknown2 { get; set; } = new();

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();

                Unknown1.Read(reader);

                uint unknown2Count = reader.ReadUInt();
                for (int i = 0; i < unknown2Count; i++)
                {
                    UnknownStruct142C187F0 unknown2 = new();
                    unknown2.Read(reader);
                    Unknown2.Add(unknown2);
                }
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);

                Unknown1.Write(writer);

                writer.Write((uint)Unknown2.Count);
                foreach (UnknownStruct142C187F0 unknown2 in Unknown2)
                    unknown2.Write(writer);
            }
        }

        public class UnknownStruct62 : IReadable, IWritable
        {
            // sub_142C1FA30
            public class UnknownStruct142C1FA30 : IReadable, IWritable
            {
                public uint Unknown0 { get; set; }
                public uint Unknown1 { get; set; }
                public uint Unknown2 { get; set; }
                public uint Unknown3 { get; set; }
                public uint Unknown4 { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();
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

            public uint Unknown0 { get; set; }
            public UnknownStruct142C1FA30 Unknown1 { get; set; } = new();

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();

                Unknown1.Read(reader);
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);

                Unknown1.Write(writer);
            }
        }

        public class UnknownStruct63 : IReadable, IWritable
        {
            public uint Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
            public uint Unknown2 { get; set; }
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
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

        public class UnknownStruct64 : IReadable, IWritable
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

        public class UnknownStruct65 : IReadable, IWritable
        {
            // sub_142C25130
            public class UnknownStruct142C25130 : IReadable, IWritable
            {
                public uint Unknown0 { get; set; }
                public uint Unknown1 { get; set; }
                public ulong Unknown2 { get; set; }
                public ulong Unknown3 { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();
                    Unknown1 = reader.ReadUInt();
                    Unknown2 = reader.ReadULong();
                    Unknown3 = reader.ReadULong();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Unknown0);
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                    writer.Write(Unknown3);
                }
            }

            // sub_142C3BC60
            public class UnknownStruct142C3BC60 : IReadable, IWritable
            {
                // sub_142C25130
                public class UnknownStruct142C25130 : IReadable, IWritable
                {
                    public uint Unknown0 { get; set; }
                    public uint Unknown1 { get; set; }
                    public ulong Unknown2 { get; set; }
                    public ulong Unknown3 { get; set; }

                    public void Read(GamePacketReader reader)
                    {
                        Unknown0 = reader.ReadUInt();
                        Unknown1 = reader.ReadUInt();
                        Unknown2 = reader.ReadULong();
                        Unknown3 = reader.ReadULong();

                    }

                    public void Write(GamePacketWriter writer)
                    {
                        writer.Write(Unknown0);
                        writer.Write(Unknown1);
                        writer.Write(Unknown2);
                        writer.Write(Unknown3);
                    }
                }

                public uint Unknown0 { get; set; }
                public UnknownStruct142C25130 Unknown1 { get; set; } = new();
                public uint Unknown2 { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();

                    Unknown1.Read(reader);

                    Unknown2 = reader.ReadUInt();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Unknown0);

                    Unknown1.Write(writer);

                    writer.Write(Unknown2);
                }
            }

            // sub_142C251E0
            public class UnknownStruct142C251E0 : IReadable, IWritable
            {
                public uint Unknown0 { get; set; }
                public uint Unknown1 { get; set; }
                public ulong Unknown2 { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();
                    Unknown1 = reader.ReadUInt();
                    Unknown2 = reader.ReadULong();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Unknown0);
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                }
            }

            // sub_142C3BDD0
            public class UnknownStruct142C3BDD0 : IReadable, IWritable
            {
                // sub_142C20DD0
                public class UnknownStruct142C20DD0 : IReadable, IWritable
                {
                    // sub_142C251E0
                    public class UnknownStruct142C251E0 : IReadable, IWritable
                    {
                        public uint Unknown0 { get; set; }
                        public uint Unknown1 { get; set; }
                        public ulong Unknown2 { get; set; }

                        public void Read(GamePacketReader reader)
                        {
                            Unknown0 = reader.ReadUInt();
                            Unknown1 = reader.ReadUInt();
                            Unknown2 = reader.ReadULong();
                        }

                        public void Write(GamePacketWriter writer)
                        {
                            writer.Write(Unknown0);
                            writer.Write(Unknown1);
                            writer.Write(Unknown2);
                        }
                    }

                    public UnknownStruct142C251E0 Unknown0 { get; set; } = new();
                    public uint Unknown1 { get; set; }
                    public uint Unknown2 { get; set; }

                    public void Read(GamePacketReader reader)
                    {
                        Unknown0.Read(reader);

                        Unknown1 = reader.ReadUInt();
                        Unknown2 = reader.ReadUInt();
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        Unknown0.Write(writer);

                        writer.Write(Unknown1);
                        writer.Write(Unknown2);
                    }
                }

                public uint Unknown0 { get; set; }
                public UnknownStruct142C20DD0 Unknown1 { get; set; } = new();

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();

                    Unknown1.Read(reader);
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Unknown0);

                    Unknown1.Write(writer);
                }
            }

            public UnknownStruct142C25130 Unknown0 { get; set; } = new();
            public List<UnknownStruct142C3BC60> Unknown1 { get; set; } = new();
            public UnknownStruct142C251E0 Unknown2 { get; set; } = new();
            public List<UnknownStruct142C3BDD0> Unknown3 { get; set; } = new();
            public byte Unknown4 { get; set; } = 128;

            public void Read(GamePacketReader reader)
            {
                Unknown0.Read(reader);

                uint unknown1Count = reader.ReadUInt();
                for (int i = 0; i < unknown1Count; i++)
                {
                    UnknownStruct142C3BC60 unknown1 = new();
                    unknown1.Read(reader);
                    Unknown1.Add(unknown1);
                }

                Unknown2.Read(reader);

                uint unknown3Count = reader.ReadUInt();
                for (int i = 0; i < unknown3Count; i++)
                {
                    UnknownStruct142C3BDD0 unknown3 = new();
                    unknown3.Read(reader);
                    Unknown3.Add(unknown3);
                }

                Unknown4 = reader.ReadByte();
            }

            public void Write(GamePacketWriter writer)
            {
                Unknown0.Write(writer);
                
                writer.Write((uint)Unknown1.Count);
                foreach (UnknownStruct142C3BC60 unknown1 in Unknown1)
                    unknown1.Write(writer);

                Unknown2.Write(writer);

                writer.Write((uint)Unknown3.Count);
                foreach (UnknownStruct142C3BDD0 unknown3 in Unknown3)
                    unknown3.Write(writer);

                writer.Write(Unknown4);
            }
        }

        public class Loadout : IReadable, IWritable
        {
            // sub_142C217F0
            public class UnknownStruct142C217F0 : IReadable, IWritable
            {
                public uint Unknown0 { get; set; } = 1;
                public uint Unknown1 { get; set; } = 46;
                public string Unknown2 { get; set; } = "Default Loadout";
                public uint Unknown3 { get; set; }
                public List<uint> Unknown4 { get; set; } = new uint[4].ToList();
                public List<uint> Unknown5 { get; set; } = new uint[5].ToList();
                public List<ulong> Unknown6 { get; set; } = new ulong[16].ToList();

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();
                    Unknown1 = reader.ReadUInt();
                    Unknown2 = reader.ReadString();
                    Unknown3 = reader.ReadUInt();

                    Unknown4 = new List<uint>();
                    uint unknown4Count = reader.ReadUInt();
                    for (int i = 0; i < unknown4Count; i++)
                        Unknown4.Add(reader.ReadUInt());

                    Unknown5 = new List<uint>();
                    uint unknown5Count = reader.ReadUInt();
                    for (int i = 0; i < unknown5Count; i++)
                        Unknown5.Add(reader.ReadUInt());

                    Unknown6 = new List<ulong>();
                    uint unknown6Count = reader.ReadUInt();
                    for (int i = 0; i < unknown6Count; i++)
                        Unknown6.Add(reader.ReadULong());
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Unknown0);
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                    writer.Write(Unknown3);

                    writer.Write((uint)Unknown4.Count);
                    foreach (uint unknown4 in Unknown4)
                        writer.Write(unknown4);

                    writer.Write((uint)Unknown5.Count);
                    foreach (uint unknown5 in Unknown5)
                        writer.Write(unknown5);

                    writer.Write((uint)Unknown6.Count);
                    foreach (ulong unknown6 in Unknown6)
                        writer.Write(unknown6);
                }
            }

            public uint Unknown0 { get; set; } = 1;
            public UnknownStruct142C217F0 Unknown1 { get; set; } = new();

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();

                Unknown1.Read(reader);
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);

                Unknown1.Write(writer);
            }
        }

        public class UnknownStruct67 : IReadable, IWritable
        {
            // These fields are all objects in the packet, but unused data in packet parses, so lazily written.
            // TODO: Update this Struct with all inner objects

            public uint Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
            public uint Unknown2 { get; set; }
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }

            public void Read(GamePacketReader reader)
            {
                uint unknown0Count = reader.ReadUInt();
                Unknown0 = unknown0Count;
                for (int i = 0; i < unknown0Count; i++)
                {
                    // TODO: Do This
                    throw new NotImplementedException();
                }

                uint unknown1Count = reader.ReadUInt();
                Unknown1 = unknown1Count;
                for (int i = 0; i < unknown1Count; i++)
                {
                    // TODO: Do This
                    throw new NotImplementedException();
                }

                uint unknown2Count = reader.ReadUInt();
                Unknown2 = unknown2Count;
                for (int i = 0; i < unknown2Count; i++)
                {
                    // TODO: Do This
                    throw new NotImplementedException();
                }

                uint unknown3Count = reader.ReadUInt();
                Unknown3 = unknown3Count;
                for (int i = 0; i < unknown3Count; i++)
                {
                    // TODO: Do This
                    throw new NotImplementedException();
                }

                uint unknown4Count = reader.ReadUInt();
                Unknown4 = unknown4Count;
                for (int i = 0; i < unknown4Count; i++)
                {
                    // TODO: Do This
                    throw new NotImplementedException();
                }
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                if (Unknown0 != 0u)
                    throw new NotImplementedException();

                writer.Write(Unknown1);
                if (Unknown1 != 0u)
                    throw new NotImplementedException();

                writer.Write(Unknown2);
                if (Unknown2 != 0u)
                    throw new NotImplementedException();

                writer.Write(Unknown3);
                if (Unknown3 != 0u)
                    throw new NotImplementedException();

                writer.Write(Unknown4);
                if (Unknown4 != 0u)
                    throw new NotImplementedException();
            }
        }

        public class UnknownStruct68 : IReadable, IWritable
        {
            public ulong Unknown0 { get; set; }
            public ulong Unknown1 { get; set; }
            public ulong Unknown2 { get; set; }
            public ulong Unknown3 { get; set; } = 1487581831;
            public uint Unknown4 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadULong();
                Unknown1 = reader.ReadULong();
                Unknown2 = reader.ReadULong();
                Unknown3 = reader.ReadULong();
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

        public class UnknownStruct76 : IReadable, IWritable
        {
            // sub_142C37990
            public class UnknownStruct142C37990 : IReadable, IWritable
            {
                public uint Unknown0 { get; set; }
                public uint Unknown1 { get; set; }
                public string Unknown2 { get; set; }
                public string Unknown3 { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadUInt();
                    Unknown1 = reader.ReadUInt();
                    Unknown2 = reader.ReadString();
                    Unknown3 = reader.ReadString();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Unknown0);
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                    writer.Write(Unknown3);
                }
            }

            public List<UnknownStruct142C37990> Unknown0 { get; set; } = new();
            public uint Unknown1 { get; set; }

            public void Read(GamePacketReader reader)
            {
                uint unknown0Count = reader.ReadUInt();
                for (int i = 0; i < unknown0Count; i++)
                {
                    UnknownStruct142C37990 unknown0 = new();
                    unknown0.Read(reader);
                    Unknown0.Add(unknown0);
                }

                Unknown1 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write((uint)Unknown0.Count);
                foreach (UnknownStruct142C37990 unknown0 in Unknown0)
                    unknown0.Write(writer);

                writer.Write(Unknown1);
            }
        }

        public uint Unknown0 { get; set; } = 655356;
        public ulong Guid { get; set; } = 1000000;
        public string AccountName { get; set; } = "TestAccount";
        public ulong CharacterId { get; set; }
        public uint Unknown1 { get; set; }
        public ulong LastLoginTimeRaw { get; set; }
        public DateTime LastLoginTime { get; set; } // Epoch Timestamp of last login
        public uint ModelId { get; set; }
        public CharacterModelInfo Model { get; set; } = new();
        public Vector4 Unknown3 { get; set; } // Probably Position
        public Vector4 Unknown4 { get; set; } // Probably Rotation
        public CharacterIdentity Identity { get; set; } = new CharacterIdentity();
        public uint Unknown5 { get; set; }
        public List<(uint, uint)> Unknown6 { get; set; } = new();
        public ulong Unknown7 { get; set; }
        public uint Unknown8 { get; set; }
        public uint Unknown9 { get; set; } // Same packet data appears in TemplateBase, but could be coincidence
        public bool Unknown10 { get; set; }
        public bool Unknown11 { get; set; }
        public bool Unknown12 { get; set; }
        public uint Unknown13 { get; set; }
        public bool Unknown14 { get; set; }
        public bool Unknown15 { get; set; }
        public uint Unknown16 { get; set; }
        public uint Unknown17 { get; set; }
        public uint Unknown18 { get; set; }
        public uint Unknown19 { get; set; }
        public uint Unknown20 { get; set; }
        public ulong Unknown21 { get; set; }
        public uint Unknown22 { get; set; }
        public bool Unknown23 { get; set; }
        public List<(uint, uint, uint)> Unknown24 { get; set; } = new();
        public bool Unknown25 { get; set; }
        public bool Unknown26 { get; set; }
        public bool Unknown27 { get; set; }
        public bool Unknown28 { get; set; }
        public List<Profile> Profiles { get; set; } = new();
        public uint CurrentProfile { get; set; }
        public List<(uint, uint)> Unknown30 { get; set; } = new();
        public List<Collection> Collections { get; set; } = new();
        public List<(uint, uint, uint)> Unknown31 { get; set; } = new();
        public uint Unknown32 { get; set; }
        public List<Buff> Buffs { get; set; } = new();
        public InventoryData Inventory { get; set; } = new();
        public uint Unknown35 { get; set; }
        public Gender GenderMaybe { get; set; } // Probably Gender
        public QuestData Quests { get; set; } = new();
        public List<Achievement> Achievements { get; set; } = new();
        public uint Acquaintances { get; set; }
        public RecipeData Recipes { get; set; } = new();
        public List<uint> Unknown40 { get; set; } = new();
        public List<uint> Unknown41 { get; set; } = new();
        public List<uint> Unknown42 { get; set; } = new();
        public List<Hotbar> Hotbars { get; set; } = new();
        public List<Mount> Mounts { get; set; } = new();
        public bool SendFirstTimeEvents { get; set; }
        public List<uint> Unknown46 { get; set; } = new();
        public List<uint> Unknown47 { get; set; } = new();
        public List<(uint, StatData)> Stats { get; set; } = new();
        public TitleData Titles { get; set; } = new();
        public List<UnknownStruct50> Unknown50 { get; set; } = new();
        public List<uint> Unknown51 { get; set; } = new();
        public uint Unknown52 { get; set; }
        public List<uint> Unknown53 { get; set; } = new();
        public List<uint> Unknown54 { get; set; } = new();
        public List<bool> Unknown55 { get; set; } = new();
        public UnknownStruct56 Unknown56 { get; set; } = new();
        public List<UnknownStruct57> Unknown57 { get; set; } = new();
        public UnknownStruct58 Unknown58 { get; set; } = new();
        public List<UnknownStruct59> Unknown59 { get; set; } = new();
        public List<UnknownStruct60> Unknown60 { get; set; } = new();
        public List<UnknownStruct61> Unknown61 { get; set; } = new();
        public List<UnknownStruct62> Unknown62 { get; set; } = new();
        public UnknownStruct63 Unknown63 { get; set; } = new();
        public UnknownStruct64 Unknown64 { get; set; } = new();
        public UnknownStruct65 Unknown65 { get; set; } = new();
        public List<Loadout> Loadouts { get; set; } = new List<Loadout>
        {
            new Loadout()
        };
        public UnknownStruct67 Unknown67 { get; set; } = new();
        public UnknownStruct68 Unknown68 { get; set; } = new();
        public byte Unknown69 { get; set; }
        public byte Unknown70 { get; set; }
        public byte Unknown71 { get; set; }
        public byte Unknown72 { get; set; }
        public byte Unknown73 { get; set; }
        public byte Unknown74 { get; set; }
        public bool IsAdminMaybe { get; set; }
        public UnknownStruct76 Unknown76 { get; set; } = new();
        public string Unknown77 { get; set; } = "";

        public void Read(GamePacketReader reader)
        {
            Unknown0 = reader.ReadUInt();
            Guid = reader.ReadULong();
            AccountName = reader.ReadString();
            CharacterId = reader.ReadULong();
            Unknown1 = reader.ReadUInt2BitLength();
            LastLoginTimeRaw = reader.ReadULong();
            LastLoginTime = DateTimeOffset.FromUnixTimeSeconds((long)LastLoginTimeRaw).DateTime;
            ModelId = reader.ReadUInt();

            Model.Read(reader);

            Unknown3 = new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Unknown4 = new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            Identity.Read(reader);

            Unknown5 = reader.ReadUInt();

            var unknown6Count = reader.ReadUInt();
            for (int i = 0; i < unknown6Count; i++)
                Unknown6.Add((reader.ReadUInt(), reader.ReadUInt()));

            Unknown7 = reader.ReadULong();
            Unknown8 = reader.ReadUInt();
            Unknown9 = reader.ReadUInt();
            Unknown10 = reader.ReadBool();
            Unknown11 = reader.ReadBool();
            Unknown12 = reader.ReadBool();
            Unknown13 = reader.ReadUInt();
            Unknown14 = reader.ReadBool();
            Unknown15 = reader.ReadBool();
            Unknown16 = reader.ReadUInt();
            Unknown17 = reader.ReadUInt();
            Unknown18 = reader.ReadUInt();
            Unknown19 = reader.ReadUInt();
            Unknown20 = reader.ReadUInt();
            Unknown21 = reader.ReadULong();
            Unknown22 = reader.ReadUInt();
            Unknown23 = reader.ReadBool();

            var unknown24Count = reader.ReadUInt();
            for (int i = 0; i < unknown24Count; i++)
                Unknown24.Add((reader.ReadUInt(), reader.ReadUInt(), reader.ReadUInt()));

            Unknown25 = reader.ReadBool();
            Unknown26 = reader.ReadBool();
            Unknown27 = reader.ReadBool();
            Unknown28 = reader.ReadBool();

            uint struct142C1AC40Count = reader.ReadUInt();
            for (int i = 0; i < struct142C1AC40Count; i++)
            {
                Profile obj = new();
                obj.Read(reader);
                Profiles.Add(obj);
            }

            CurrentProfile = reader.ReadUInt();

            uint unknown30Count = reader.ReadUInt();
            for (int i = 0; i < unknown30Count; i++)
                Unknown30.Add((reader.ReadUInt(), reader.ReadUInt()));

            uint collectionCount = reader.ReadUInt();
            for (int i = 0; i < collectionCount; i++)
            {
                Collection collection = new();
                collection.Read(reader);
                Collections.Add(collection);
            }

            var unknown31Count = reader.ReadUInt();
            for (int i = 0; i < unknown31Count; i++)
                Unknown31.Add((reader.ReadUInt(), reader.ReadUInt(), reader.ReadUInt()));

            Unknown32 = reader.ReadUInt();

            uint buffCount = reader.ReadUInt();
            for (int i = 0; i < buffCount; i++)
            {
                Buff buff = new();
                buff.Read(reader);
                Buffs.Add(buff);
            }

            Inventory.Read(reader);

            uint unknown34Count = reader.ReadUInt();
            Unknown35 = unknown34Count;
            for (int i = 0; i < unknown34Count; i++)
            {
                // TODO: Do This
                throw new NotImplementedException();
            }

            GenderMaybe = (Gender)reader.ReadUInt();

            Quests.Read(reader);

            uint achievementsCount = reader.ReadUInt();
            for (int i = 0; i < achievementsCount; i++)
            {
                Achievement achievement = new();
                achievement.Read(reader);
                Achievements.Add(achievement);
            }

            uint acquaintencesCount = reader.ReadUInt();
            Acquaintances = acquaintencesCount;
            for (int i = 0; i < acquaintencesCount; i++)
            {
                // TODO: Do This
                throw new NotImplementedException();
            }

            Recipes.Read(reader);

            uint unknown40Count = reader.ReadUInt();
            for (int i = 0; i < unknown40Count; i++)
                Unknown40.Add(reader.ReadUInt());

            uint unknown41Count = reader.ReadUInt();
            for (int i = 0; i < unknown41Count; i++)
                Unknown41.Add(reader.ReadUInt());

            uint unknown42Count = reader.ReadUInt();
            for (int i = 0; i < unknown42Count; i++)
                Unknown42.Add(reader.ReadUInt());

            uint unknown43Count = reader.ReadUInt();
            for (int i = 0; i < unknown43Count; i++)
            {
                Hotbar unknown43 = new();
                unknown43.Read(reader);
                Hotbars.Add(unknown43);
            }

            uint mountsCount = reader.ReadUInt();
            for (int i = 0; i < mountsCount; i++)
            {
                Mount mount= new();
                mount.Read(reader);
                Mounts.Add(mount);
            }

            SendFirstTimeEvents = reader.ReadBool();

            uint unknown46Count = reader.ReadUInt();
            for (int i = 0; i < unknown46Count; i++)
                Unknown46.Add(reader.ReadUInt());

            uint unknown47Count = reader.ReadUInt();
            for (int i = 0; i < unknown47Count; i++)
                Unknown47.Add(reader.ReadUInt());

            uint statsCount = reader.ReadUInt();
            for (int i = 0; i < statsCount; i++)
            {
                uint statId = reader.ReadUInt();
                StatData statData = new();
                statData.Read(reader);
                Stats.Add((statId, statData));
            }

            Titles.Read(reader);

            uint unknown50Count = reader.ReadUInt();
            for (int i = 0; i < unknown50Count; i++)
            {
                UnknownStruct50 unknown50 = new();
                unknown50.Read(reader);
                Unknown50.Add(unknown50);
            }

            uint unknown51Count = reader.ReadUInt();
            for (int i = 0; i < unknown51Count; i++)
                Unknown51.Add(reader.ReadUInt());

            Unknown52 = reader.ReadUInt();

            uint unknown53Count = reader.ReadUInt();
            for (int i = 0; i < unknown53Count; i++)
                Unknown53.Add(reader.ReadUInt());

            uint unknown54Count = reader.ReadUInt();
            for (int i = 0; i < unknown54Count; i++)
                Unknown54.Add(reader.ReadUInt());

            uint unknown55Count = reader.ReadUInt();
            for (int i = 0; i < unknown55Count; i++)
                Unknown55.Add(reader.ReadBool());

            Unknown56.Read(reader);

            uint unknown57Count = reader.ReadUInt();
            for (int i = 0; i < unknown57Count; i++)
            {
                UnknownStruct57 unknown57 = new();
                unknown57.Read(reader);
                Unknown57.Add(unknown57);
            }

            Unknown58.Read(reader);

            uint unknown59Count = reader.ReadUInt();
            for (int i = 0; i < unknown59Count; i++)
            {
                UnknownStruct59 unknown59 = new();
                unknown59.Read(reader);
                Unknown59.Add(unknown59);
            }

            uint unknown60Count = reader.ReadUInt();
            for (int i = 0; i < unknown60Count; i++)
            {
                UnknownStruct60 unknown60 = new();
                unknown60.Read(reader);
                Unknown60.Add(unknown60);
            }

            uint unknown61Count = reader.ReadUInt();
            for (int i = 0; i < unknown61Count; i++)
            {
                UnknownStruct61 unknown61 = new();
                unknown61.Read(reader);
                Unknown61.Add(unknown61);
            }

            uint unknown62Count = reader.ReadUInt();
            for (int i = 0; i < unknown62Count; i++)
            {
                UnknownStruct62 unknown62 = new();
                unknown62.Read(reader);
                Unknown62.Add(unknown62);
            }

            Unknown63.Read(reader);

            Unknown64.Read(reader);

            Unknown65.Read(reader);

            uint loadoutsCount = reader.ReadUInt();
            for (int i = 0; i < loadoutsCount; i++)
            {
                Loadout loadout = new();
                loadout.Read(reader);
                Loadouts.Add(loadout);
            }

            Unknown67.Read(reader);

            Unknown68.Read(reader);

            Unknown69 = reader.ReadByte();
            Unknown70 = reader.ReadByte();
            Unknown71 = reader.ReadByte();
            Unknown72 = reader.ReadByte();
            Unknown73 = reader.ReadByte();
            Unknown74 = reader.ReadByte();
            IsAdminMaybe = reader.ReadBool();

            Unknown76.Read(reader);

            Unknown77 = reader.ReadString();

            // There is 34 bytes of data left in the packet which doesn't appear to be read by the client.
            // 00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-ff-ff-ff-ff-00-00-00-00-00-01-00-00-00-00-00-00-00
            reader.ReadBytes(34u);

            log.Info($"\n\n   {reader.TotalBytes - reader.BytesRemaining} / {reader.TotalBytes} ({((float)(reader.TotalBytes - reader.BytesRemaining) / (float)reader.TotalBytes) * 100f}%) Bytes Read\n");
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Guid);
            writer.Write(AccountName);
            writer.Write(CharacterId);
            writer.WriteUIntWith2BitLength((int)Unknown1);
            writer.Write(LastLoginTimeRaw); // TODO: Figure out how to write Timestamp

            writer.Write(ModelId);
            Model.Write(writer);

            writer.Write(Unknown3);
            writer.Write(Unknown4);

            Identity.Write(writer);

            writer.Write(Unknown5);

            writer.Write((uint)Unknown6.Count);
            foreach (var item in Unknown6)
            {
                writer.Write(item.Item1);
                writer.Write(item.Item2);
            }

            writer.Write(Unknown7);
            writer.Write(Unknown8);
            writer.Write(Unknown9);
            writer.Write(Unknown10);
            writer.Write(Unknown11);
            writer.Write(Unknown12);
            writer.Write(Unknown13);
            writer.Write(Unknown14);
            writer.Write(Unknown15);
            writer.Write(Unknown16);
            writer.Write(Unknown17);
            writer.Write(Unknown18);
            writer.Write(Unknown19);
            writer.Write(Unknown20);
            writer.Write(Unknown21);
            writer.Write(Unknown22);
            writer.Write(Unknown23);

            writer.Write((uint)Unknown24.Count);
            foreach (var item in Unknown24)
            {
                writer.Write(item.Item1);
                writer.Write(item.Item2);
                writer.Write(item.Item3);
            }

            writer.Write(Unknown25);
            writer.Write(Unknown26);
            writer.Write(Unknown27);
            writer.Write(Unknown28);

            writer.Write((uint)Profiles.Count);
            foreach (Profile profile in Profiles)
                profile.Write(writer);
            writer.Write(CurrentProfile);

            writer.Write((uint)Unknown30.Count);
            foreach (var item in Unknown30)
            {
                writer.Write(item.Item1);
                writer.Write(item.Item2);
            }

            writer.Write((uint)Collections.Count);
            foreach (Collection collection in Collections)
                collection.Write(writer);

            writer.Write((uint)Unknown31.Count);
            foreach (var item in Unknown31)
            {
                writer.Write(item.Item1);
                writer.Write(item.Item2);
                writer.Write(item.Item3);
            }

            writer.Write(Unknown32);

            writer.Write((uint)Buffs.Count);
            foreach (Buff buff in Buffs)
                buff.Write(writer);

            Inventory.Write(writer);

            writer.Write((uint)Unknown35);
            for (int i = 0; i < Unknown35; i++)
            {
                // TODO: Do This
                throw new NotImplementedException();
            }

            writer.Write((uint)GenderMaybe);

            Quests.Write(writer);

            writer.Write((uint)Achievements.Count);
            foreach (Achievement achievement in Achievements)
                achievement.Write(writer);

            writer.Write(Acquaintances);
            for (int i = 0; i < Acquaintances; i++)
            {
                // TODO: Do This
                throw new NotImplementedException();
            }

            Recipes.Write(writer);

            writer.Write((uint)Unknown40.Count);
            foreach (var item in Unknown40)
                writer.Write(item);

            writer.Write((uint)Unknown41.Count);
            foreach (var item in Unknown41)
                writer.Write(item);

            writer.Write((uint)Unknown42.Count);
            foreach (var item in Unknown42)
                writer.Write(item);

            writer.Write((uint)Hotbars.Count);
            foreach (Hotbar hotbar in Hotbars)
                hotbar.Write(writer);

            writer.Write((uint)Mounts.Count);
            foreach (Mount mount in Mounts)
                mount.Write(writer);

            writer.Write(SendFirstTimeEvents);

            writer.Write((uint)Unknown46.Count);
            foreach (var item in Unknown46)
                writer.Write(item);

            writer.Write((uint)Unknown47.Count);
            foreach (var item in Unknown47)
                writer.Write(item);

            writer.Write((uint)Stats.Count);
            foreach (var stat in Stats)
            {
                writer.Write(stat.Item1);
                stat.Item2.Write(writer);
            }

            Titles.Write(writer);

            writer.Write((uint)Unknown50.Count);
            foreach (var unknownStruct50 in Unknown50)
                unknownStruct50.Write(writer);

            writer.Write((uint)Unknown51.Count);
            foreach (var item in Unknown51)
                writer.Write(item);

            writer.Write(Unknown52);

            writer.Write((uint)Unknown53.Count);
            foreach (var item in Unknown53)
                writer.Write(item);

            writer.Write((uint)Unknown54.Count);
            foreach (var item in Unknown54)
                writer.Write(item);

            writer.Write((uint)Unknown55.Count);
            foreach (var item in Unknown55)
                writer.Write(item);

            Unknown56.Write(writer);

            writer.Write((uint)Unknown57.Count);
            foreach (var unknownStruct57 in Unknown57)
                unknownStruct57.Write(writer);

            Unknown58.Write(writer);

            writer.Write((uint)Unknown59.Count);
            foreach (var unknownStruct59 in Unknown59)
                unknownStruct59.Write(writer);

            writer.Write((uint)Unknown60.Count);
            foreach (var unknownStruct60 in Unknown60)
                unknownStruct60.Write(writer);

            writer.Write((uint)Unknown61.Count);
            foreach (var unknownStruct61 in Unknown61)
                unknownStruct61.Write(writer);

            writer.Write((uint)Unknown62.Count);
            foreach (var unknownStruct62 in Unknown62)
                unknownStruct62.Write(writer);

            Unknown63.Write(writer);

            Unknown64.Write(writer);

            Unknown65.Write(writer);

            writer.Write((uint)Loadouts.Count);
            foreach (Loadout loadout in Loadouts)
                loadout.Write(writer);

            Unknown67.Write(writer);

            Unknown68.Write(writer);

            writer.Write(Unknown69);
            writer.Write(Unknown70);
            writer.Write(Unknown71);
            writer.Write(Unknown72);
            writer.Write(Unknown73);
            writer.Write(Unknown74);
            writer.Write(IsAdminMaybe);

            Unknown76.Write(writer);

            writer.Write(Unknown77);

            // There is 34 bytes of data left in the packet which doesn't appear to be read by the client.
            // 00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-ff-ff-ff-ff-00-00-00-00-00-01-00-00-00-00-00-00-00
            writer.WriteBytes(Convert.FromHexString("00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-ff-ff-ff-ff-00-00-00-00-00-01-00-00-00-00-00-00-00".Replace("-", "")));
        }
    }
}
