// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IriProfile.cs -- профиль ИРИ
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Профиль ИРИ
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("iri-profile")]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{Title} {Query}")]
#endif
    public sealed class IriProfile
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Тег поля ИРИ.
        /// </summary>
        public const string IriTag = "140";

        #endregion

        #region Properties

        /// <summary>
        /// Подполе A
        /// </summary>
        [SubField('a')]
        [XmlAttribute("active")]
        [JsonProperty("active")]
        public bool Active { get; set; }

        /// <summary>
        /// Подполе B
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("id")]
        [JsonProperty("id")]
        // ReSharper disable once InconsistentNaming
        public string ID { get; set; }

        /// <summary>
        /// Подполе C
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Подполе D
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("query")]
        [JsonProperty("query")]
        public string Query { get; set; }

        /// <summary>
        /// Подполе E
        /// </summary>
        [SubField('e')]
        [XmlAttribute("periodicity")]
        [JsonProperty("periodicity")]
        public int Periodicity { get; set; }

        /// <summary>
        /// Подполе F
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("lastServed")]
        [JsonProperty("lastServed")]
        public string LastServed { get; set; }

        /// <summary>
        /// Подполе I
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        [XmlAttribute("database")]
        [JsonProperty("database")]
        public string Database { get; set; }

        /// <summary>
        /// Ссылка на читателя.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public ReaderInfo Reader { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля.
        /// </summary>
        [NotNull]
        public static IriProfile ParseField
            (
                [NotNull] RecordField field
            )
        {
            // TODO Support for unknown subfields

            Code.NotNull(field, "field");

            IriProfile result = new IriProfile
            {
                Active = field.GetFirstSubFieldValue('a') == "1",
                ID = field.GetFirstSubFieldValue('b'),
                Title = field.GetFirstSubFieldValue('c'),
                Query = field.GetFirstSubFieldValue('d'),
                Periodicity = int.Parse(field.GetFirstSubFieldValue('e')),
                LastServed = field.GetFirstSubFieldValue('f'),
                Database = field.GetFirstSubFieldValue('i')
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        public static IriProfile[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<IriProfile> result = new List<IriProfile>();
            foreach (RecordField field in record.Fields
                .GetField(IriTag))
            {
                IriProfile profile = ParseField(field);
                result.Add(profile);
            }

            return result.ToArray();
        }

#if !WIN81 && !PORTABLE

        /// <summary>
        /// Считывание из файла.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static IriProfile[] LoadFromFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            IriProfile[] result = SerializationUtility
                .RestoreArrayFromFile<IriProfile>
                (
                    fileName
                )
                .ThrowIfNull("RestoreArrayFromFile");

            return result;
        }

        /// <summary>
        /// Сохранение в файл.
        /// </summary>
        public static void SaveToFile
            (
                [NotNull] string fileName,
                [NotNull][ItemNotNull] IriProfile[] profiles
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(profiles, "profiles");

            profiles.SaveToFile(fileName);
        }

#endif

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Active = reader.ReadBoolean();
            ID = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            Query = reader.ReadNullableString();
            Periodicity = reader.ReadPackedInt32();
            LastServed = reader.ReadNullableString();
            Database = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.Write(Active);
            writer.WriteNullable(ID);
            writer.WriteNullable(Title);
            writer.WriteNullable(Query);
            writer.WritePackedInt32(Periodicity);
            writer.WriteNullable(LastServed);
            writer.WriteNullable(Database);
        }

        #endregion
    }
}

