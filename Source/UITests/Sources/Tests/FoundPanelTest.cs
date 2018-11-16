/* FoundPanelTest.cs --
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
using ManagedIrbis.Client;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    public sealed class FoundPanelTest
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            using (IrbisConnection connection = new IrbisConnection())
            {
                connection.ParseConnectionString
                    (
                        "host=127.0.0.1;port=6666;"
                        + "user=1;password=1;db=IBIS;"
                    );
                connection.Connect();

                RecordAdapter adapter = new RecordAdapter(connection);
                adapter.Fill();

                using (Form form = new Form())
                {
                    form.Size = new Size(800, 600);

                    FoundPanel panel = new FoundPanel(adapter)
                    {
                        Location = new Point(10, 10),
                        Size = new Size(600, 200)
                    };
                    form.Controls.Add(panel);

                    form.ShowDialog(ownerWindow);
                }
            }
        }

        #endregion
    }
}
