/* MstRecordLeader32.cs
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using AM.IO;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("MFN={Mfn}, Length={Length}, NVF={Nvf}, Status={Status}")]
    public sealed class MstRecordLeader32
    {
        #region Constants

        /// <summary>
        /// Фиксированный размер лидера записи.
        /// </summary>
        public const int LeaderSize = 18;

        #endregion

        #region Properties

        /// <summary>
        /// Номер записи в  файле документов.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Длина записи (всегда четное число).
        /// </summary>
        public int Length { get; set; }

        public int PreviousBlock { get; set; }

        public int PreviousOffset { get; set; }

        /// <summary>
        /// Смещение (базовый адрес) полей
        /// переменной длины (это общая часть
        /// лидера и справочника записи в байтах).
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        /// Число полей в записи (т.е. число входов
        /// в справочнике).
        /// </summary>
        public int Nvf { get; set; }

        /// <summary>
        /// Индикатор записи (логически удаленная и т.п.).
        /// </summary>
        public int Status { get; set; }

        #endregion

        #region Public methods

        public static MstRecordLeader32 Read
            (
                Stream stream
            )
        {
            MstRecordLeader32 result = new MstRecordLeader32
            {
                Mfn = stream.ReadInt32Host(),
                Length = stream.ReadInt16Host(),
                PreviousBlock = stream.ReadInt32Host(),
                PreviousOffset = stream.ReadInt16Host(),
                Base = stream.ReadInt16Host(),
                Nvf = stream.ReadInt16Host(),
                Status = stream.ReadInt16Network()
            };

            //Debug.Assert(result.Base ==
            //    (LeaderSize + result.Nvf * MstDictionaryEntry32.EntrySize));

            return result;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format
                (
                    "Mfn: {0}, Length: {1}, "
                  + "Base: {2}, Nvf: {3}, Status: {4} ",
                    Mfn,
                    Length,
                    Base,
                    Nvf,
                    Status
                );
        }

        #endregion
    }
}
