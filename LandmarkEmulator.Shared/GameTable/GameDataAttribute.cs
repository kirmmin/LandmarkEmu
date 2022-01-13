using System;

namespace LandmarkEmulator.Shared.GameTable
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GameDataAttribute : Attribute
    {
        /// <summary>
        /// Returns whether or not this <see cref="GameData"/> type has an ID field. True by default.
        /// </summary>
        public bool HasId { get; } = true;
        /// <summary>
        /// The filename that this <see cref="GameData"/> will be reading from.
        /// </summary>
        public string FileName { get; }

        public GameDataAttribute(bool hasId = true)
            : this(null)
        {
            HasId = hasId;
        }

        public GameDataAttribute(string fileName, bool hasId = true)
        {
            FileName = fileName;
            HasId    = hasId;
        }
    }
}
