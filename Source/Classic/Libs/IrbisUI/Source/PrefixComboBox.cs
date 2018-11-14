// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PrefixComboBox.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Windows.Forms;

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
    public class PrefixComboBox
        : ComboBox
    {
        #region Properties



        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PrefixComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Fill the combo box with scenarios list.
        /// </summary>
        public void FillWithScenarios
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database
            )
        {
            Code.NotNull(connection, "connection");

            SearchScenario[] scenarios = SearchScenario.LoadSearchScenarios
                (
                    connection,
                    database
                );
            if (ReferenceEquals(scenarios, null))
            {
                // TODO do something
                throw new IrbisException();
            }

            // ReSharper disable once CoVariantArrayConversion
            Items.AddRange(scenarios);
        }

        #endregion
    }
}
