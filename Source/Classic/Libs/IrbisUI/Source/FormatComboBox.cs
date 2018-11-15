// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FormatComboBox.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [System.ComponentModel.DesignerCategory("Code")]
    public class FormatComboBox
        : ComboBox
    {
        #region Properties



        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormatComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Fill the combo box with scenarios list.
        /// </summary>
        public void FillWithFormats
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database
            )
        {
            Code.NotNull(connection, "connection");

            throw new NotImplementedException();

            // ReSharper disable once CoVariantArrayConversion
            //Items.AddRange();
        }

        #endregion
    }
}
