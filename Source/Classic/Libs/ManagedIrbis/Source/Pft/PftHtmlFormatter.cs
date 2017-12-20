// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftHtmlFormatter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PftHtmlFormatter
        : PftFormatter
    {
        #region Properties

        /// <summary>
        /// Text separator.
        /// </summary>
        [NotNull]
        public PftTextSeparator Separator { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftHtmlFormatter()
        {
            Separator = new PftTextSeparator();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftHtmlFormatter
            (
                [NotNull] PftContext context
            )
            : base(context)
        {
            Separator = new PftTextSeparator();
        }

        #endregion

        #region PftFormatter members

        /// <inheritdoc cref="PftFormatter.ParseProgram" />
        public override void ParseProgram
            (
                string source
            )
        {
            Code.NotNull(source, "source");

            if (Separator.SeparateText(source))
            {
                Log.Error
                    (
                        "PftHtmlFormatter::ParseProgram: "
                        + "can't separate text"
                    );

                throw new PftSyntaxException();
            }

            string prepared = Separator.Accumulator;

            base.ParseProgram(prepared);
        }

        #endregion
    }
}
