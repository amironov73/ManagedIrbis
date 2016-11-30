// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PlainTextDriver.cs -- 
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

namespace ManagedIrbis.Pft.Infrastructure.Text
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PlainTextDriver
        : TextDriver
    {
        #region Properties

        /// <summary>
        /// Name of the driver.
        /// </summary>
        public override string Name { get { return "Plain text"; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextDriver
            (
                [NotNull] PftOutput output
            )
            : base(output)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods
        
        #endregion

        #region TextDriver members

        /// <inheritdoc/>
        public override TextDriver Bold
            (
                string text
            )
        {
            Output.Write(text);

            return this;
        }

        /// <inheritdoc/>
        public override TextDriver Italic
            (
                string text
            )
        {
            Output.Write(text);

            return this;
        }

        /// <inheritdoc/>
        public override TextDriver NewParagraph()
        {
            Output.WriteLine();

            return this;
        }

        /// <inheritdoc/>
        public override TextDriver Underline
            (
                string text
            )
        {
            Output.Write(text);

            return this;
        }

        #endregion

        #region Object members

        #endregion
    }
}
