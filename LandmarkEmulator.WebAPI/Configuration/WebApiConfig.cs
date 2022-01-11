namespace LandmarkEmulator.WebAPI.Configuration
{
    public class WebApiConfig
    {
        public bool Enabled { get; set; } = true;
        public string BaseURI { get; set; } = "https://0.0.0.0:5000";
    }
}
