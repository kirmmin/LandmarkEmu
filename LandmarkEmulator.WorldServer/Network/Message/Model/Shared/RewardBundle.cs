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
                throw new NotImplementedException();
            }
        }

        public class BundleRewardEntry : IReadable, IWritable
        {
            public uint Type { get; set; }

            public void Read(GamePacketReader reader)
            {
                Type = reader.ReadUInt();
                if (Type == 1)
                {
                    // TODO: Seems to always be 42 bytes when Types i1 but what if type is different?!
                    reader.ReadBytes(42u);
                }
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
            throw new NotImplementedException();
        }
    }
}
