/* QToken.cs --
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

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// Token.
    /// </summary>
    [DebuggerDisplay("{Kind} {Text} {Position}")]
    public sealed class QToken
    {
        #region Properties

        /// <summary>
        /// Token kind.
        /// </summary>
        public QTokenKind Kind { get; set; }

        /// <summary>
        /// Token position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Token text.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public QToken()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public QToken
            (
                QTokenKind kind,
                int position,
                string text
            )
        {
            Kind = kind;
            Position = position;
            Text = text;
        }

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
