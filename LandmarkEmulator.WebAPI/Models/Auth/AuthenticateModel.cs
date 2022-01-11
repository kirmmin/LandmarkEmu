using System;

namespace LandmarkEmulator.WebAPI.Models.Auth
{
    public class AuthenticateModel
    {
        public string SessionKey { get; set; }
        public DateTime SessionKeyExpiration { get; set; }
        public string Message { get; set; }
    }
}
