// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstRecordLeader64.cs -- leader of MST record
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Leader of MST record.
    /// </summary>
    [DebuggerDisplay("MFN={Mfn}, Length={Length}, NVF={Nvf}, Status={Status}")]
    public struct MstRecordLeader64
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
        /// Длина записи (в документации сказано, что всегда четное число,
        /// но по факту это не так).
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
        /// Число полей в записи (т. е. число входов
        /// в справочнике).
        /// </summary>
        public int Nvf { get; set; }

        /// <summary>
        /// Индикатор записи (логически удаленная и т. п.).
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Номер версии записи.
        /// </summary>
        public int Version { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Read the record leader.
        /// </summary>
        public static MstRecordLeader64 Read
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

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

            //Debug.Assert(result.Base ==
            //    (LeaderSize + result.Nvf * MstDictionaryEntry64.EntrySize));

            return result;
        }

        /// <summary>
        /// Write the record leader.
        /// </summary>
        public void Write
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            stream.WriteInt32Network(Mfn);
            stream.WriteInt32Network(Length);
            stream.WriteInt64Network(Previous);
            stream.WriteInt32Network(Base);
            stream.WriteInt32Network(Nvf);
            stream.WriteInt32Network(Version);
            stream.WriteInt32Network(Status);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
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
