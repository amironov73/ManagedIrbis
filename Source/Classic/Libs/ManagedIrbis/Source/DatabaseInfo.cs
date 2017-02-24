// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DatabaseInfo.cs -- информация о базе данных ИРБИС
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

#if FW4
using System.Diagnostics.CodeAnalysis;
#endif

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Информация о базе данных ИРБИС
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{Name} {Description}")]
#endif
    public sealed class DatabaseInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Разделитель элементов
        /// </summary>
        public const char ItemDelimiter = (char)0x1E;

        #endregion

        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Описание базы данных
        /// </summary>
        [CanBeNull]
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Максимальный MFN.
        /// </summary>
        [XmlAttribute("maxMfn")]
        [JsonProperty("maxMfn")]
        public int MaxMfn { get; set; }

        /// <summary>
        /// Список логически удаленных записей.
        /// </summary>
        [CanBeNull]
        [XmlArrayItem("mfn")]
        [XmlArray("logicallyDeleted")]
        [JsonProperty("logicallyDeleted")]
        public int[] LogicallyDeletedRecords { get; set; }

        /// <summary>
        /// Список физически удаленных записей.
        /// </summary>
        [CanBeNull]
        [XmlArrayItem("mfn")]
        [XmlArray("physicallyDeleted")]
        [JsonProperty("physicallyDeleted")]
        public int[] PhysicallyDeletedRecords { get; set; }

        /// <summary>
        /// Список неактуализированных записей.
        /// </summary>
        [CanBeNull]
        [XmlArrayItem("mfn")]
        [XmlArray("nonActualizedRecords")]
        [JsonProperty("nonActualizedRecords")]
        public int[] NonActualizedRecords { get; set; }

        /// <summary>
        /// Список заблокированных записей.
        /// </summary>
        [CanBeNull]
        [XmlArrayItem("mfn")]
        [XmlArray("lockedRecords")]
        [JsonProperty("lockedRecords")]
        public int[] LockedRecords { get; set; }

        /// <summary>
        /// Флаг монопольной блокировки базы данных.
        /// </summary>
        [XmlAttribute("databaseLocked")]
        [JsonProperty("databaseLocked")]
        public bool DatabaseLocked { get; set; }

        /// <summary>
        /// База данных только для чтения.
        /// </summary>
        [XmlAttribute("readOnly")]
        [JsonProperty("readOnly")]
        public bool ReadOnly { get; set; }

        #endregion

        #region Private members

        private static int[] _ParseLine
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return new int[0];
            }

            string[] items = text.Split(ItemDelimiter);
            int[] result = items
                // ReSharper disable once ConvertClosureToMethodGroup
                // Due to .NET 3.5
                .Select(_ => int.Parse(_))
                .OrderBy(_ => _)
                .ToArray();

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Describe the database.
        /// </summary>
        public string Describe()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat
                (
                    "Name: {0}", 
                    Name.ToVisibleString()
                );
            result.AppendLine();

            result.AppendFormat
                (
                    "Description: {0}", 
                    Description.ToVisibleString()
                );
            result.AppendLine();

            if (!ReferenceEquals(LogicallyDeletedRecords, null))
            {
                result.Append("Logically deleted records: ");
                result.AppendLine(NumericUtility.CompressRange
                    (
                        LogicallyDeletedRecords
                    ));
            }

            if (!ReferenceEquals(PhysicallyDeletedRecords, null))
            {
                result.Append("Physically deleted records: ");
                result.AppendLine(NumericUtility.CompressRange
                    (
                        PhysicallyDeletedRecords
                    ));
            }

            if (!ReferenceEquals(NonActualizedRecords, null))
            {
                result.Append("Non-actualized records: ");
                result.AppendLine(NumericUtility.CompressRange
                    (
                        NonActualizedRecords
                    ));
            }

            if (!ReferenceEquals(LockedRecords, null))
            {
                result.Append("Locked records: ");
                result.AppendLine(NumericUtility.CompressRange
                    (
                        LockedRecords
                    ));
            }

            result.AppendFormat("Max MFN: {0}", MaxMfn);
            result.AppendLine();

            result.AppendFormat("Read-only: {0}", ReadOnly);
            result.AppendLine();

            result.AppendFormat
                (
                    "Database locked: {0}", 
                    DatabaseLocked
                );
            result.AppendLine();

            return result.ToString();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        [NotNull]
        public static DatabaseInfo ParseServerResponse
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            DatabaseInfo result = new DatabaseInfo
                {
                    PhysicallyDeletedRecords
                        = _ParseLine(response.GetAnsiString()),
                    LogicallyDeletedRecords
                        = _ParseLine(response.GetAnsiString()),
                    NonActualizedRecords
                        = _ParseLine(response.GetAnsiString()),
                    LockedRecords
                        = _ParseLine(response.GetAnsiString()),
                    MaxMfn = _ParseLine(response.GetAnsiString())
                        .GetItem(0,0),
                    DatabaseLocked
                        = _ParseLine(response.GetAnsiString())
                        .GetItem(0,0) != 0
                };

            return result;
        }

        /// <summary>
        /// Разбор файла меню
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static DatabaseInfo[] ParseMenu
            (
                [NotNull] string[] text
            )
        {
            Code.NotNull(text, "text");

            List<DatabaseInfo> result = new List<DatabaseInfo>();

            for (int i = 0; i < text.Length; i += 2)
            {
                string name = text[i];
                if (string.IsNullOrEmpty(name)
                    || name.StartsWith("*"))
                {
                    break;
                }
                bool readOnly = false;
                if (name.StartsWith("-"))
                {
                    name = name.Substring(1);
                    readOnly = true;
                }
                string description = text[i + 1];
                DatabaseInfo oneBase = new DatabaseInfo
                    {
                        Name = name,
                        Description = description,
                        ReadOnly = readOnly
                    };
                result.Add(oneBase);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Should serialize <see cref="MaxMfn"/>?
        /// </summary>
        #if FW4
        [ExcludeFromCodeCoverage]
        #endif
        public bool ShouldSerializeMaxMfn()
        {
            return MaxMfn != 0;
        }

        /// <summary>
        /// Should serialize <see cref="LogicallyDeletedRecords"/>?
        /// </summary>
        #if FW4
        [ExcludeFromCodeCoverage]
        #endif
        public bool ShouldSerializeLogicallyDeletedRecords()
        {
            return !ReferenceEquals(LogicallyDeletedRecords, null);
        }

        /// <summary>
        /// Should serialize <see cref="PhysicallyDeletedRecords"/>?
        /// </summary>
        #if FW4
        [ExcludeFromCodeCoverage]
        #endif
        public bool ShouldSerializePhysicallyDeletedRecords()
        {
            return !ReferenceEquals(PhysicallyDeletedRecords, null);
        }

        /// <summary>
        /// Should serialize <see cref="NonActualizedRecords"/>?
        /// </summary>
        #if FW4
        [ExcludeFromCodeCoverage]
        #endif
        public bool ShouldSerializeNonActualizedRecords()
        {
            return !ReferenceEquals(NonActualizedRecords, null);
        }

        /// <summary>
        /// Should serialize <see cref="LockedRecords"/>?
        /// </summary>
        #if FW4
        [ExcludeFromCodeCoverage]
        #endif
        public bool ShouldSerializeLockedRecords()
        {
            return !ReferenceEquals(LockedRecords, null);
        }

        /// <summary>
        /// Should serialize <see cref="DatabaseLocked"/>?
        /// </summary>
        #if FW4
        [ExcludeFromCodeCoverage]
        #endif
        public bool ShouldSerializeDatabaseLocked()
        {
            return DatabaseLocked;
        }

        /// <summary>
        /// Should serialize <see cref="ReadOnly"/>?
        /// </summary>
        #if FW4
        [ExcludeFromCodeCoverage]
        #endif
        public bool ShouldSerializeReadOnly()
        {
            return ReadOnly;
        }

        #endregion

        #region IHandmadeSerializable membrs

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Name = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            MaxMfn = reader.ReadPackedInt32();
            LogicallyDeletedRecords 
                = reader.ReadNullableInt32Array();
            PhysicallyDeletedRecords
                = reader.ReadNullableInt32Array();
            NonActualizedRecords
                = reader.ReadNullableInt32Array();
            LockedRecords
                = reader.ReadNullableInt32Array();
            DatabaseLocked = reader.ReadBoolean();
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Name)
                .WriteNullable(Description)
                .WritePackedInt32(MaxMfn)
                .WriteNullableArray(LogicallyDeletedRecords)
                .WriteNullableArray(PhysicallyDeletedRecords)
                .WriteNullableArray(NonActualizedRecords)
                .WriteNullableArray(LockedRecords)
                .Write(DatabaseLocked);
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return Name.ToVisibleString();
            }

            return string.Format
                (
                    "{0} - {1}",
                    Name,
                    Description
                );
        }

        #endregion
    }
}
