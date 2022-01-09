namespace LandmarkEmulator.Database.Configuration
{
    public class DatabaseConnectionString : IConnectionString
    {
        public DatabaseProvider Provider { get; set; }
        public string ConnectionString { get; set; }
    }
}
