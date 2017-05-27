// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextPosition.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Position in <see cref="TextNavigator"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Column={Column} Line={Line} Position={Position}")]
    public sealed class TextPosition
    {
        #region Properties

        /// <summary>
        /// Column number.
        /// </summary>
        public int Column { get; private set; }

        /// <summary>
        /// Line number.
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// Absolute position.
        /// </summary>
        public int Position { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextPosition
            (
                [NotNull] TextNavigator navigator
            )
        {
            Code.NotNull(navigator, "navigator");

            Column = navigator.Column;
            Line = navigator.Line;
            Position = navigator.Position;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Line={0}, Column={1}",
                    Line,
                    Column
                );
        }

        #endregion
    }
}
