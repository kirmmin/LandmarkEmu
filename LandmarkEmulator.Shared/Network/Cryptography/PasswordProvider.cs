using LandmarkEmulator.Shared.Game;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace LandmarkEmulator.Shared.Network.Cryptography
{
    public static class PasswordProvider
    {
        /// <summary>
        /// Returns a random salt and password verifier for supplied email and plaintext password.
        /// </summary>
        public static (string salt, string verifier) GenerateSaltAndVerifier(string password, byte[] s = null)
        {
            if (s == null)
                s = RandomProvider.GetBytes(16u);

            byte[] v = KeyDerivation.Pbkdf2(
                password: password,
                salt: s,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return (s.ToHexString(), v.ToHexString());
        }

        /// <summary>
        /// Verifies the given password with a salt and verifier.
        /// </summary>
        public static bool VerifyPassword(string s, string v, string password)
        {
            var salt = Convert.FromHexString(s);
            if (salt == null)
                return false;

            // hash the given password
            var hashOfpasswordToCheck = GenerateSaltAndVerifier(password, salt);

            // compare both hashes
            if (String.Compare(v, hashOfpasswordToCheck.verifier) == 0)
            {
                return true;
            }
            return false;
        }
    }
}
