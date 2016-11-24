/* TextDriver.cs -- 
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
    public abstract class TextDriver
    {
        #region Properties

        /// <summary>
        /// Name of the driver.
        /// </summary>
        [NotNull]
        public virtual string Name { get { return "None"; } }

        /// <summary>
        /// Output.
        /// </summary>
        [NotNull]
        public PftOutput Output { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected TextDriver
            (
                [NotNull] PftOutput output
            )
        {
            Code.NotNull(output, "output");

            Output = output;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Bold face.
        /// </summary>
        [NotNull]
        public virtual TextDriver Bold
            (
                [NotNull] string text
            )
        {
            return this;
        }

        /// <summary>
        /// Italic.
        /// </summary>
        [NotNull]
        public virtual TextDriver Italic
            (
                [NotNull] string text
            )
        {
            return this;
        }

        /// <summary>
        /// New paragraph.
        /// </summary>
        [NotNull]
        public virtual TextDriver NewParagraph()
        {
            return this;
        }

        /// <summary>
        /// Underline.
        /// </summary>
        [NotNull]
        public virtual TextDriver Underline
            (
                [NotNull] string text
            )
        {
            return this;
        }

        #endregion

        #region Object members

        #endregion
    }
}
