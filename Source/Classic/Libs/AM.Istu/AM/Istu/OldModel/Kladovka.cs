// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Kladovka.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Configuration;
using AM.Data;
using AM.Logging;

using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Linq;
using BLToolkit.Mapping;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class Kladovka
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Table "attendances"
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Table<AttendanceRecord> Attendances
        {
            get { return DB.GetTable<AttendanceRecord>(); }
        }

        /// <summary>
        /// Connection.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Database connection.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public DbManager DB { get; private set; }

        /// <summary>
        /// Table "podsob".
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Table<PodsobRecord> Podsob
        {
            get { return DB.GetTable<PodsobRecord>(); }
        }

        /// <summary>
        /// Table "readers".
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Table<ReaderRecord> Readers
        {
            get { return DB.GetTable<ReaderRecord>(); }
        }

        /// <summary>
        /// Table "translator".
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Table<TranslatorRecord> Translator
        {
            get { return DB.GetTable<TranslatorRecord>(); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Kladovka()
        {
            DB = new DbManager();
            Connection = IrbisConnectionUtility.GetClientFromConfig();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Kladovka
            (
                [JetBrains.Annotations.NotNull] DbManager db,
                [JetBrains.Annotations.NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(db, "db");
            Code.NotNull(connection, "connection");

            DB = db;
            Connection = connection;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Kladovka
            (
                [JetBrains.Annotations.NotNull] string suffix,
                [JetBrains.Annotations.NotNull] string irbisConnectionString
            )
        {
            Code.NotNullNorEmpty(suffix, "suffix");
            Code.NotNullNorEmpty(irbisConnectionString, "irbisConnectionString");

            DB = new DbManager(suffix);
            Connection = new IrbisConnection(irbisConnectionString);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Create attendance.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Kladovka CreateAttendance
            (
                [JetBrains.Annotations.NotNull] AttendanceRecord attendance
            )
        {
            Code.NotNull(attendance, "attendance");

            Query<AttendanceRecord>.Insert
                (
                    Attendances.DataContextInfo,
                    attendance
                );

            return this;
        }

        /// <summary>
        /// Create podsob record.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Kladovka CreatePodsob
            (
                [JetBrains.Annotations.NotNull] PodsobRecord podsob
            )
        {
            Code.NotNull(podsob, "podsob");

            Query<PodsobRecord>.Insert
                (
                    Podsob.DataContextInfo,
                    podsob
                );

            return this;
        }

        /// <summary>
        /// Create reader.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Kladovka CreateReader
            (
                [JetBrains.Annotations.NotNull] ReaderRecord reader
            )
        {
            Code.NotNull(reader, "reader");

            Query<ReaderRecord>.Insert
                (
                    Readers.DataContextInfo,
                    reader
                );

            return this;
        }

        /// <summary>
        /// Find records using specified expression.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public int[] FindRecords
            (
                [JetBrains.Annotations.NotNull] string format,
                params object[] args
            )
        {
            Code.NotNullNorEmpty(format, "format");

            int[] result = Connection.Search(format, args);

            return result;
        }

        /// <summary>
        /// Find podsob record by barcode or RFID.
        /// </summary>
        [CanBeNull]
        public PodsobRecord FindPodsobByBarcode
            (
                [JetBrains.Annotations.NotNull] string barcode
            )
        {
            Code.NotNullNorEmpty(barcode, "barcode");

            int inventory;
            PodsobRecord result;
            TranslatorRecord translator = GetTranslatorRecord(barcode);
            if (ReferenceEquals(translator, null))
            {
                if (!NumericUtility.TryParseInt32(barcode, out inventory))
                {
                    return null;
                }
                result = GetPodsobRecord(inventory);
            }
            else
            {
                inventory = translator.Inventory;
                result = GetPodsobRecord(inventory);
            }
            if (ReferenceEquals(result, null))
            {
                result = new PodsobRecord();
            }
            int[] found = FindRecords
                (
                    "\"IN={0}\"",
                    inventory.ToInvariantString()
                );
            if (found.Length == 0)
            {
                return null;
            }
            MarcRecord record = ReadMarcRecord(found[0]);
            result.Record = record;

            return result;
        }

        /// <summary>
        /// Get podsob record for the inventory number.
        /// </summary>
        [JetBrains.Annotations.CanBeNull]
        public PodsobRecord GetPodsobRecord
            (
                int inventory
            )
        {
            PodsobRecord result = Podsob.FirstOrDefault
                (
                    rec => rec.Inventory == inventory
                );

            return result;
        }

        /// <summary>
        /// Get translator record for the barcode or rfid.
        /// </summary>
        [JetBrains.Annotations.CanBeNull]
        public TranslatorRecord GetTranslatorRecord
            (
                [JetBrains.Annotations.CanBeNull] string barcode
            )
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return null;
            }

            TranslatorRecord result = Translator.FirstOrDefault
                (
                    rec => rec.Inventory.ToString() == barcode
                           || rec.Barcode == barcode
                           || rec.Rfid == barcode
                );

            return result;
        }

        /// <summary>
        /// Read <see cref="MarcRecord"/> for given MFN.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public MarcRecord ReadMarcRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            MarcRecord result = Connection.ReadRecord(mfn);
            result.Description = Connection.FormatRecord("@brief", mfn);

            return result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            DB.Dispose();
        }

        #endregion
    }
}
