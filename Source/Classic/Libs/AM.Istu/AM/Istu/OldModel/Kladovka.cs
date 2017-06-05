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
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Kladovka
            (
                [JetBrains.Annotations.NotNull] DbManager db
            )
        {
            Code.NotNull(db, "db");

            DB = db;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Kladovka
            (
                [JetBrains.Annotations.NotNull] string suffix
            )
        {
            Code.NotNullNorEmpty(suffix, "suffix");

            DB= new DbManager(suffix);
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
                    rec => rec.Barcode == barcode
                           || rec.Rfid == barcode
                );

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
