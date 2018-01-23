// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExportCommand.cs -- 
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
using ManagedIrbis.ImportExport;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ExportCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExportCommand()
            : base("export")
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

            string fileName = null;
            if (arguments.Length != 0)
            {
                fileName = arguments[0].Text;
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                using (StreamWriter writer
                    = TextWriterUtility.Create(fileName, IrbisEncoding.Utf8))
                {
                    foreach (MxRecord mxRecord in executive.Records)
                    {
                        MarcRecord record = executive.Provider.ReadRecord(mxRecord.Mfn);
                        if (!ReferenceEquals(record, null))
                        {
                            string text = record.ToPlainText();
                            writer.Write(text);
                            writer.WriteLine("*****");
                        }
                    }
                }
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
