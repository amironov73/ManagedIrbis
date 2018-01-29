// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DbCommand.cs -- 
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DbCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DbCommand()
            : base("db")
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region MxCommand members

        /// <inheritdoc cref="MxCommand.Execute" />
        public override bool Execute
        (
            MxExecutive executive,
            MxArgument[] arguments
        )
        {
            OnBeforeExecute();

            if (!executive.Provider.Connected)
            {
                executive.WriteLine("Not connected");
                return false;
            }

            string saveDatabase = executive.Provider.Database;
            string dbName = null;
            if (arguments.Length != 0)
            {
                dbName = arguments[0].Text;
            }

            if (!string.IsNullOrEmpty(dbName))
            {
                executive.Provider.Database = dbName;
            }

            try
            {
                int maxMfn = executive.Provider.GetMaxMfn() - 1;
                executive.WriteMessage(string.Format
                    (
                        "DB={0}, Max MFN={1}",
                        executive.Provider.Database,
                        maxMfn
                    ));
            }
            catch
            {
                executive.WriteError(string.Format
                    (
                        "Error changing DB, restoring to {0}",
                        saveDatabase
                    ));
                executive.Provider.Database = saveDatabase;
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
