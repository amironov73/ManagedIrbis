/* IrbisLoginFormTest.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AM;

using CodeJam;
using IrbisUI;
using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    public sealed class IrbisLoginFormTest
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window parentWindow
            )
        {
            using (IrbisLoginForm form = new IrbisLoginForm())
            {
                if (form.ShowDialog(parentWindow) == DialogResult.OK)
                {
                    ConnectionSettings settings
                        = form.ToConnectionSettings();
                    MessageBox.Show(settings.ToString());
                }
            }
        }

        #endregion
    }
}
