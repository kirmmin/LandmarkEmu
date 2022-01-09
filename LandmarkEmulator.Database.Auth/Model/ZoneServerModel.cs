namespace LandmarkEmulator.Database.Auth.Model
{
    public class ZoneServerModel
    {
        public ulong Id { get; set; }
        public uint NameId { get; set; }
        public string Host { get; set; }
        public ushort Port { get; set; }
        public uint Flags { get; set; }
    }
}
