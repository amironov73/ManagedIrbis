/* ProgressBarUtility.cs -- useful extensions for ProgressBar
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace IrbisUI.Extensions
{
    /// <summary>
    /// Useful extension for <see cref="ProgressBar"/>.
    /// </summary>
    [PublicAPI]
    public static class ProgressBarUtility
    {
        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Shows or hides marquee for specified
        /// <paramref name="progressBar"/>.
        /// </summary>
        public static void ShowMarquee
            (
                [NotNull] this ProgressBar progressBar,
                bool show
            )
        {
            Code.NotNull(progressBar, "progressBar");

            progressBar.Style = show
                ? ProgressBarStyle.Marquee
                : ProgressBarStyle.Blocks;
        }

        #endregion
    }
}
