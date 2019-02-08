// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstStat64.cs -- MST file statistics
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// MST file statistics
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MstStat64
    {
        #region Events

        /// <summary>
        /// Вызывается во время обработки записей.
        /// </summary>
        public event EventHandler Processing;

        #endregion

        #region Properties

        /// <summary>
        /// Общее количество записей в файле документов
        /// (включая удалённые).
        /// </summary>
        public long TotalRecordCount { get; set; }

        /// <summary>
        /// Количество обработанных записей.
        /// </summary>
        public long ProcessedRecords { get; set; }

        /// <summary>
        /// Общая длина файла (в байтах).
        /// </summary>
        public long TotalFileLength { get; set; }

        /// <summary>
        /// Количество физически удалённых записей.
        /// </summary>
        public long PhysicallyDeletedCount { get; set; }

        /// <summary>
        /// Количество логически удалённых записей.
        /// </summary>
        public long LogicallyDeletedCount { get; set; }

        /// <summary>
        /// Суммарный размер логически удалённых записей
        /// (без предыдущих версий).
        /// </summary>
        public long LogicallyDeletedSize { get; set; }

        /// <summary>
        /// Количество предыдущих версий записей,
        /// хранящихся в файле документов.
        /// </summary>
        public long PreviousVersionCount { get; set; }

        /// <summary>
        /// Суммарный размер предыдущих версий записей,
        /// хранящихся в файле документов.
        /// </summary>
        public long PreviousVersionSize { get; set; }

        /// <summary>
        /// Суммарный размер последних версий не удалённых записей,
        /// хранящихся в файле документов.
        /// </summary>
        public long UsefulSize { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Process the master file.
        /// </summary>
        public void ProcessFile
            (
                [NotNull] string masterFileName
            )
        {
            Code.NotNullNorEmpty(masterFileName, "masterFileName");

            string crossFileName = Path.ChangeExtension(masterFileName, IrbisCatalog.CrossReferenceExtension);
            using (FileStream xrfFile = InsistentFile.OpenForSharedRead(crossFileName))
            using (FileStream mstFile = InsistentFile.OpenForSharedRead(masterFileName))
            {
                MstControlRecord64 control = MstControlRecord64.Read(mstFile);
                TotalRecordCount = control.NextMfn - 1;
                TotalFileLength = mstFile.Length;

                for (long mfn = 1; mfn <= TotalRecordCount; mfn++)
                {
                    long mstOffset = xrfFile.ReadInt64Network();
                    int flags = xrfFile.ReadInt32Network();

                    if (mstOffset == 0 || (flags & 0x02) != 0)
                    {
                        // Запись физически отсутствует
                        PhysicallyDeletedCount++;
                        continue;
                    }

                    int useful = 0;
                    bool first = true;
                    do
                    {
                        mstFile.Position = mstOffset;
                        MstRecordLeader64 leader = MstRecordLeader64.Read(mstFile);
                        int recordLength = leader.Length;

                        if (first)
                        {
                            useful += recordLength;
                            first = false;
                        }
                        else
                        {
                            PreviousVersionCount++;
                            PreviousVersionSize += recordLength;
                        }

                        mstOffset = leader.Previous;
                    } while (mstOffset != 0);

                    if ((flags & 1) != 0)
                    {
                        // Запись логически удалена
                        LogicallyDeletedCount++;
                        LogicallyDeletedSize += useful;
                    }
                    else
                    {
                        UsefulSize += useful;
                    }

                    ProcessedRecords = mfn;
                    Processing.Raise(this);
                }
            }
        }

        #endregion
    }
}
