using LandmarkEmulator.Shared.GameTable.Text;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.Shared.Game
{
    public class LandmarkText : IReadable, IWritable
    {
        public uint Id { get; set; }
        public string Text { get; set; }

        public LandmarkText()
        {

        }

        public LandmarkText(uint id)
        {
            Id   = id;
            SetText();
        }

        private void SetText()
        {
            Text = TextManager.Instance.GetTextForId(Id);
        }

        public void Read(GamePacketReader reader)
        {
            Id = reader.ReadUInt();
            SetText();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Id);
        }
    }
}
