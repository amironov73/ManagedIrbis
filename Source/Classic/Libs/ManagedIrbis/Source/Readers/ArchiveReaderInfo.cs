// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ArchiveReaderInfo.cs -- архивная информация о читателе.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM;
using AM.Collections;
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
    /// Информация о читателе.
    /// </summary>
    [PublicAPI]
    [XmlRoot("reader")]
    [MoonSharpUserData]
    public sealed class ArchiveReaderInfo
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Identifier for LiteDB.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Номер читательского. Поле 30.
        /// </summary>
        [CanBeNull]
        [Field("30")]
        [XmlAttribute("ticket")]
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        /// <summary>
        /// Информация о посещениях.
        /// </summary>
        [XmlArray("visits")]
        [JsonProperty("visits")]
        public VisitInfo[] Visits { get; set; }

        /// <summary>
        /// Произвольные данные, ассоциированные с читателем.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        /// <summary>
        /// Дата первого посещения
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime FirstVisitDate
        {
            get
            {
                if (Visits.IsNullOrEmpty())
                {
                    return DateTime.MinValue;
                }

                return Visits.First().DateGiven;
            }
        }

        /// <summary>
        /// Дата последнего посещения.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime LastVisitDate
        {
            get
            {
                if (Visits.IsNullOrEmpty())
                {
                    return DateTime.MinValue;
                }

                return Visits.Last().DateGiven;
            }
        }

        /// <summary>
        /// Кафедра последнего посещения.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public string LastVisitPlace
        {
            get
            {
                if (Visits.IsNullOrEmpty())
                {
                    return null;
                }

                return Visits.Last().Department;
            }
        }

        /// <summary>
        /// Последний обслуживавший библиотекарь.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public string LastVisitResponsible
        {
            get
            {
                if (Visits.IsNullOrEmpty())
                {
                    return null;
                }

                return Visits.Last().Responsible;
            }
        }

        /// <summary>
        /// MFN записи.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Flag for the reader info.
        /// </summary>
        [XmlAttribute("marked")]
        [JsonProperty("marked")]
        public bool Marked { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the specified record.
        /// </summary>
        [NotNull]
        public static ArchiveReaderInfo Parse
            (
                [NotNull] MarcRecord record
            )
        {
            // TODO Support for unknown fields

            Code.NotNull(record, "record");

            ArchiveReaderInfo result = new ArchiveReaderInfo
            {
                Ticket = record.FM(30),
                Visits = record.Fields
                    .GetField(40)
                    .Select(field => VisitInfo.Parse(field))
                    .ToArray(),
                Mfn = record.Mfn,
            };

            return result;
        }

        /// <summary>
        /// Считывание из файла.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public static ArchiveReaderInfo[] ReadFromFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            ArchiveReaderInfo[] result = SerializationUtility
                .RestoreArrayFromFile<ArchiveReaderInfo>(fileName);

            return result;
        }

        /// <summary>
        /// Сохранение в файле.
        /// </summary>
        public static void SaveToFile
            (
                [NotNull] string fileName,
                [NotNull] [ItemNotNull] ArchiveReaderInfo[] readers
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(readers, "readers");

            readers.SaveToFile(fileName);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WritePackedInt32(Id);
            writer.WriteNullable(Ticket);
            writer.WriteNullableArray(Visits);
            writer.WritePackedInt32(Mfn);
        }

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Id = reader.ReadPackedInt32();
            Ticket = reader.ReadNullableString();
            Visits = reader.ReadNullableArray<VisitInfo>();
            Mfn = reader.ReadPackedInt32();
        }

        /// <summary>
        /// Формирование записи по данным о читателе.
        /// </summary>
        [NotNull]
        public MarcRecord ToRecord()
        {
            MarcRecord result = new MarcRecord
            {
                Mfn = Mfn
            };

            result.AddNonEmptyField(30, Ticket);
            foreach (VisitInfo visit in Visits)
            {
                result.AddField(visit.ToField());
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Ticket.ToVisibleString();
        }

        #endregion
    }
}
