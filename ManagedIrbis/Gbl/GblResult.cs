/* GblResult.cs -- result of GBL execution
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using AM;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Network;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Result of GBL execution.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblResult
    {
        #region Properties

        /// <summary>
        /// Момент начала обработки.
        /// </summary>
        public DateTime TimeStarted { get; set; }

        /// <summary>
        /// Всего времени затрачено (с момента начала обработки).
        /// </summary>
        public TimeSpan TimeElapsed { get; set; }

        /// <summary>
        /// Отменено пользователем.
        /// </summary>
        // TODO implement
        public bool Canceled { get; set; }

        /// <summary>
        /// Исключение (если возникло).
        /// </summary>
        // TODO implement
        public Exception Exception { get; set; }

        /// <summary>
        /// Предполагалось обработать записей.
        /// </summary>
        // TODO implement
        public int RecordsSupposed { get; set; }

        /// <summary>
        /// Обработано записей.
        /// </summary>
        public int RecordsProcessed { get; set; }

        /// <summary>
        /// Успешно обработано записей.
        /// </summary>
        public int RecordsSucceeded { get; set; }

        /// <summary>
        /// Ошибок при обработке записей.
        /// </summary>
        public int RecordsFailed { get; set; }

        /// <summary>
        /// Результаты для каждой записи.
        /// </summary>
        public ProtocolLine[] Protocol { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse server response.
        /// </summary>
        public void Parse
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            Protocol = ProtocolLine.Parse(response);
            RecordsProcessed = Protocol.Length;
            RecordsSucceeded = Protocol.Count(line => line.Success);
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
            return string.Format
                (
                    "RecordsProcessed: {0}, Canceled: {1}",
                    RecordsProcessed,
                    Canceled
                );
        }

        #endregion
    }
}
