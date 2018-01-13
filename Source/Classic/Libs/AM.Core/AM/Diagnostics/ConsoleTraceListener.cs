// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConsoleTraceListener.cs -- console TRACE listener.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !UAP

#region Using directives

using System;
using System.Diagnostics;

using JetBrains.Annotations;

#endregion

namespace AM.Diagnostics
{
    /// <summary>
    /// Console TRACE listener.
    /// </summary>
    [PublicAPI]
    public sealed class ConsoleTraceListener
        : TextWriterTraceListener
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ConsoleTraceListener"/> class.
        /// </summary>
        public ConsoleTraceListener()
            : base(Console.Out)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ConsoleTraceListener"/> class.
        /// </summary>
        /// <param name="initializationData">
        /// The initialization data.</param>
        /// <remarks>Called by runtime.</remarks>
        public ConsoleTraceListener(string initializationData)
            : this()
        {
            Trace.WriteLine(initializationData);
        }

        #endregion
    }
}

#endif
