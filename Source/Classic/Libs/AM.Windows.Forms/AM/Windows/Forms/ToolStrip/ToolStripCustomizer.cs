// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ToolStripCustomizer.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public static class ToolStripCustomizer
    {
        /// <summary>
        /// Customizes the specified tool strip.
        /// </summary>
        public static bool Customize
            (
                [NotNull] ToolStrip toolStrip
            )
        {
            Code.NotNull(toolStrip, "toolStrip");

            using (ToolStripCustomizationForm form
                = new ToolStripCustomizationForm(toolStrip))
            {
                return form.ShowDialog(toolStrip.FindForm())
                    == DialogResult.OK;
            }
        }
    }
}
