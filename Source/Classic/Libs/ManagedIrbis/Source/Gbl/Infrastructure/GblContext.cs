// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblContext.cs --
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
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblContext
    {
        #region Properties

        /// <summary>
        /// Current record.
        /// </summary>
        [CanBeNull]
        public MarcRecord CurrentRecord { get; set; }

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; set; }

        /// <summary>
        /// Record source.
        /// </summary>
        [NotNull]
        public RecordSource RecordSource { get; set; }

        /// <summary>
        /// Logger.
        /// </summary>
        [NotNull]
        public GblLogger Logger { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblContext()
        {
            Logger = new GblLogger();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Go to next record.
        /// </summary>
        public bool Advance()
        {
            CurrentRecord = RecordSource.GetNextRecord();

            return !ReferenceEquals(CurrentRecord, null);
        }

        #endregion

        #region Object members

        #endregion
    }
}
