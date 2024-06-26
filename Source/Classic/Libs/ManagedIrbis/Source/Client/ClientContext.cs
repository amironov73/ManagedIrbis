﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ClientContext.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ClientContext
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Databases.
        /// </summary>
        public DatabaseInfo[] Databases { get; set; }

        /// <summary>
        /// Search scenarios.
        /// </summary>
        public SearchScenario[] Scenarios { get; set; }

        /// <summary>
        /// Formats.
        /// </summary>
        public MenuFile[] Formats { get; set; }

        /// <summary>
        /// Worksheets.
        /// </summary>
        public MenuFile[] Worksheets { get; set; }

        /// <summary>
        /// Found records.
        /// </summary>
        public int[] FoundRecords { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClientContext
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Reset the context.
        /// </summary>
        public void Reset()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
