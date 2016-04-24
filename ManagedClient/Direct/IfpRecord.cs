/* IfpRecord.cs -- inverted file record
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
    [Serializable]
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

        public int LowOffset { get; set; }

        public int HighOffset { get; set; }

        public int TotalLinkCount { get; set; }

        public int BlockLinkCount { get; set; }

        public int Capacity { get; set; }

        public List<TermLink> Links { get { return _links; } }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private readonly List<TermLink> _links = new List<TermLink>();

        #endregion

        #region Public methods

        public static IfpRecord Read
            (
                Stream stream,
                long offset
            )
        {
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
