// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportOutput.cs -- 
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

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReportOutput
    {
        #region Properties

        /// <summary>
        /// Result text.
        /// </summary>
        [NotNull]
        public string Text { get { return _buffer.ToString(); } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportOutput()
        {
            _buffer = new StringBuilder();
        }

        #endregion

        #region Private members

        private readonly StringBuilder _buffer;

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the output.
        /// </summary>
        [NotNull]
        public ReportOutput Clear()
        {
            _buffer.Length = 0;

            return this;
        }

        /// <summary>
        /// Trim end of text.
        /// </summary>
        [NotNull]
        public ReportOutput TrimEnd()
        {
            int length = _buffer.Length;
            
            while (length != 0)
            {
                string s = _buffer.ToString(length - 1, 1);
                if (char.IsWhiteSpace(s, 0))
                {
                    length--;
                    _buffer.Length = length;
                }
                else
                {
                    break;
                }
            }

            return this;
        }

        /// <summary>
        /// Write text.
        /// </summary>
        [NotNull]
        public ReportOutput Write
            (
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                _buffer.Append(text);
            }

            return this;
        }

        #endregion
    }
}
