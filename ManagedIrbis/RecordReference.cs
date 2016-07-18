/* RecordReference.cs -- ссылка на запись.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;
using CodeJam;
using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Ссылка на запись (например, для сохранения в "кармане").
    /// </summary>
    [PublicAPI]
    [XmlRoot("record")]
    [MoonSharpUserData]
    [DebuggerDisplay("MFN={Mfn}, Index={Index}")]
    public sealed class RecordReference
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Сервер ИРБИС64. Например, "127.0.0.1".
        /// </summary>
        [CanBeNull]
        [XmlAttribute("host")]
        [JsonProperty("host")]
        public string HostName { get; set; }

        /// <summary>
        /// База данных. Например, "IBIS".
        /// </summary>
        [CanBeNull]
        [XmlAttribute("db")]
        [JsonProperty("db")]
        public string Database { get; set; }

        /// <summary>
        /// MFN. Чаще всего = 0, т. к. используется Index.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Шифр записи в базе данных, например "81.432.1-42/P41-012833".
        /// </summary>
        [CanBeNull]
        [XmlAttribute("index")]
        [JsonProperty("index")]
        public string Index { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Загрузка ссылок из упакованного файла.
        /// </summary>
        public static RecordReference[] LoadFromZipFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNull(fileName, "fileName");

            RecordReference[] result = SerializationUtility
                .RestoreArrayFromZipFile<RecordReference>
                (
                    fileName
                );

            return result;
        }

        /// <summary>
        /// Загрузка записи, соответствующей ссылке,
        /// с сервера.
        /// </summary>
        [CanBeNull]
        public MarcRecord ReadRecord
            (
                [NotNull] IrbisConnection client
            )
        {
            Code.NotNull(client, "client");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Загрузка записей, соответствующих ссылкам.
        /// </summary>
        public static List<MarcRecord> ReadRecords
            (
                [NotNull] IrbisConnection client,
                [NotNull] IEnumerable<RecordReference> references
            )
        {
            Code.NotNull(client, "client");
            Code.NotNull(references, "references");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Сохранение ссылок в упакованном файле.
        /// </summary>
        public static void SaveToZipFile
            (
                [NotNull][ItemNotNull] RecordReference[] references,
                [NotNull] string fileName
            )
        {
            Code.NotNull(references, "references");
            Code.NotNull(fileName, "fileName");

            references.SaveToZipFile(fileName);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            HostName = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            Mfn = reader.ReadPackedInt32();
            Index = reader.ReadNullableString();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(HostName)
                .WriteNullable(Database)
                .WritePackedInt32(Mfn)
                .WriteNullable(Index);
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
                    "Mfn: {0}, Index: {1}",
                    Mfn,
                    Index
                );
        }

        #endregion
    }
}
