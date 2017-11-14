// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExceptionUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ExceptionUtility
    {
        #region Public methods

        /// <summary>
        /// Throw exception with message.
        /// </summary>
        public static void Throw
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            string message = string.Format
                (
                    format,
                    args
                );

            Log.Trace
                (
                    "ExceptionUtility::Throw: "
                    + message
                );

            throw new Exception(message);
        }

        /// <summary>
        /// Unwrap the <see cref="AggregateException"/>
        /// (or do nothing if not aggregate).
        /// </summary>
        [NotNull]
        public static Exception Unwrap
            (
                [NotNull] Exception exception
            )
        {
            Code.NotNull(exception, "exception");

#if FW4

            AggregateException aggregate = exception as AggregateException;
            if (!ReferenceEquals(aggregate, null))
            {
                aggregate = aggregate.Flatten();

                aggregate.Handle
                    (
                        ex =>
                            {
                                Log.TraceException
                                    (
                                        "ExceptionUtility::Unwrap",
                                        ex
                                    );

                                return true;
                            }
                    );

                return aggregate.InnerExceptions[0];
            }

#endif

            return exception;
        }

        #endregion
    }
}
