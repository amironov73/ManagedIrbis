/* PftHtmlFormatter.cs -- 
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

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftFormatter members

        /// <inheritdoc/>
        public override void ParseProgram
            (
                string text
            )
        {
            Code.NotNull(text, "text");

            if (Separator.SeparateText(text))
            {
                throw new PftSyntaxException();
            }

            string prepared = Separator.Accumulator;

            base.ParseProgram(prepared);
        }

        #endregion

        #region Object members

        #endregion
    }
}
