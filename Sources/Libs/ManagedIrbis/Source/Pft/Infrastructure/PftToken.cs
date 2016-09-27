/* PftToken.cs --
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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Runtime;

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
        : IHandmadeSerializable
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
        [CanBeNull]
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

        #region IHandmadeSerializable members

        /// <inheritdoc/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Column = reader.ReadPackedInt32();
            Kind = (PftTokenKind) reader.ReadPackedInt32();
            Line = reader.ReadPackedInt32();
            Text = reader.ReadNullableString();
        }

        /// <inheritdoc/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32(Column)
                .WritePackedInt32((int) Kind)
                .WritePackedInt32(Line)
                .WriteNullable(Text);
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return Kind 
                + ": " 
                + Text.ToVisibleString();
        }

        #endregion
    }
}
