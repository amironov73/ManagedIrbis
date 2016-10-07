/* IrbisLoginForm2Test.cs -- 
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
    public sealed class IrbisLoginForm2Test
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            using (IrbisLoginForm2 form = new IrbisLoginForm2())
            {
                ConnectionSettings settings = new ConnectionSettings();
                form.ApplySettings(settings);

                if (form.ShowDialog(ownerWindow) == DialogResult.OK)
                {
                    settings = form.GatherSettings();
                    MessageBox.Show(settings.ToString());
                }
            }
        }

        #endregion
    }
}
