// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FlcProcessor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Flc
{
    /// <summary>
    /// FLC processor.
    /// </summary>
    public sealed class FlcProcessor
    {
        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Check the record.
        /// </summary>
        [NotNull]
        public FlcResult CheckRecord
            (
                [NotNull] IrbisConnection connection,
                [NotNull] MarcRecord record,
                [NotNull] string format
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(format, format);

            string text = connection.FormatRecord
                (
                    format,
                    record
                );
            FlcResult result = FlcResult.Parse(text);

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
