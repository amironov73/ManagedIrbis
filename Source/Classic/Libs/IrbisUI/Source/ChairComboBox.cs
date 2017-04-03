// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChairComboBox.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;

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
    public class ChairComboBox
        : ComboBox
    {
        #region Properties

        /// <summary>
        /// Selected chair.
        /// </summary>
        [CanBeNull]
        public ChairInfo SelectedChair
        {
            get { return SelectedItem as ChairInfo; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChairComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Fill the combo box with chair list.
        /// </summary>
        public void FillWithChairs
            (
                [NotNull] IrbisConnection connection,
                bool addAllItem
            )
        {
            Code.NotNull(connection, "connection");

            ChairInfo[] chairs = ChairInfo.Read
                (
                    connection,
                    ChairInfo.ChairMenu,
                    addAllItem
                );
            Items.Clear();
            Items.AddRange(chairs);
        }

        /// <summary>
        /// Fill the combo box with places list.
        /// </summary>
        public void FillWithPlaces
            (
                [NotNull] IrbisConnection connection,
                bool addAllItem
            )
        {
            Code.NotNull(connection, "connection");

            ChairInfo[] chairs = ChairInfo.Read
                (
                    connection,
                    ChairInfo.PlacesMenu,
                    addAllItem
                );
            Items.Clear();
            Items.AddRange(chairs);
        }

        #endregion

        #region Object members

        #endregion
    }
}
