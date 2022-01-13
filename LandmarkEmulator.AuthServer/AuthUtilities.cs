using LandmarkEmulator.AuthServer.Network.Message.Static;
using LandmarkEmulator.Shared.Configuration;
using LandmarkEmulator.Shared.Database;
using LandmarkEmulator.Shared.GameTable;
using LandmarkEmulator.Shared.GameTable.Model;
using System;
using System.Text.RegularExpressions;

namespace LandmarkEmulator.AuthServer
{
    public static class AuthUtilities
    {
        /// <summary>
        /// Returns a <see cref="NameValidationResult"/> that described whether the provided name is allowed and available.
        /// </summary>
        public static NameValidationResult ValidateName(string name)
        {
            bool strictName = ConfigurationManager<AuthServerConfiguration>.Instance.Config.CreationRules.StrictName;

            foreach (BlackListEntry entry in GameTableManager.Instance.BlackList.Entries)
            {
                // Name must not match a word that requires an exact match
                if (entry.RequiresExactMatch && name.ToUpper() == entry.Word.ToUpper())
                    return NameValidationResult.NameNaughty;

                // Name must not include a bad word that does not require an exact match
                if (!entry.RequiresExactMatch && name.Contains(entry.Word, StringComparison.OrdinalIgnoreCase))
                    return NameValidationResult.NameNaughty;
            }

            // Name must not already exist.
            if (DatabaseManager.Instance.CharacterDatabase.CharacterNameExists(name))
                return NameValidationResult.NameTaken;

            // Name must be capitalized, no spaces, and only contain English alphabet characters.
            // Example: https://regex101.com/r/Ig0KT4/1
            if (strictName && !Regex.IsMatch(name, @"^[A-Z](?:[^0-9][^A-Z][a-z]{1,19})$"))
                return NameValidationResult.NameInvalid;

            // Name must not have any spaces in it
            // Example: https://regex101.com/r/RIeMXa/1
            if (Regex.IsMatch(name, @"[\s]"))
                return NameValidationResult.NameInvalid;

            // Names must be between 2 and 20 characters long
            if (name.Length < 2 || name.Length > 20)
                return NameValidationResult.NameInvalid;

            return NameValidationResult.Success;
        }
    }
}
