// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DatabaseComboBox.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

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
    public class DatabaseComboBox
        : ComboBox
    {
        #region Properties

        /// <summary>
        /// Selected chair.
        /// </summary>
        [CanBeNull]
        public DatabaseInfo SelectedDatabase
        {
            get { return SelectedItem as DatabaseInfo; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DatabaseComboBox()
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
        public void FillWithDatabases
            (
                [NotNull] IrbisConnection connection,
                string listFile
            )
        {
            Code.NotNull(connection, "connection");

            DatabaseInfo[] databases = connection.ListDatabases(listFile);

            Items.AddRange(databases);
        }

        #endregion

        #region Object members

        #endregion
    }
}
