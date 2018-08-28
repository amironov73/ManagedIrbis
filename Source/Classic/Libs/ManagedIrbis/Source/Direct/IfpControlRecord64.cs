// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IfpControlRecord64.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Runtime.InteropServices;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Управляющая запись IFP-файла в ИРБИС64.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public sealed class IfpControlRecord64
    {
        #region Constants

        /// <summary>
        /// Размер управляющей записи (байты).
        /// </summary>        
        public const int RecordSize = 20;

        #endregion

        #region Properties

        /// <summary>
        /// Ссылка на свободное место в IFP-файле.
        /// </summary>
        public long NextOffset { get; set; }

        /// <summary>
        /// Количество блоков в N01 файле.
        /// </summary>
        public int NodeBlockCount { get; set; }

        /// <summary>
        /// Количество блоков в L01 файле.
        /// </summary>
        public int LeafBlockCount { get; set; }

        /// <summary>
        /// Резерв.
        /// </summary>
        public int Reserved { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Read the control record from specified stream.
        /// </summary>
        [NotNull]
        public static IfpControlRecord64 Read
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            IfpControlRecord64 result = new IfpControlRecord64()
            {
                NextOffset = stream.ReadInt64Network(),
                NodeBlockCount = stream.ReadInt32Network(),
                LeafBlockCount = stream.ReadInt32Network(),
                Reserved = stream.ReadInt32Network()
            };

            return result;
        }

        /// <summary>
        /// Write the control record to specified stream.
        /// </summary>
        public void Write
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            stream.WriteInt64Network(NextOffset);
            stream.WriteInt32Network(NodeBlockCount);
            stream.WriteInt32Network(LeafBlockCount);
            stream.WriteInt32Network(Reserved);
        }

        #endregion
    }
}
