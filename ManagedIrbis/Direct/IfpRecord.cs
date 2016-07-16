/* IfpRecord.cs -- inverted file record
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Direct
{
    /// <summary>
    /// Inverted file record
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IfpRecord
    {
        #region Constants

        /// <summary>
        /// Число ссылок на термин, после превышения которого
        /// используется специальный блок ссылок.
        /// </summary>
        public const int MinPostingsInBlock = 256;

        #endregion

        #region Properties

        /// <summary>
        /// Младшее слово смещения на следующую запись(если нет - 0).
        /// </summary>
        public int LowOffset { get; set; }

        /// <summary>
        /// Старшее слово смещения на следующую запись(если нет - 0).
        /// </summary>
        /// <remarks>Признак последнего блока – LOW=HIGH= -1.</remarks>
        public int HighOffset { get; set; }

        /// <summary>
        /// Ообщее число ссылок для данного термина
        /// (только в первой записи);
        /// число ссылок в данном блоке(в следующих записях).
        /// </summary>
        public int TotalLinkCount { get; set; }

        /// <summary>
        /// Число ссылок в данном блоке.
        /// </summary>
        public int BlockLinkCount { get; set; }

        /// <summary>
        /// Вместимость записи в ссылках.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Собственно ссылки.
        /// </summary>
        public List<TermLink> Links { get { return _links; } }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private readonly List<TermLink> _links = new List<TermLink>();

        #endregion

        #region Public methods

        /// <summary>
        /// Считываем из потока.
        /// </summary>
        [NotNull]
        public static IfpRecord Read
            (
                [NotNull] Stream stream,
                long offset
            )
        {
            Code.NotNull(stream, "stream");

            //new ObjectDumper()
            //    .DumpStream(stream, offset, 100);

            stream.Position = offset;

            IfpRecord result = new IfpRecord
                {
                    LowOffset = stream.ReadInt32Network(),
                    HighOffset = stream.ReadInt32Network(),
                    TotalLinkCount = stream.ReadInt32Network(),
                    BlockLinkCount = stream.ReadInt32Network(),
                    Capacity = stream.ReadInt32Network()
                };

            for (int i = 0; i < result.BlockLinkCount; i++)
            {
                TermLink link = TermLink.Read(stream);
                result.Links.Add(link);
            }

            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (TermLink link in Links)
            {
                builder.AppendLine(link.ToString());
            }

            return string.Format
                (
                    "LowOffset: {0}, HighOffset: {1}, TotalLinkCount: {2}, "
                    + "BlockLinkCount: {3}, Capacity: {4}\r\nItems: {5}", 
                    LowOffset, 
                    HighOffset,
                    TotalLinkCount,
                    BlockLinkCount,
                    Capacity,
                    builder
                );
        }

        #endregion
    }
}
