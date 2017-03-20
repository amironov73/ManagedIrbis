﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportContext.cs -- 
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

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ReportContext
    {
        #region Properties

        /// <summary>
        /// Abstract client.
        /// </summary>
        [NotNull]
        public AbstractClient Client { get; set; }

        /// <summary>
        /// Current record.
        /// </summary>
        [CanBeNull]
        public MarcRecord CurrentRecord { get; internal set; }

        /// <summary>
        /// Record index.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Records.
        /// </summary>
        [NotNull]
        public NonNullCollection<MarcRecord> Records { get; private set; }

        /// <summary>
        /// Output.
        /// </summary>
        [NotNull]
        public ReportOutput Output { get; private set; }

        /// <summary>
        /// Variables.
        /// </summary>
        [NotNull]
        public ReportVariableManager Variables { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportContext
            (
                [NotNull] AbstractClient client
            )
        {
            Code.NotNull(client, "client");

            Variables = new ReportVariableManager();
            Records = new NonNullCollection<MarcRecord>();
            Output = new ReportOutput();
            Client = client;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
