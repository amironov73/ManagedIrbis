/* SuggestingTextBox.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Suggestions
{
    /// <summary>
    /// <see cref="TextBox"/> with suggesting facility.
    /// </summary>
    [PublicAPI]
    public partial class SuggestingTextBox
        : TextBox
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SuggestingTextBox()
        {
            InitializeComponent();
        }

        #endregion
    }
}
