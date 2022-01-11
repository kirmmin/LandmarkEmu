using System;
using System.Security.Cryptography;

namespace LandmarkEmulator.Shared.Network.Cryptography
{
    public static class RandomProvider
    {
        /// <summary>
        /// Get a random set of bytes with the length of the given unsigned integer.
        /// </summary>
        public static byte[] GetBytes(uint count)
        {
            return RandomNumberGenerator.GetBytes((int)count);
        }

        /// <summary>
        /// Get a random <see cref="Guid"/>.
        /// </summary>
        public static Guid GetGuid()
        {
            byte[] data = GetBytes(16u);
            return new Guid(data);
        }
    }
}
