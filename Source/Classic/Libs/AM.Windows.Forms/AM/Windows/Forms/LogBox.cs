// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LogBox.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AM.Text.Output;
using CodeJam;
using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [System.ComponentModel.DesignerCategory("Code")]
    public class LogBox
        : TextBox
    {
        #region Properties

        /// <summary>
        /// Output.
        /// </summary>
        [NotNull]
        public AbstractOutput Output { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LogBox()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor

            ReadOnly = true;
            BackColor = SystemColors.Window;
            Multiline = true;
            WordWrap = true;
            ScrollBars = ScrollBars.Vertical;

            Output = new TextBoxOutput(this);

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Set output
        /// </summary>
        public void SetOutput
            (
                [NotNull] AbstractOutput output
            )
        {
            Code.NotNull(output, "output");

            Output = output;
        }

        #endregion

        #region Object members

        #endregion
    }
}
