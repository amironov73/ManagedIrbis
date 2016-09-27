/* PftToken.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Token.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Kind} {Text} {Line} {Column}")]
    public sealed class PftToken
    {
        #region Properties

        /// <summary>
        /// Column number.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Token kind.
        /// </summary>
        public PftTokenKind Kind { get; set; }

        /// <summary>
        /// Line number.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Token text.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftToken()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftToken
            (
                PftTokenKind kind,
                int line,
                int column,
                string text
            )
        {
            Kind = kind;
            Line = line;
            Column = column;
            Text = text;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return Text.ToVisibleString();
        }

        #endregion
    }
}
