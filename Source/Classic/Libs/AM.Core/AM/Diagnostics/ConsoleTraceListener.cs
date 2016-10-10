﻿/* ConsoleTraceListener.cs -- console TRACE listener.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !NETCORE

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