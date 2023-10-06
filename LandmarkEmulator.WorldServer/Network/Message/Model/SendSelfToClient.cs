using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using LandmarkEmulator.Shared.Network.Message.Model.Shared;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.SendSelfToClient)]
    public class SendSelfToClient : IReadable, IWritable
    {
        public class CharacterIdentity : IReadable, IWritable
        {
            public uint Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
            public uint Unknown2 { get; set; }
            public string FirstName { get; set; }
            public string Unknown3 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
                Unknown1 = reader.ReadUInt();
                Unknown2 = reader.ReadUInt();
                FirstName = reader.ReadString();
                Unknown3 = reader.ReadString();
            }

            public void Write(GamePacketWriter writer)
            {
                throw new System.NotImplementedException();
            }
        }

        // Seems replicated here, or vey close at least: https://github.com/H1emu/h1z1-server/blob/master/src/packets/ClientProtocol/ClientProtocol_860/shared.ts#L1449-L1484
        public class Profile : IReadable, IWritable
        {
            public uint Unknown0 { get; set; }
            public uint Unknown1 { get; set; }
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

            public uint Unknown19 { get; set; }
            public uint Unknown20 { get; set; }
            public uint Unknown21 { get; set; }
            public uint Unknown22 { get; set; }
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
            public uint Unknown33 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
                Unknown1 = reader.ReadUInt();
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
                throw new NotImplementedException();
            }
        }

        public class Collection : IReadable, IWritable
        {
            public class RewardBundle : IReadable, IWritable
            {
                public class Currency : IReadable, IWritable
                {
                    public uint CurrencyId { get; set; }
                    public uint Amount { get; set; }

                    public void Read(GamePacketReader reader)
                    {
                        CurrencyId = reader.ReadUInt();
                        Amount = reader.ReadUInt();
                    }

                    public void Write(GamePacketWriter writer)
                    {
                        throw new NotImplementedException();
                    }
                }

                public bool Unknown0 { get; set; }
                public uint Unknown1 { get; set; }
                public List<Currency> Currencies { get; set; } = new();
                public uint Unknown2 { get; set; }
                public uint Unknown3 { get; set; }
                public uint Unknown4 { get; set; }
                public ulong Unknown5 { get; set; }
                public uint Unknown6 { get; set; }
                public uint Unknown7 { get; set; }
                public uint Unknown8 { get; set; }
                public uint Unknown9 { get; set; }
                public uint Unknown10 { get; set; }
                public ulong Unknown11 { get; set; }
                public ulong Unknown12 { get; set; }
                public uint Unknown13 { get; set; }
                public uint Unknown14 { get; set; }
                public uint BundleRewardEntryCount { get; set; }
                public List<uint> BundleRewardEntries { get; set; } = new(); // TODO: Write out BundleRewardEntries reader
                public uint Unknown16 { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadBool();
                    Unknown1 = reader.ReadUInt();

                    uint currencyCount = reader.ReadUInt();
                    for (int i = 0; i < currencyCount; i++)
                    {
                        Currency currency = new();
                        currency.Read(reader);
                        Currencies.Add(currency);
                    }

                    Unknown2 = reader.ReadUInt();
                    Unknown3 = reader.ReadUInt();
                    Unknown4 = reader.ReadUInt();
                    Unknown5 = reader.ReadULong();
                    Unknown6 = reader.ReadUInt();
                    Unknown7 = reader.ReadUInt();
                    Unknown8 = reader.ReadUInt();
                    Unknown9 = reader.ReadUInt();
                    Unknown10 = reader.ReadUInt();
                    Unknown11 = reader.ReadULong();
                    Unknown12 = reader.ReadULong();
                    Unknown13 = reader.ReadUInt();
                    Unknown14 = reader.ReadUInt();

                    uint bundleRewardEntryCount = reader.ReadUInt();
                    BundleRewardEntryCount = bundleRewardEntryCount;
                    for (int i = 0; i < BundleRewardEntryCount; i++)
                        BundleRewardEntries.Add(reader.ReadUInt());

                    Unknown16 = reader.ReadUInt();
                }

                public void Write(GamePacketWriter writer)
                {
                    throw new NotImplementedException();
                }
            }

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
                throw new NotImplementedException();
            }
        }

        public class StructUnknown33 : IReadable, IWritable
        {
            public ulong Unknown0 { get; set; }
            public ulong Unknown1 { get; set; }
            public uint Unknown2 { get; set; }
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }
            public uint Unknown5 { get; set; }
            public uint Unknown6 { get; set; }
            public uint Unknown7 { get; set; }
            public uint Unknown8 { get; set; }
            public ulong Unknown9 { get; set; }
            public ulong Unknown10 { get; set; }
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
            public uint Unknown22 { get; set; }
            public uint Unknown23 { get; set; }
            public uint Unknown24 { get; set; }
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
                Unknown1 = reader.ReadULong();
                Unknown2 = reader.ReadUInt();
                Unknown3 = reader.ReadUInt();
                Unknown4 = reader.ReadUInt();
                Unknown5 = reader.ReadUInt();
                Unknown6 = reader.ReadUInt();
                Unknown7 = reader.ReadUInt();
                Unknown8 = reader.ReadUInt();
                Unknown9 = reader.ReadULong();
                Unknown10 = reader.ReadULong();
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

                Unknown22 = reader.ReadUInt();
                Unknown23 = reader.ReadUInt();
                Unknown24 = reader.ReadUInt();
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
                throw new NotImplementedException();
            }
        }

        public class StructUnknown34 : IReadable, IWritable
        {
            public class StructUnknown34Item : IReadable, IWritable
            {
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
                        throw new NotImplementedException();
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
                        throw new NotImplementedException();
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
                        throw new NotImplementedException();
                    }
                }

                public ulong Unknown0 { get; set; }
                public uint Unknown1 { get; set; }
                public uint Unknown2 { get; set; }
                public uint Unknown3 { get; set; }
                public byte Unknown4 { get; set; }
                public uint Unknown5 { get; set; }
                public ulong Unknown6 { get; set; }
                public uint Unknown7 { get; set; }
                public uint Unknown8 { get; set; }
                public ulong Unknown9 { get; set; }
                public ulong Unknown10 { get; set; }
                public uint Unknown11 { get; set; }
                public uint Unknown12 { get; set; }
                public byte Unknown13 { get; set; }
                public uint Unknown14 { get; set; }
                public byte subData1Flag { get; set; }
                public SubData SubDataItem { get; set; } = new();
                public SubData2 SubData2Item { get; set; } = new();
                public SubData3 SubData3Item { get; set; } = new();
                public uint Unknown15 { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Unknown0 = reader.ReadULong();
                    Unknown1 = reader.ReadUInt();
                    Unknown2 = reader.ReadUInt();
                    Unknown3 = reader.ReadUInt();
                    Unknown4 = reader.ReadByte();
                    Unknown5 = reader.ReadUInt();
                    Unknown6 = reader.ReadULong();
                    Unknown7 = reader.ReadUInt();
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
                        SubDataItem.Read(reader);

                    byte hasSubData2 = reader.ReadByte();
                    if (hasSubData2 != 0)
                        SubData2Item.Read(reader);

                    SubData3Item.Read(reader);

                    Unknown15 = reader.ReadUInt();

                    reader.ReadBytes(53u);
                }

                public void Write(GamePacketWriter writer)
                {
                    throw new NotImplementedException();
                }
            }

            public List<(uint, bool)> Unknown0 { get; set; } = new();
            public uint ItemCount { get; set; }
            public List<StructUnknown34Item> Items { get; set; } = new();

            public void Read(GamePacketReader reader)
            {
                for (int i = 0; i < 12; i++)
                    Unknown0.Add((reader.ReadUInt(), reader.ReadBool()));

                uint itemCount = reader.ReadUInt();
                ItemCount = itemCount;
                for (int i = 0; i < 20; i++)
                {
                    StructUnknown34Item structUnknown34Item = new();
                    structUnknown34Item.Read(reader);
                    Items.Add(structUnknown34Item);
                }
            }

            public void Write(GamePacketWriter writer)
            {
                throw new NotImplementedException();
            }
        }

        public uint Unknown0 { get; set; }
        public ulong Guid { get; set; }
        public string AccountName { get; set; }
        public ulong CharacterId { get; set; }
        public uint Unknown1 { get; set; }
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
        public uint CurrentProfile { get; set; } // Probably ProfileId in use
        public List<(uint, uint)> Unknown30 { get; set; } = new();
        public List<Collection> Collections { get; set; } = new();
        public List<(uint, uint, uint)> Unknown31 { get; set; } = new();
        public uint Unknown32 { get; set; }
        public List<StructUnknown33> Unknown33 { get; set; } = new();
        public StructUnknown34 Unknown34 { get; set; } = new();

        public void Read(GamePacketReader reader)
        {
            Unknown0 = reader.ReadUInt();
            Guid = reader.ReadULong();
            AccountName = reader.ReadString();
            CharacterId = reader.ReadULong();
            Unknown1 = reader.ReadUInt2BitLength();
            LastLoginTime = DateTimeOffset.FromUnixTimeSeconds((long)reader.ReadULong()).DateTime;
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

            uint unknown33Count = reader.ReadUInt();
            for (int i = 0; i < unknown33Count; i++)
            {
                StructUnknown33 unknown33 = new();
                unknown33.Read(reader);
                Unknown33.Add(unknown33);
            }

            Unknown34.Read(reader);
        }

        public void Write(GamePacketWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
