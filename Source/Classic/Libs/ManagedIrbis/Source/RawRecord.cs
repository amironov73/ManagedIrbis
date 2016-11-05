/* RawRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AM;
using AM.Text;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Raw (not decoded) record.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("[{Database}] MFN={Mfn} ({Version})")]
#endif
    public sealed class RawRecord
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Признак удалённой записи.
        /// </summary>
        public bool Deleted
        {
            get { return (Status & RecordStatus.LogicallyDeleted) != 0; }
            set
            {
                if (value)
                {
                    Status |= RecordStatus.LogicallyDeleted;
                }
                else
                {
                    Status &= ~RecordStatus.LogicallyDeleted;
                }
            }
        }

        /// <summary>
        /// Version.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Lines of text.
        /// </summary>
        [CanBeNull]
        public string[] Lines { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static void _AppendIrbisLine
            (
                StringBuilder builder,
                string delimiter,
                string format,
                params object[] args
            )
        {
            builder.AppendFormat(format, args);
            builder.Append(delimiter);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode record to text.
        /// </summary>
        [NotNull]
        public string EncodeRecord()
        {
            return EncodeRecord(IrbisText.IrbisDelimiter);
        }

        /// <summary>
        /// Encode record to text.
        /// </summary>
        [NotNull]
        public string EncodeRecord
            (
                [CanBeNull] string delimiter
            )
        {
            StringBuilder result = new StringBuilder();

            _AppendIrbisLine
                (
                    result,
                    delimiter,
                    "{0}#{1}",
                    Mfn,
                    (int)Status
                );
            _AppendIrbisLine
                (
                    result,
                    delimiter,
                    "0#{0}",
                    Version
                );

            if (!ReferenceEquals(Lines, null))
            {
                foreach (string line in Lines)
                {
                    result.Append(line);
                    result.Append(delimiter);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Parse MFN, status and version of the record
        /// </summary>
        [NotNull]
        public static RawRecord ParseMfnStatusVersion
            (
                [NotNull] string line1,
                [NotNull] string line2,
                [NotNull] RawRecord record
            )
        {
            Code.NotNullNorEmpty(line1, "line1");
            Code.NotNullNorEmpty(line2, "line2");
            Code.NotNull(record, "record");

            Regex regex = new Regex(@"^(-?\d+)\#(\d*)?");
            Match match = regex.Match(line1);
            record.Mfn = Math.Abs(int.Parse(match.Groups[1].Value));
            if (match.Groups[2].Length > 0)
            {
                record.Status = (RecordStatus)int.Parse
                    (
                        match.Groups[2].Value
                    );
            }
            match = regex.Match(line2);
            if (match.Groups[2].Length > 0)
            {
                record.Version = int.Parse(match.Groups[2].Value);
            }

            return record;
        }

        /// <summary>
        /// Parse text.
        /// </summary>
        [NotNull]
        public static RawRecord Parse
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            string[] lines = IrbisText
                .SplitIrbisToLines(text);

            if (lines[0] == lines[1])
            {
                lines = lines.GetSpan(1);
            }

            RawRecord result = Parse(lines);

            return result;
        }

        /// <summary>
        /// Parse text lines.
        /// </summary>
        [NotNull]
        public static RawRecord Parse
            (
                [NotNull] string[] lines
            )
        {
            Code.NotNull(lines, "lines");

            if (lines.Length < 2)
            {
                throw new IrbisException();
            }

            string line1 = lines[0];
            string line2 = lines[1];
            lines = lines.GetSpan(2);

            RawRecord result = new RawRecord
            {
                Lines = lines
            };

            ParseMfnStatusVersion
                (
                    line1,
                    line2,
                    result
                );

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return EncodeRecord(Environment.NewLine);
        }

        #endregion
    }
}
