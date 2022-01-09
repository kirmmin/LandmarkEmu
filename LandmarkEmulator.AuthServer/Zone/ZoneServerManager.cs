using LandmarkEmulator.Shared;
using LandmarkEmulator.Shared.Database;
using System.Collections.Immutable;
using System.Linq;

namespace LandmarkEmulator.AuthServer.Zone
{
    public class ZoneServerManager : Singleton<ZoneServerManager>
    {
        public ImmutableList<ZoneServer> ZoneServers { get; private set; }

        public void Initialise()
        {
            ZoneServers = DatabaseManager.Instance.AuthDatabase.GetZoneServers()
                .Select(s => new ZoneServer(s))
                .ToImmutableList();
        }
    }
}
