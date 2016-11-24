/* HtmlDriver.cs -- 
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
    public sealed class HtmlDriver
        : TextDriver
    {
        #region Properties

        /// <summary>
        /// Name of the driver.
        /// </summary>
        public override string Name { get { return "HTML"; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public HtmlDriver
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
            Output.Write
                (
                    "<b>"
                    + text
                    + "</b>"
                );

            return this;
        }

        /// <inheritdoc/>
        public override TextDriver Italic
            (
                string text
            )
        {
            Output.Write
                (
                    "<i>"
                    + text
                    + "</i>"
                );

            return this;
        }

        /// <inheritdoc/>
        public override TextDriver NewParagraph()
        {
            Output.WriteLine("<p>");

            return this;
        }

        /// <inheritdoc/>
        public override TextDriver Underline
            (
                string text
            )
        {
            Output.Write
                (
                    "<u>"
                    + text
                    + "</u>"
                );

            return this;
        }

        #endregion

        #region Object members

        #endregion
    }
}
