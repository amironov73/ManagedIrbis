// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SemiConnectedClient.cs -- 
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

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Connected IRBIS client with some local functionality
    /// </summary>
    public class SemiConnectedClient
        : ConnectedClient
    {
        #region Private members

        #endregion

        #region IrbisProvider members

        /// <inheritdoc cref="IrbisProvider.FormatRecord" />
        public override string FormatRecord
            (
                MarcRecord record,
                string format
            )
        {
            Code.NotNull(record, "record");
            Code.NotNull(format, "format");

            // TODO some caching

            PftProgram program = PftUtility.CompileProgram(format);
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            context.SetProvider(this);
            program.Execute(context);
            string result = context.GetProcessedOutput();

            return result;
        }

        #endregion
    }
}
