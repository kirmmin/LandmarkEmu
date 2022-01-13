using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LandmarkEmulator.Shared.GameTable
{
    public class GameTable<T> where T : class, new()
    {
        public IEnumerable<T> Entries => entries.Values.ToList();
        public int RecordCount => entries.Count;
        public ulong MaxId => entries.LastOrDefault().Key;

        private Dictionary<ulong, T> entries = new();
        private static readonly Dictionary<FieldInfo, GameTableFieldArrayAttribute> attributeCache;
        private bool _hasId = true;

        /// <summary>
        /// Initiailise a new <see cref="GameTable{T}"/> container for a data type.
        /// </summary>
        static GameTable()
        {
            attributeCache = new Dictionary<FieldInfo, GameTableFieldArrayAttribute>();
            foreach (FieldInfo modelField in typeof(T).GetFields())
            {
                GameTableFieldArrayAttribute attribute = modelField.GetCustomAttribute<GameTableFieldArrayAttribute>();
                attributeCache.Add(modelField, attribute);
            }
        }

        /// <summary>
        /// Initiailise a new <see cref="GameTable{T}"/> container for a data type, with a given file path and whether it has IDs.
        /// </summary>
        public GameTable(string path, bool hasId)
        {
            _hasId = hasId;

            using (StreamReader reader = File.OpenText(path))
            {
                ReadEntries(reader);   
            }
        }

        /// <summary>
        /// Reads the entries from the file provided as a <see cref="StreamReader"/>.
        /// </summary>
        private void ReadEntries(StreamReader reader)
        {
            PropertyInfo[] typeFields = typeof(T).GetProperties();

            uint recordCount = 0;
            string line = String.Empty;
            while ((line = reader.ReadLine()) != null)
            {
                // Skip header
                if (recordCount == 0)
                {
                    recordCount++;
                    continue;
                }

                // Initiailise new entry for GameTable
                var entry = new T();

                // Create strings of each
                var pieces = Regex.Split(line, @"\^");
                for (int i = 0; i < typeFields.Length; i++)
                {
                    PropertyInfo modelField = typeFields[i];
                    switch (Type.GetTypeCode(modelField.PropertyType))
                    {
                        case TypeCode.Boolean:
                            modelField.SetValue(entry, Convert.ToBoolean(Byte.Parse(pieces[i])));
                            break;
                        case TypeCode.Byte:
                            modelField.SetValue(entry, Byte.Parse(pieces[i]));
                            break;
                        case TypeCode.SByte:
                            modelField.SetValue(entry, SByte.Parse(pieces[i]));
                            break;
                        case TypeCode.Int16:
                            modelField.SetValue(entry, Int16.Parse(pieces[i]));
                            break;
                        case TypeCode.Int32:
                            modelField.SetValue(entry, Int32.Parse(pieces[i]));
                            break;
                        case TypeCode.Int64:
                            modelField.SetValue(entry, Int64.Parse(pieces[i]));
                            break;
                        case TypeCode.UInt16:
                            modelField.SetValue(entry, UInt16.Parse(pieces[i]));
                            break;
                        case TypeCode.UInt32:
                            modelField.SetValue(entry, UInt32.Parse(pieces[i]));
                            break;
                        case TypeCode.UInt64:
                            modelField.SetValue(entry, UInt64.Parse(pieces[i]));
                            break;
                        case TypeCode.String:
                        default:
                            modelField.SetValue(entry, pieces[i]);
                            break;
                    }
                }

                var id = (uint)(_hasId ? typeFields[0].GetValue(entry) : recordCount);
                entries.TryAdd(id, entry);

                recordCount++;
            }
        }

        /// <summary>
        /// Returns a <see cref="{T}"/> with a given ID. Null if ID does not exist.
        /// </summary>
        public T GetEntry(ulong id)
        {
            if (id > MaxId)
                return null;

            return entries.TryGetValue(id, out T value) ? value : null;
        }
    }
}
