// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LabelEx.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Windows.Forms;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    public class LabelEx
        : Label
    {
        #region Properties

        /// <summary>
        /// Gets or sets the buddy control.
        /// </summary>
        /// <value>The buddy control.</value>
        [System.ComponentModel.DefaultValue(null)]
        public Control BuddyControl { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Transfers the focus to buddy control.
        /// </summary>
        public void TransferFocusToBuddyControl()
        {
            if (BuddyControl != null)
            {
                BuddyControl.Focus();
            }
        }

        #endregion

        #region Control members

        /// <summary>
        /// Raises the
        /// <see cref="E:System.Windows.Forms.Control.Click"/>
        /// event.
        /// </summary>
        protected override void OnClick
            (
                EventArgs e
            )
        {
            base.OnClick(e);
            TransferFocusToBuddyControl();
        }

        #endregion
    }
}