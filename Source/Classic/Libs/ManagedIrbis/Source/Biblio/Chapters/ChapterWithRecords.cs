// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChapterWithRecords.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Pft;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ChapterWithRecords
        : BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Records.
        /// </summary>
        [NotNull]
        public RecordCollection Records { get; private set; }

        /// <summary>
        /// Duplicates.
        /// </summary>
        [NotNull]
        public RecordCollection Duplicates { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChapterWithRecords()
        {
            Records = new RecordCollection();
            Duplicates = new RecordCollection();
        }

        #endregion

        #region Private members

        /// <summary>
        /// Format records.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] FormatRecords
            (
                [NotNull] BiblioContext context,
                [NotNull] int[] mfns,
                [NotNull] string format
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(mfns, "mfns");
            Code.NotNullNorEmpty(format, "format");

            if (mfns.Length == 0)
            {
                return StringUtility.EmptyArray;
            }

            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");

            using (IPftFormatter formatter
                = processor.AcquireFormatter(context))
            {
                formatter.ParseProgram(format);
                string[] formatted = formatter.FormatRecords(mfns);
                if (formatted.Length != mfns.Length)
                {
                    throw new IrbisException();
                }

                return formatted;
            }
        }

        /// <summary>
        /// Format records.
        /// </summary>
        public string[] FormatRecords
            (
                [NotNull] BiblioContext context,
                [NotNull] string format
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(format, "format");

            RecordCollection records = Records
                .ThrowIfNull("Records");
            int[] mfns = records.Select(r => r.Mfn).ToArray();
            string[] result = FormatRecords(context, mfns, format);

            return result;
        }

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
