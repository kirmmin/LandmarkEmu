using System;

namespace LandmarkEmulator.Database.Auth.Model
{
    public class AccountModel
    {
        public ulong Id { get; set; }
        public string Username { get; set; }
        public string Salt { get; set; }
        public string Verifier { get; set; }
        public string SessionKey { get; set; }
        public DateTime SessionKeyExpiration { get; set; }
        public string ServerTicket { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
