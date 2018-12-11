// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* XrfRecord64.cs
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Usingd directives

using System.Diagnostics;

using JetBrains.Annotations;

#endregion

namespace UnsafeIrbis.Direct
{
    //
    // Extract from official documentation:
    // http://sntnarciss.ru/irbis/spravka/wtcp006002000.htm
    //
    // Каждая ссылка состоит из 3-х полей:
    // Число бит Параметр
    // 32        XRF_LOW – младшее слово в 8 байтовом смещении на запись;
    // 32        XRF_HIGH– старшее слово в 8 байтовом смещении на запись;
    // 32        XRF_FLAGS – Индикатор записи в виде битовых флагов
    //           следующего содержания:
    //             BIT_LOG_DEL(1)  - логически удаленная запись;
    //             BIT_PHYS_DEL(2) - физически удаленная запись;
    //             BIT_ABSENT(4)  - несуществующая запись;
    //             BIT_NOTACT_REC(8)- неактуализированная запись;
    //             BIT_LOCK_REC(64)- заблокированная запись.
    //

    /// <summary>
    /// Contains information about record offset and status.
    /// </summary>
    [PublicAPI]
    [DebuggerDisplay("Offset={Offset}, Status={Status}")]
    public struct XrfRecord64
    {
        #region Constants

        /// <summary>
        /// Fixed record size.
        /// </summary>
        public const int RecordSize = sizeof(long) + sizeof(int);

        #endregion

        #region Properties

        /// <summary>
        /// MFN
        /// </summary>
        public int Mfn;

        /// <summary>
        /// 8-byte offset of the record in the MST file.
        /// </summary>
        public long Offset;

        /// <summary>
        /// Status of the record.
        /// </summary>
        public RecordStatus Status;

        /// <summary>
        /// Whether the record is locked?
        /// </summary>
        public bool Locked
        {
            get => (Status & RecordStatus.Locked) != 0;
            set
            {
                if (value)
                {
                    Status |= RecordStatus.Locked;
                }
                else
                {
                    Status &= ~RecordStatus.Locked;
                }
            }
        }

        /// <summary>
        /// Whether the record is deleted?
        /// </summary>
        public bool Deleted => (Status & RecordStatus.Deleted) != 0;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return $"MFN: {Mfn}, Offset: {Offset}, Status: {Status}";
        }

        #endregion
    }
}
