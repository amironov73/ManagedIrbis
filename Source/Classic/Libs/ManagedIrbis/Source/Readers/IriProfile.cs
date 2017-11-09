// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IriProfile.cs -- профиль ИРИ
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
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
    [DebuggerDisplay("{Title} {Query}")]
    public sealed class IriProfile
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Тег поля ИРИ.
        /// </summary>
        public const int Tag = 140;

        /// <summary>
        /// Известные коды.
        /// </summary>
        public const string KnownCodes = "abcdefi";

        #endregion

        #region Properties

        /// <summary>
        /// Статус профиля (активен или нет). Подполе A.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("active")]
        [JsonProperty("active", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Active { get; set; }

        /// <summary>
        /// Код (порядковый номер). Подполе B.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("id")]
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        // ReSharper disable once InconsistentNaming
        public string ID { get; set; }

        /// <summary>
        /// Описание профиля на естественном языке. Подполе C.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("title")]
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        /// Запрос на языке ИРБИС. Подполе D.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("query")]
        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public string Query { get; set; }

        /// <summary>
        /// Периодичность в днях, целое число. Подполе E.
        /// </summary>
        [SubField('e')]
        [XmlAttribute("periodicity")]
        [JsonProperty("periodicity", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Periodicity { get; set; }

        /// <summary>
        /// Дата последнего обслуживания. Подполе F.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("lastServed")]
        [JsonProperty("lastServed", NullValueHandling = NullValueHandling.Ignore)]
        public string LastServed { get; set; }

        /// <summary>
        /// Список баз данных. Подполе I.
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        [XmlAttribute("database")]
        [JsonProperty("database", NullValueHandling = NullValueHandling.Ignore)]
        public string Database { get; set; }

        /// <summary>
        /// Поле, в котором хранится профиль.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public RecordField Field { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [XmlElement("unknown")]
        [JsonProperty("unknown", NullValueHandling = NullValueHandling.Ignore)]
        [Browsable(false)]
        public SubField[] UnknownSubFields { get; set; }

        /// <summary>
        /// Ссылка на читателя.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public ReaderInfo Reader { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

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
            Code.NotNull(field, "field");

            IriProfile result = new IriProfile
            {
                Active = field.GetFirstSubFieldValue('a') == "1",
                ID = field.GetFirstSubFieldValue('b'),
                Title = field.GetFirstSubFieldValue('c'),
                Query = field.GetFirstSubFieldValue('d'),
                Periodicity = int.Parse(field.GetFirstSubFieldValue('e')),
                LastServed = field.GetFirstSubFieldValue('f'),
                Database = field.GetFirstSubFieldValue('i'),
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes),
                Field = field
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
                .GetField(Tag))
            {
                IriProfile profile = ParseField(field);
                result.Add(profile);
            }

            return result.ToArray();
        }


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

#if WIN81 || PORTABLE

            throw new System.NotImplementedException();
#else

            IriProfile[] result = SerializationUtility
                .RestoreArrayFromFile<IriProfile>
                    (
                        fileName
                    )
                .ThrowIfNull("RestoreArrayFromFile");

            return result;

#endif
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

#if WIN81 || PORTABLE

            throw new System.NotImplementedException();

#else


            profiles.SaveToFile(fileName);

#endif
        }


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
            UnknownSubFields = reader.ReadNullableArray<SubField>();
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
            writer.WriteNullableArray(UnknownSubFields);
        }

        #endregion
    }
}

