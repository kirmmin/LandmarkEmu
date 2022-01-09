namespace LandmarkEmulator.Database.Configuration
{
    public interface IConnectionString
    {
        DatabaseProvider Provider { get; }
        string ConnectionString { get; }
    }
}
