namespace LandmarkEmulator.WebAPI.Configuration
{
    public class WebApiConfig
    {
        public bool Enabled { get; set; } = true;
        public string Host { get; set; } = "0.0.0.0";
        public ushort Port { get; set; } = 5000;
    }
}
