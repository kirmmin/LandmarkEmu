using LandmarkEmulator.Shared.Network.Message;
using static LandmarkEmulator.Parser.Parser;

namespace LandmarkEmulator.Parser
{
    class ParserPacketOptions : PacketOptions
    {
        public bool IsClient { get; set; }
        public GamePacketType GamePacketType { get; set; }
    }
}
