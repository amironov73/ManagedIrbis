// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LabeledTextBox.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

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
    public partial class LabeledTextBox
        : UserControl
    {
        #region Events

        #endregion

        #region Properties

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Label Label
        {
            [DebuggerStepThrough]
            get
            {
                return _label;
            }
        }

        /// <summary>
        /// Gets the text box.
        /// </summary>
        /// <value>The text box.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TextBox TextBox
        {
            [DebuggerStepThrough]
            get
            {
                return _textBox;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledTextBox"/> class.
        /// </summary>
        public LabeledTextBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Control members

        #endregion
    }
}