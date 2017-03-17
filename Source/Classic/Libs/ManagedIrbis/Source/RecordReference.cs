// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordReference.cs -- ссылка на запись.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 * 
 * TODO use Host property
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

using ManagedIrbis.ImportExport;

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
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("MFN={Mfn}, Index={Index}")]
#endif
    public sealed class RecordReference
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Host of IRBIS-server, e. g. "127.0.0.1".
        /// <c>null</c> for default host.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("host")]
        [JsonProperty("host")]
        public string HostName { get; set; }

        /// <summary>
        /// Database name, e. g. "IBIS".
        /// </summary>
        [CanBeNull]
        [XmlAttribute("db")]
        [JsonProperty("db")]
        public string Database { get; set; }

        /// <summary>
        /// MFN of the record.
        /// <c>0</c> means "use <see cref="Index"/> field".
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Index of the record, e. g. "81.432.1-42/P41-012833".
        /// </summary>
        [CanBeNull]
        [XmlAttribute("index")]
        [JsonProperty("index")]
        public string Index { get; set; }

        /// <summary>
        /// Record itself. Not written.
        /// Can be <c>null</c>.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord Record { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RecordReference()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordReference
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            HostName = record.HostName;
            Database = record.Database;
            Mfn = record.Mfn;
            Index = record.Index ?? record.FM("903");
            Record = record;
        }

        #endregion

        #region Public methods

#if !SILVERLIGHT && !WIN81 && !PORTABLE

        /// <summary>
        /// Load references from archive file.
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

#endif

        /// <summary>
        /// Load record according to the reference.
        /// </summary>
        [CanBeNull]
        public MarcRecord ReadRecord
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Verify(true);

            if (Mfn != 0)
            {
                // ReSharper disable AssignNullToNotNullAttribute
                Record = connection.ReadRecord
                    (
                        Database,
                        Mfn,
                        false,
                        null
                    );
                // ReSharper restore AssignNullToNotNullAttribute
            }
            else
            {
                Record = connection.SearchReadOneRecord
                    (
                        "\"I={0}\"",
                        Index
                    );
            }

            return Record;
        }

        /// <summary>
        /// Load records according to the references.
        /// </summary>
        [NotNull]
        public static List<MarcRecord> ReadRecords
            (
                [NotNull] IrbisConnection connection,
                [NotNull] IEnumerable<RecordReference> references,
                bool throwOnError
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(references, "references");

            List<MarcRecord> result = new List<MarcRecord>();
            foreach (RecordReference reference in references)
            {
                MarcRecord record = reference.ReadRecord(connection);
                if (ReferenceEquals(record, null))
                {
                    if (throwOnError)
                    {
                        throw new IrbisException("record not found");
                    }
                }
                else
                {
                    result.Add(record);
                }
            }

            return result;
        }

#if !SILVERLIGHT && !WIN81 && !PORTABLE

        /// <summary>
        /// Save references to the archive file.
        /// </summary>
        public static void SaveToZipFile
            (
                [NotNull][ItemNotNull] RecordReference[] references,
                [NotNull] string fileName
            )
        {
            Code.NotNull(references, "references");
            Code.NotNullNorEmpty(fileName, "fileName");

            references.SaveToZipFile(fileName);
        }

#endif

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            HostName = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            Mfn = reader.ReadPackedInt32();
            Index = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object stat to the stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(HostName)
                .WriteNullable(Database)
                .WritePackedInt32(Mfn)
                .WriteNullable(Index);
        }

        #endregion

        #region IVerifiable<T> members

        /// <summary>
        /// Verify the object state.
        /// </summary>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<RecordReference> verifier
                = new Verifier<RecordReference>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .NotNullNorEmpty(Database, "Database")
                .Assert
                    (
                        (Mfn != 0)
                        || string.IsNullOrEmpty(Index),
                        "Mfn or Index"
                    );

            return verifier.Result;
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
            if (ReferenceEquals(Record, null))
            {
                return string.Format
                    (
                        "{0}#{1}#{2}",
                        Database,
                        Mfn,
                        Index
                    );
            }

            string result = string.Format
                (
                    "{0}{1}{2}",
                    Database,
                    IrbisText.IrbisDelimiter,
                    Record.ToProtocolText()
                );

            return result;
        }

        #endregion
    }
}
