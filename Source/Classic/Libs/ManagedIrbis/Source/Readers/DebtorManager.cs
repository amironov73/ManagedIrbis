/* DebtorManager.cs -- работа с задолжниками
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Работа с задолжниками.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{Department}")]
#endif
    public sealed class DebtorManager
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Кафедра обслуживания.
        /// </summary>
        [CanBeNull]
        public string Department { get; set; }

        /// <summary>
        /// С какой даты задолженность?
        /// </summary>
        [CanBeNull]
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// По какую дату задолженность.
        /// </summary>
        [CanBeNull]
        public DateTime? ToDate { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DebtorManager
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Получение списка задолжников.
        /// </summary>
        public DebtorInfo[] GetDebtors()
        {
            // TODO implement

            return new DebtorInfo[0];
        }

        #endregion

        #region Object members

        #endregion
    }
}
