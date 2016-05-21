/* IrbisRecord.cs -- MARC-record
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// MARC-record
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("[{Database}] MFN={Mfn} ({Version})")]
    public sealed class IrbisRecord
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// MFN записи
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Статус записи: удалена, блокирована и т.д.
        /// </summary>
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Версия записи. Нумеруется с нуля.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Смещение предыдущей версии записи.
        /// </summary>
        public long PreviousOffset { get; set; }

        /// <summary>
        /// Поля записи.
        /// </summary>
        public RecordFieldCollection Fields
        {
            get { return _fields; }
        }

        /// <summary>
        /// Признак удалённой записи.
        /// </summary>
        public bool Deleted
        {
            get { return ((Status & RecordStatus.LogicallyDeleted) != 0); }
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
        public string Description { get; set; }

        /// <summary>
        /// Используется при сортировке записей.
        /// </summary>
        [CanBeNull]
        public string SortKey { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [CanBeNull]
        public object UserData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private readonly RecordFieldCollection _fields
            = new RecordFieldCollection();

        #endregion

        #region Public methods

        /// <summary>
        /// Compares two records.
        /// </summary>
        public static int Compare
            (
                [NotNull] IrbisRecord record1,
                [NotNull] IrbisRecord record2
            )
        {
            Code.NotNull(() => record1);
            Code.NotNull(() => record2);

            int result = (int) record1.Status - (int) record2.Status;
            if (result != 0)
            {
                return result;
            }
            result = record1.Fields.Count - record2.Fields.Count;
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
            Code.NotNull(() => reader);

            Database = reader.ReadNullableString();
            Mfn = reader.ReadPackedInt32();
            Status = (RecordStatus) reader.ReadByte();
            Version = reader.ReadPackedInt32();
            Fields.RestoreFromStream(reader);
            Description = reader.ReadNullableString();
            SortKey = reader.ReadNullableString();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(() => writer);

            writer.WriteNullable(Database);
            writer.WritePackedInt32(Mfn);
            writer.Write((byte) Status);
            writer.WritePackedInt32(Version);
            Fields.SaveToStream(writer);
            writer.WriteNullable(Description);
            writer.WriteNullable(SortKey);
        }

        #endregion

        #region Object members

        #endregion
    }
}
