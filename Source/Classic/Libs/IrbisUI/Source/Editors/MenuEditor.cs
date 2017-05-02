// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MenuEditor.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Windows.Forms;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Worksheet;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Editors
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class MenuEditor
        : IMarcEditor
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IMarcEditor members

        /// <inheritdoc/>
        public void PerformEdit
            (
                EditContext context
            )
        {
            Code.NotNull(context, "context");

            using (MenuForm form = new MenuForm())
            {
                string inputInfo = context.Item.InputInfo;
                MenuSpecification menuSpecification
                    = MenuSpecification.Parse(inputInfo);

                FileSpecification fileSpecification 
                    = menuSpecification.ToFileSpecification();

                MenuFile menu = context.Provider.ReadMenuFile
                    (
                        fileSpecification
                    )
                    .ThrowIfNull("menu");

                form.SetEntries(menu.Entries.ToArray());

                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    context.Accept = true;
                }
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
