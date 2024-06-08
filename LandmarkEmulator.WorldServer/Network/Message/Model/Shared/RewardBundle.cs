using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;
using System.Collections.Generic;
using NLog;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkEmulator.WorldServer.Network.Message.Model.Shared
{
    public class RewardBundle : IReadable, IWritable
    {
        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public class Currency : IReadable, IWritable
        {
            public uint CurrencyId { get; set; }
            public uint Quantity { get; set; }

            public void Read(GamePacketReader reader)
            {
                CurrencyId = reader.ReadUInt();
                Quantity = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(CurrencyId);
                writer.Write(Quantity);
            }
        }

        public class BundleRewardEntry : IReadable, IWritable
        {
            public uint Type { get; set; }
            public byte[] Data { get; set; }

            public void Read(GamePacketReader reader)
            {
                Type = reader.ReadUInt();
                if (Type == 1)
                {
                    // TODO: Seems to always be 42 bytes when Type's 1 but what if type is different?!
                    Data = reader.ReadBytes(42u);
                }
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Type);
                if (Type == 1)
                {
                    // TODO: Handle the 42 bytes discovered in packets.
                    writer.WriteBytes(Data);
                }
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
        public ulong CharacterId { get; set; }
        public uint Unknown13 { get; set; }
        public uint Unknown14 { get; set; }
        public uint BundleRewardEntryCount { get; set; }
        public List<BundleRewardEntry> BundleRewardEntries { get; set; } = new(); // TODO: Write out BundleRewardEntries reader
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
            CharacterId = reader.ReadULong();
            Unknown13 = reader.ReadUInt();
            Unknown14 = reader.ReadUInt();

            uint bundleRewardEntryCount = reader.ReadUInt();
            BundleRewardEntryCount = bundleRewardEntryCount;
            for (int i = 0; i < BundleRewardEntryCount; i++)
            {
                BundleRewardEntry rewardEntry = new();
                rewardEntry.Read(reader);
                BundleRewardEntries.Add(rewardEntry);
            }

            Unknown16 = reader.ReadUInt();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Unknown0);
            writer.Write(Unknown1);

            writer.Write((uint)Currencies.Count);
            foreach (Currency currency in Currencies)
                currency.Write(writer);

            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
            writer.Write(Unknown7);
            writer.Write(Unknown8);
            writer.Write(Unknown9);
            writer.Write(Unknown10);
            writer.Write(Unknown11);
            writer.Write(CharacterId);
            writer.Write(Unknown13);
            writer.Write(Unknown14);

            writer.Write((uint)BundleRewardEntries.Count);
            foreach (BundleRewardEntry bundleRewardEntry in BundleRewardEntries)
                bundleRewardEntry.Write(writer);

            writer.Write(Unknown16);
        }
    }
}
