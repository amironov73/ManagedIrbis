/* RevisionInfo.cs -- данные о редактировании записи
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AM.IO;
using BLToolkit.Mapping;

using JetBrains.Annotations;

using ManagedClient.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Fields
{
    /// <summary>
    /// Данные о редактировании записи (поле 907).
    /// </summary>
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("Stage={Stage} Date={Date} Name={Name}")]
    public sealed class RevisionInfo
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abc";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const string Tag = "907";

        #endregion

        #region Properties

        /// <summary>
        /// Этап работы. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("stage")]
        [JsonProperty("stage")]
        [MapField("stage")]
        public string Stage { get; set; }

        /// <summary>
        /// Дата. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("date")]
        [JsonProperty("date")]
        [MapField("date")]
        public string Date { get; set; }

        /// <summary>
        /// ФИО оператора. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        [MapField("name")]
        public string Name { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [MapIgnore]
        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        #endregion

        #region Private members

        [NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля.
        /// </summary>
        public static RevisionInfo Parse
            (
                RecordField field
            )
        {
            RevisionInfo result = new RevisionInfo
                {
                    Date = field.GetFirstSubFieldValue('a'),
                    Name = field.GetFirstSubFieldValue('b'),
                    Stage = field.GetFirstSubFieldValue('c')
                };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static RevisionInfo[] Parse
            (
                IrbisRecord record,
                string tag
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("tag");
            }

            return record.Fields
                .GetField(tag)
                .Select(Parse)
                .ToArray();
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static RevisionInfo[] Parse
            (
                IrbisRecord record
            )
        {
            return Parse
                (
                    record,
                    Tag
                );
        }

        /// <summary>
        /// Превращение обратно в поле.
        /// </summary>
        public RecordField ToField()
        {
            RecordField result = new RecordField("907")
                .AddNonEmptySubField('a', Date)
                .AddNonEmptySubField('b', Name)
                .AddNonEmptySubField('c', Stage);
            return result;
        }

        /// <summary>
        /// Save to stream
        /// </summary>
        public void SaveToStream
            (
                [JetBrains.Annotations.NotNull] BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Stage)
                .WriteNullable(Date)
                .WriteNullable(Name);
        }

        /// <summary>
        /// Read from stream.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [JetBrains.Annotations.NotNull]
        public static RevisionInfo ReadFromStream
            (
                [JetBrains.Annotations.NotNull] BinaryReader reader
            )
        {
            RevisionInfo result = new RevisionInfo
            {
                Stage = reader.ReadNullableString(),
                Date = reader.ReadNullableString(),
                Name = reader.ReadNullableString()
            };

            return result;
        }

        /// <summary>
        /// Save bunch to the stream.
        /// </summary>
        public static void SaveToStream
            (
                [JetBrains.Annotations.NotNull] IEnumerable<RevisionInfo> items,
                [JetBrains.Annotations.NotNull] Stream stream
            )
        {
            BinaryWriter writer = new BinaryWriter(stream);
            foreach (RevisionInfo item in items)
            {
                item.SaveToStream(writer);
            }
        }

        /// <summary>
        /// Save bunch to the file.
        /// </summary>
        public static void SaveToFile
            (
                [JetBrains.Annotations.NotNull] IEnumerable<RevisionInfo> items,
                [JetBrains.Annotations.NotNull] string fileName
            )
        {
            using (Stream stream = File.Create(fileName))
            {
                SaveToStream(items, stream);
            }
        }


        /// <summary>
        /// Save bunch to memory.
        /// </summary>
        public static byte[] SaveToMemory
            (
                [JetBrains.Annotations.NotNull] IEnumerable<RevisionInfo> items
            )
        {
            using (MemoryStream stream = new MemoryStream())
            {
                SaveToStream(items, stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Read bunch from the stream.
        /// </summary>
        public static List<RevisionInfo> ReadFromStream
            (
                [JetBrains.Annotations.NotNull] Stream stream
            )
        {
            BinaryReader reader = new BinaryReader(stream);
            List<RevisionInfo> result = new List<RevisionInfo>();

            while (reader.PeekChar() >= 0)
            {
                RevisionInfo item = ReadFromStream(reader);
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Read bunch from the stream.
        /// </summary>
        public static List<RevisionInfo> ReadFromMemory
            (
                [JetBrains.Annotations.NotNull] byte[] bytes
            )
        {
            using (Stream stream = new MemoryStream(bytes))
            {
                return ReadFromStream(stream);
            }
        }

        /// <summary>
        /// Read bunch from the file.
        /// </summary>
        public static List<RevisionInfo> ReadFromFile
            (
                [JetBrains.Annotations.NotNull] string fileName
            )
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                return ReadFromStream(stream);
            }
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "Stage: {0}, Date: {1}, Name: {2}",
                    Stage,
                    Date,
                    Name
                );
        }

        #endregion
    }
}
