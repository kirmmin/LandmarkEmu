using System;
using System.IO;

namespace LandmarkEmulator.Shared.Game
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the remaining bytes in a <see cref="Stream"/>.
        /// </summary>
        public static uint Remaining(this Stream stream)
        {
            if (stream.Length < stream.Position)
                throw new InvalidOperationException();

            return (uint)(stream.Length - stream.Position);
        }

        /// <summary>
        /// Convert a <see cref="byte[]"/> into a hex string.
        /// </summary>
        public static string ToHexString(this byte[] value)
        {
            return BitConverter.ToString(value).Replace("-", "");
        }

        /// <summary>
        /// Checks a string to see if contains another string.
        /// </summary>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
