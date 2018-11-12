/* DictionaryPanelTest.cs --
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
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    public sealed class DictionaryPanelTest
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            using (Form form = new Form())
            using (IrbisConnection connection = new IrbisConnection())
            {
                connection.ParseConnectionString
                    (
                        "host=127.0.0.1;port=6666;"
                        + "user=1;password=1;db=IBIS;"
                    );
                connection.Connect();

                form.Size = new Size(800, 600);

                TermAdapter adapter = new TermAdapter(connection, "K=");
                adapter.Fill();
                DictionaryPanel panel = new DictionaryPanel(adapter)
                {
                    Location = new Point(10, 10),
                    Size = new Size(300, 300),
                };
                form.Controls.Add(panel);

                TextBox currentBox = new TextBox
                {
                    Location = new Point(400, 10),
                    Width = 300
                };
                form.Controls.Add(currentBox);

                TextBox choosedBox = new TextBox
                {
                    Location = new Point(400, 40),
                    Width = 300
                };
                form.Controls.Add(choosedBox);

                adapter.Source.CurrentChanged += (sender, args) =>
                {
                    currentBox.Text = adapter.FullTerm;
                };
                panel.Choosed += (sender, args) =>
                {
                    choosedBox.Text = adapter.CurrentValue;
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
