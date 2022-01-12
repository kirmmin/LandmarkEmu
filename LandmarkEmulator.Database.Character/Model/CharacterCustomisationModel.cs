namespace LandmarkEmulator.Database.Character.Model
{
    public class CharacterCustomisationModel
    {
        public ulong Id { get; set; }
        public uint Slot { get; set; }
        public uint Option { get; set; }
        public uint Tint { get; set; }

        public CharacterModel Character { get; set; }
    }
}
