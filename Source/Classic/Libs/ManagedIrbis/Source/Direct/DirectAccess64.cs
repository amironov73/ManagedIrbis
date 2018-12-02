// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DirectAccess64.cs -- direct reading IRBIS64 databases
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM.Collections;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    //
    // Начиная с 2014.1
    //
    // Добавлена возможность фрагментации словаря БД.
    //
    // При значительных объемах БД (более 1 млн. записей)
    // фрагментация словаря БД существенно ускоряет актуализацию записей.
    //
    // В связи с этим в INI-файл (irbisa.ini) добавлен новый параметр
    //
    // CREATE_OLD_INVERTION_FILES=1
    //
    // который принимает значения: 1 (по умолчанию) - нет фрагментации
    // 0 - есть фрагментация.
    //
    // Использовать фрагментацию словаря (т.е. устанавливать значение 0)
    // имеет смысл при работе с БД, содержащими более 1 млн. записей.
    //
    // При включении фрагментации (CREATE_OLD_INVERTION_FILES=0)
    // для непустой БД (MFN>0) необходимо выполнить режим
    // СОЗДАТЬ СЛОВАРЬ ЗАНОВО.
    //
    // В таблицу описания БД в интерфейсе АРМа Администратор добавлена
    // строка, отражающая состояние БД в части фрагментации словаря:
    // ФРАГМЕНТАЦИЯ СЛОВАРЯ - Да/Нет.
    //

    //
    // Начиная с 2015.1
    //
    // Обеспечена возможность фрагментации файла документов
    // (по умолчанию отсутствует), что позволяет распараллелить
    // (а следовательно и ускорить) процессы одновременного редактирования
    // записей из разных диапазонов MFN. Данный режим рекомендуется
    // применять при активной книговыдаче для баз данных читателей
    // RDR и электронного каталога.
    //
    // Чтобы включить режим фрагментации файла документов, надо установить
    // в ини файле АРМ Администратор параметр
    // MST_NUM_FRAGMENTS=N
    // (где N принимает значения от 2 до 32 и определяет количество
    // фрагментов, на которые разбивается файл документов)
    // после чего сделать РЕОРГАНИЗАЦИЮ ФАЙЛА ДОКУМЕНТОВ.
    // Параметр фрагментации сохраняется непосредственно в базе данных,
    // так что все дальнейшие операции (на сервере или в АРМ Администратор),
    // кроме РЕОРГАНИЗАЦИИ ФАЙЛА ДОКУМЕНТОВ, параметр MST_NUM_FRAGMENTS
    // не используют.
    // В результате фрагментации образуется N пар файлов MST и XRF.
    // Нумерация начинается с индекса 0 и сохраняется в расширении этих файлов,
    // например, IBIS.MST1 IBIS.XRF1, причем индекс 0 не используется,
    // т.е. IBIS.MST0 пишется как IBIS.MST. Новые записи сохраняются
    // в последнем фрагменте файла документов (с индексом N–1),
    // поэтому при существенном изменении общего объема БД необходимо
    // проводить РЕОРГАНИЗАЦИЮ ФАЙЛА ДОКУМЕНТОВ (в результате чего фрагментация,
    // т.е. распределение записей по фрагментам будет выполнена заново.)
    //
    // Фактически не работает
    //

    /// <summary>
    /// Direct reading IRBIS64 databases.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DirectAccess64
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Master file.
        /// </summary>
        [NotNull]
        public MstFile64 Mst { get; private set; }

        /// <summary>
        /// Cross-references file.
        /// </summary>
        [NotNull]
        public XrfFile64 Xrf { get; private set; }

        /// <summary>
        /// Inverted (index) file.
        /// </summary>
        [NotNull]
        public InvertedFile64 InvertedFile { get; private set; }

        /// <summary>
        /// Database path.
        /// </summary>
        [NotNull]
        public string Database { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DirectAccess64
            (
                [NotNull] string masterFile
            )
            : this(masterFile, DirectAccessMode.Exclusive)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DirectAccess64
            (
                [NotNull] string masterFile,
                DirectAccessMode mode
            )
        {
            Code.NotNullNorEmpty(masterFile, "masterFile");

            Database = Path.GetFileNameWithoutExtension(masterFile);
            Mst = new MstFile64(Path.ChangeExtension(masterFile, ".mst"), mode);
            Xrf = new XrfFile64(Path.ChangeExtension(masterFile, ".xrf"), mode);
            InvertedFile = new InvertedFile64(Path.ChangeExtension(masterFile, ".ifp"), mode);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get database info.
        /// </summary>
        [NotNull]
        public DatabaseInfo GetDatabaseInfo()
        {
            int maxMfn = GetMaxMfn();
            LocalList<int> logicallyDeleted = new LocalList<int>();
            LocalList<int> physicallyDeleted = new LocalList<int>();
            LocalList<int> nonActualized = new LocalList<int>();
            LocalList<int> lockedRecords = new LocalList<int>();

            for (int mfn = 1; mfn <= maxMfn; mfn++)
            {
                XrfRecord64 record = Xrf.ReadRecord(mfn);
                if ((record.Status & RecordStatus.LogicallyDeleted) != 0)
                {
                    logicallyDeleted.Add(mfn);
                }
                if ((record.Status & RecordStatus.PhysicallyDeleted) != 0)
                {
                    physicallyDeleted.Add(mfn);
                }
                if ((record.Status & RecordStatus.NonActualized) != 0)
                {
                    nonActualized.Add(mfn);
                }
                if ((record.Status & RecordStatus.Locked) != 0)
                {
                    lockedRecords.Add(mfn);
                }
            }

            DatabaseInfo result = new DatabaseInfo
            {
                MaxMfn = maxMfn,
                DatabaseLocked = Mst.ReadDatabaseLockedFlag(),
                LogicallyDeletedRecords = logicallyDeleted.ToArray(),
                PhysicallyDeletedRecords = physicallyDeleted.ToArray(),
                NonActualizedRecords = nonActualized.ToArray(),
                LockedRecords = lockedRecords.ToArray()
            };

            return result;
        }

        /// <summary>
        /// Get max MFN for database. Not next MFN!
        /// </summary>
        public int GetMaxMfn()
        {
            return Mst.ControlRecord.NextMfn - 1;
        }

        /// <summary>
        /// Read raw record.
        /// </summary>
        [CanBeNull]
        public MstRecord64 ReadRawRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            XrfRecord64 xrfRecord;
            try
            {
                xrfRecord = Xrf.ReadRecord(mfn);
            }
            catch
            {
                return null;
            }

            if (xrfRecord.Offset == 0)
            {
                return null;
            }

            MstRecord64 result = Mst.ReadRecord(xrfRecord.Offset);

            return result;
        }

        /// <summary>
        /// Read record with given MFN.
        /// </summary>
        [CanBeNull]
        public MarcRecord ReadRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            XrfRecord64 xrfRecord;
            try
            {
                xrfRecord = Xrf.ReadRecord(mfn);
            }
            catch
            {
                return null;
            }

            if (xrfRecord.Offset == 0
                || (xrfRecord.Status & RecordStatus.PhysicallyDeleted) != 0)
            {
                return null;
            }

            MstRecord64 mstRecord = Mst.ReadRecord(xrfRecord.Offset);
            MarcRecord result = mstRecord.DecodeRecord();
            result.Database = Database;

            return result;
        }

        /// <summary>
        /// Read all versions of the record.
        /// </summary>
        [NotNull]
        public MarcRecord[] ReadAllRecordVersions
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            List<MarcRecord> result = new List<MarcRecord>();
            MarcRecord lastVersion = ReadRecord(mfn);
            if (lastVersion != null)
            {
                result.Add(lastVersion);
                while (true)
                {
                    long offset = lastVersion.PreviousOffset;
                    if (offset == 0)
                    {
                        break;
                    }
                    MstRecord64 mstRecord = Mst.ReadRecord(offset);
                    MarcRecord previousVersion = mstRecord.DecodeRecord();
                    previousVersion.Database = lastVersion.Database;
                    previousVersion.Mfn = lastVersion.Mfn;
                    result.Add(previousVersion);
                    lastVersion = previousVersion;
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Read links for the term.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public TermLink[] ReadLinks
            (
                [NotNull] string key
            )
        {
            Code.NotNull(key, "key");

            return InvertedFile.SearchExact(key);
        }

        /// <summary>
        /// Read terms.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public TermInfo[] ReadTerms
            (
                [NotNull] TermParameters parameters
            )
        {
            Code.NotNull(parameters, "parameters");

            TermInfo[] result = InvertedFile.ReadTerms(parameters);

            return result;
        }

        /// <summary>
        /// Reopen files.
        /// </summary>
        public void ReopenFiles
            (
                DirectAccessMode newMode
            )
        {
            Mst.ReopenFile(newMode);
            Xrf.ReopenFile(newMode);
            InvertedFile.ReopenFiles(newMode);
        }

        /// <summary>
        /// Simple search.
        /// </summary>
        [NotNull]
        public int[] SearchSimple
            (
                [NotNull] string key
            )
        {
            Code.NotNullNorEmpty(key, "key");

            int[] found = InvertedFile.SearchSimple(key);
            List<int> result = new List<int>();
            foreach (int mfn in found)
            {
                if (!Xrf.ReadRecord(mfn).Deleted)
                {
                    result.Add(mfn);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Simple search and read records.
        /// </summary>
        [NotNull]
        public MarcRecord[] SearchReadSimple
            (
                [NotNull] string key
            )
        {
            Code.NotNullNorEmpty(key, "key");

            int[] mfns = InvertedFile.SearchSimple(key);
            List<MarcRecord> result = new List<MarcRecord>();
            foreach (int mfn in mfns)
            {
                try
                {
                    XrfRecord64 xrfRecord = Xrf.ReadRecord(mfn);
                    if (!xrfRecord.Deleted)
                    {
                        MstRecord64 mstRecord = Mst.ReadRecord(xrfRecord.Offset);
                        if (!mstRecord.Deleted)
                        {
                            MarcRecord irbisRecord
                                = mstRecord.DecodeRecord();
                            irbisRecord.Database = Database;
                            result.Add(irbisRecord);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.TraceException
                    (
                        "DirectReader64::SearchReadSimple",
                        exception
                    );
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public void WriteRawRecord
            (
                [NotNull] MstRecord64 mstRecord
            )
        {
            Code.NotNull(mstRecord, "mstRecord");

            MstRecordLeader64 leader = mstRecord.Leader;
            int mfn = leader.Mfn;
            XrfRecord64 xrfRecord;
            if (mfn == 0)
            {
                mfn = Mst.ControlRecord.NextMfn;
                leader.Mfn = mfn;
                MstControlRecord64 control = Mst.ControlRecord;
                control.NextMfn = mfn + 1;
                Mst.ControlRecord = control;
                xrfRecord = new XrfRecord64
                {
                    Mfn = mfn,
                    Offset = Mst.WriteRecord(mstRecord),
                    Status = (RecordStatus)leader.Status
                };
            }
            else
            {
                xrfRecord = Xrf.ReadRecord(mfn);
                long previousOffset = xrfRecord.Offset;
                leader.Previous = previousOffset;
                MstRecordLeader64 previousLeader
                    = Mst.ReadLeader(previousOffset);
                previousLeader.Status = (int)RecordStatus.NonActualized;
                Mst.UpdateLeader(previousLeader, previousOffset);
                xrfRecord.Offset = Mst.WriteRecord(mstRecord);
            }
            Xrf.WriteRecord(xrfRecord);

            Mst.UpdateControlRecord(false);
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public void WriteRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            if (record.Version < 0)
            {
                record.Version = 0;
            }

            record.Version++;
            record.Status |= RecordStatus.Last | RecordStatus.NonActualized;
            MstRecord64 mstRecord64 = MstRecord64.EncodeRecord(record);
            WriteRawRecord(mstRecord64);
            record.Database = Database;
            record.Mfn = mstRecord64.Leader.Mfn;
            record.PreviousOffset = mstRecord64.Leader.Previous;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {

            Mst.Dispose();
            Xrf.Dispose();
            InvertedFile.Dispose();
        }

        #endregion
    }
}

