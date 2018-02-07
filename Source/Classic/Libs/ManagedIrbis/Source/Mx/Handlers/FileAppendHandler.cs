// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FileAppendHandler.cs -- 
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

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Mx.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FileAppendHandler
        : FileWriteHandler
    {
        #region Properties

        /// <inheritdoc cref= "FileWriteHandler.Prefix" />
        public override string Prefix
        {
            get { return "|>>"; }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        [CanBeNull]
        private StreamWriter _writer;

        #endregion

        #region Public methods

        #endregion

        #region MxHandler members

        /// <inheritdoc cref="FileWriteHandler.BeginOutput" />
        public override void BeginOutput
            (
                MxExecutive executive
            )
        {
            Encoding encoding = Encoding ?? Encoding.UTF8;

            if (!ReferenceEquals(_writer, null))
            {
                _writer.Dispose();
                _writer = null;
            }

            if (!string.IsNullOrEmpty(FileName))
            {
                _writer = TextWriterUtility.Append(FileName, encoding);
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
