/* MstRecordLeader64.cs
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
    [Serializable]
    [ComVisible(false)]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("MFN={Mfn}, Length={Length}, NVF={Nvf}, Status={Status}")]
    public sealed class MstRecordLeader64
    {
        #region Constants
        
        /// <summary>
        /// Фиксированный размер лидера записи.
        /// </summary>
        public const int LeaderSize = 32;

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

        /// <summary>
        /// Ссылка на предыдущую версию записи.
        /// </summary>
        public long Previous { get; set; }

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

        /// <summary>
        /// Номер версии записи.
        /// </summary>
        public int Version { get; set; }

        #endregion

        #region Public methods

        public static MstRecordLeader64 Read(Stream stream)
        {
            MstRecordLeader64 result = new MstRecordLeader64
            {
                Mfn = stream.ReadInt32Network(),
                Length = stream.ReadInt32Network(),
                Previous = stream.ReadInt64Network(),
                Base = stream.ReadInt32Network(),
                Nvf = stream.ReadInt32Network(),
                Version = stream.ReadInt32Network(),
                Status = stream.ReadInt32Network()
            };

            Debug.Assert(result.Base == 
                (LeaderSize + result.Nvf * MstDictionaryEntry64.EntrySize));

            return result;
        }

        #endregion

        #region Object members

        public override string ToString ( )
        {
            return string.Format 
                ( 
                    "Mfn: {0}, Length: {1}, Previous: {2}, "
                  + "Base: {3}, Nvf: {4}, Status: {5}, "
                  + "Version: {6}", 
                    Mfn,
                    Length, 
                    Previous, 
                    Base, 
                    Nvf, 
                    Status, 
                    Version 
                );
        }

        #endregion
    }
}
