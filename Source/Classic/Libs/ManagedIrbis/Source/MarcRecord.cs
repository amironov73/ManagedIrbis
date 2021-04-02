// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MarcRecord.cs -- MARC record
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Pft;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// MARC record.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("[{Database}] MFN={Mfn} ({Version})")]
    public sealed class MarcRecord
        : IHandmadeSerializable,
        IReadOnly<MarcRecord>,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Hostname of IRBIS-server.
        /// </summary>
        [CanBeNull]
        [JsonProperty("hostname")]
        public string HostName { get; set; }

        /// <summary>
        /// Name of database that the records belongs to.
        /// </summary>
        [CanBeNull]
        [JsonProperty("database")]
        public string Database
        {
            get { return _database; }
            set { SetDatabase(value); }
        }

        /// <summary>
        /// MFN записи
        /// </summary>
        [JsonProperty("mfn")]
        public int Mfn
        {
            get { return _mfn; }
            set { SetMfn(value); }
        }

        /// <summary>
        /// Статус записи: удалена, блокирована и т.д.
        /// </summary>
        [JsonProperty("status")]
        public RecordStatus Status
        {
            get { return _status; }
            set { SetStatus(value); }
        }

        /// <summary>
        /// Версия записи. Нумеруется с нуля.
        /// </summary>
        [JsonProperty("version")]
        public int Version
        {
            get { return _version; }
            set { SetVersion(value); }
        }

        /// <summary>
        /// Смещение предыдущей версии записи.
        /// </summary>
        [JsonIgnore]
        public long PreviousOffset { get; set; }

        /// <summary>
        /// Поля записи.
        /// </summary>
        [JsonProperty("fields")]
        public RecordFieldCollection Fields { get { return _fields; } }

        /// <summary>
        /// Признак удалённой записи.
        /// </summary>
        [JsonIgnore]
        public bool Deleted
        {
            get { return (Status & RecordStatus.LogicallyDeleted) != 0; }
            set
            {
                if (value)
                {
                    Status |= RecordStatus.LogicallyDeleted;
                }
                else
                {
                    Status &= ~RecordStatus.LogicallyDeleted;
                }
            }
        }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        [CanBeNull]
        [JsonIgnore]
        public string Description { get; set; }

        /// <summary>
        /// Используется при сортировке записей.
        /// </summary>
        [CanBeNull]
        [JsonIgnore]
        public string SortKey { get; set; }

        /// <summary>
        /// Индекс документа.
        /// Используется для идентификации записей.
        /// </summary>
        [CanBeNull]
        [JsonIgnore]
        public string Index { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [CanBeNull]
        [JsonIgnore]
        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        /// <summary>
        /// Whether the record was modified?
        /// </summary>
        [JsonIgnore]
        public bool Modified
        {
            get { return _modified; }
            set { _modified = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор
        /// </summary>
        public MarcRecord()
        {
            _fields = new RecordFieldCollection()
                ._SetRecord(this);
        }

        /// <summary>
        /// Конструктор для клонирования.
        /// </summary>
        private MarcRecord
            (
                [NotNull] MarcRecord other
            )
        {
            Log.Trace("MarcRecord::CopyConstructor");

            Database = other.Database;
            Mfn = other.Mfn;
            Status = other.Status;
            Version = other.Version;
            PreviousOffset = other.PreviousOffset;
            _fields = other.Fields.Clone();
            _fields._SetRecord(this);
            Description = other.Description;
            SortKey = other.SortKey;
            Index = other.Index;
            UserData = other.UserData;
        }

        #endregion

        #region Private members

        private readonly RecordFieldCollection _fields;

        private string _database;

        private int _mfn, _version;

        private RecordStatus _status;

        [NonSerialized]
        private bool _modified;

        [NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Создание "глубокой" копии записи.
        /// </summary>
        [NotNull]
        public MarcRecord Clone()
        {
            Log.Trace("MarcRecord::Clone");

            MarcRecord result = new MarcRecord(this);

            return result;
        }

        /// <summary>
        /// Compares two records.
        /// </summary>
        public static int Compare
            (
                [NotNull] MarcRecord record1,
                [NotNull] MarcRecord record2
            )
        {
            Code.NotNull(record1, "record1");
            Code.NotNull(record2, "record2");

            int result = record1.Fields.Count - record2.Fields.Count;
            if (result != 0)
            {
                return result;
            }

            for (int i = 0; i < record1.Fields.Count; i++)
            {
                RecordField field1 = record1.Fields[i];
                RecordField field2 = record2.Fields[i];

                result = RecordField.Compare
                    (
                        field1,
                        field2
                    );
                if (result != 0)
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Получить текст поля до разделителей подполей
        /// первого повторения поля с указанной меткой.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <returns>Значение поля или <c>null</c>.</returns>
        [CanBeNull]
        // ReSharper disable InconsistentNaming
        public string FM
        // ReSharper restore InconsistentNaming
            (
                int tag
            )
        {
            return Fields.GetFirstFieldValue(tag);
        }

        /// <summary>
        /// Текст всех полей с указанным тегом.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        // ReSharper disable InconsistentNaming
        public string[] FMA
        // ReSharper restore InconsistentNaming
            (
                int tag
            )
        {
            return Fields.GetFieldValue(tag);
        }

        /// <summary>
        /// Текст первого подполя с указанным тегом и кодом.
        /// </summary>
        [CanBeNull]
        // ReSharper disable InconsistentNaming
        public string FM
        // ReSharper restore InconsistentNaming
            (
                int tag,
                char code
            )
        {
            return Fields.GetFirstSubFieldValue(tag, code);
        }

        /// <summary>
        /// Текст всех подполей с указанным тегом и кодом.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        // ReSharper disable InconsistentNaming
        public string[] FMA
        // ReSharper restore InconsistentNaming
            (
                int tag,
                char code
            )
        {
            return Fields.GetSubFieldValue(tag, code);
        }

        /// <summary>
        /// Internal formatting the record/field/subfield.
        /// </summary>
        /// <remarks>
        /// Do not use external resources!
        /// </remarks>
        [CanBeNull]
        // ReSharper disable InconsistentNaming
        public string FR
        // ReSharper restore InconsistentNaming
            (
                [NotNull] string format
            )
        {
            Code.NotNull(format, "format");

            // TODO Some caching?

            using (PftFormatter formatter = new PftFormatter())
            {
                formatter.ParseProgram(format);

                string result = formatter.FormatRecord(this);

                return result;
            }
        }

        /// <summary>
        /// Assign database name to the record.
        /// </summary>
        [NotNull]
        public MarcRecord SetDatabase
            (
                [CanBeNull] string newDatabase
            )
        {
            ThrowIfReadOnly();

            _database = newDatabase;
            Modified = true;

            return this;
        }

        /// <summary>
        /// Assign MFN to the record.
        /// </summary>
        [NotNull]
        public MarcRecord SetMfn
            (
                int newMfn
            )
        {
            ThrowIfReadOnly();
            Code.Nonnegative(newMfn, "newMfn");

            _mfn = newMfn;
            Modified = true;

            return this;
        }

        /// <summary>
        /// Change status of the record.
        /// </summary>
        [NotNull]
        public MarcRecord SetStatus
            (
                RecordStatus newStatus
            )
        {
            ThrowIfReadOnly();

            _status = newStatus;
            Modified = true;

            return this;
        }

        /// <summary>
        /// Assign version number to the record.
        /// </summary>
        [NotNull]
        public MarcRecord SetVersion
            (
                int newVersion
            )
        {
            ThrowIfReadOnly();
            Code.Nonnegative(newVersion, "newVersion");

            _version = newVersion;
            Modified = true;

            return this;
        }

        /// <summary>
        /// For Newtonsoft.Json.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDatabase()
        {
            return !string.IsNullOrEmpty(Database);
        }

        /// <summary>
        /// For Newtonsoft.Json.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeHostName()
        {
            return !string.IsNullOrEmpty(HostName);
        }


        /// <summary>
        /// For Newtonsoft.Json.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMfn()
        {
            return Mfn != 0;
        }

        /// <summary>
        /// For Newtonsoft.Json.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeStatus()
        {
            return Status != 0;
        }

        /// <summary>
        /// For Newtonsoft.Json.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeVersion()
        {
            return Version != 0;
        }

        /// <summary>
        /// Отключает верификацию.
        /// </summary>
        public static void TurnOffVerification()
        {
            ReadRecordCommand.ThrowOnEmptyRecord = false;
            ReadRecordCommand.ThrowOnVerify = false;
            FieldValue.ThrowOnVerify = false;
            SubFieldValue.ThrowOnVerify = false;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Log.Trace("MarcRecord::RestoreFromStream");

            Database = reader.ReadNullableString();
            Mfn = reader.ReadPackedInt32();
            Status = (RecordStatus)reader.ReadByte();
            Version = reader.ReadPackedInt32();
            Fields.RestoreFromStream(reader);
            Description = reader.ReadNullableString();
            SortKey = reader.ReadNullableString();
            Index = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            Log.Trace("MarcRecord::SaveToStream");

            writer.WriteNullable(Database);
            writer.WritePackedInt32(Mfn);
            writer.Write((byte)Status);
            writer.WritePackedInt32(Version);
            Fields.SaveToStream(writer);
            writer.WriteNullable(Description);
            writer.WriteNullable(SortKey);
            writer.WriteNullable(Index);
        }

        #endregion

        #region IReadOnly<T> members

        // ReSharper disable InconsistentNaming
        internal bool _readOnly;
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Whether the record read-only?
        /// </summary>
        [JsonIgnore]
        public bool ReadOnly
        {
            get { return _readOnly; }
        }

        /// <summary>
        /// Creates read-only clone of the record.
        /// </summary>
        public MarcRecord AsReadOnly()
        {
            MarcRecord result = Clone();
            result.SetReadOnly();

            return result;
        }

        /// <summary>
        /// Marks the record as read-only.
        /// </summary>
        public void SetReadOnly()
        {
            _readOnly = true;
            Fields.SetReadOnly();
        }

        /// <summary>
        /// Throws if read only.
        /// </summary>
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Log.Error("MarcRecord::ThrowIfReadOnly");

                throw new ReadOnlyException();
            }
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Log.Trace("MarcRecord::Verify");

            Verifier<MarcRecord> verifier = new Verifier<MarcRecord>
                (
                    this,
                    throwOnError
                );

            foreach (RecordField field in Fields)
            {
                verifier.Assert
                    (
                        field.Verify(throwOnError),
                        "Field " + field.Tag
                    );
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return ProtocolText.EncodeRecord(this);
        }

        #endregion
    }
}
