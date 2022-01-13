namespace LandmarkEmulator.Shared.GameTable.Model
{
    public class BlackListEntry
    {
        public string Word { get; set; }
        public string ReplacementWord { get; set; }
        public uint FilterType { get; set; }
        public string ExemptWords { get; set; }
        public bool IgnoreSubstringChecks { get; set; }
        public bool RequiresExactMatch { get; set; }
    }
}
