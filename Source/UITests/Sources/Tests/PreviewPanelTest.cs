/* PreviewPanelTest.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    public sealed class PreviewPanelTest
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            using (Form form = new Form())
            {
                form.Size = new Size(800, 600);

                PreviewPanel panel = new PreviewPanel
                {
                    Location = new Point(10, 10),
                    Size = new Size(600, 300)
                };
                form.Controls.Add(panel);

                string text = @"<h3>Город-сад</h3>
<p>По небу тучи бегают,<br/>
Дождями сумрак сжат,<br/>
под старою телегою<br/>
рабочие лежат.</p>
<p>И слышит шепот гордый<br/>
вода и под и над:<br/>
""Через четыре года<br/>
здесь будет город-сад!""</p>";

                panel.SetText(text);

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
