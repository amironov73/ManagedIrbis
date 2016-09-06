/* DaDataClientTest.cs -- 
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
using AM.Suggestions;
using AM.Suggestions.DaData;
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
    public sealed class DaDataClientTest
        : IUITest
    {
        #region IUITest members

        public TextBox TextBox;
        public ListBox ListBox;

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            using (Form form = new Form())
            {
                form.Size = new Size(800, 600);

                TextBox = new TextBox
                {
                    Text = "Иркутск",
                    Location = new Point(10, 10),
                    Width = 200
                };
                form.Controls.Add(TextBox);

                Button button = new Button
                {
                    Text = "Suggest",
                    Location = new Point(215, 10),
                    Width = 100
                };
                form.Controls.Add(button);

                ListBox = new ListBox
                {
                    Location = new Point(10, 50),
                    Size = new Size(350, 200)
                };
                form.Controls.Add(ListBox);

                button.Click += Button_Click;

                form.ShowDialog(ownerWindow);
            }
        }

        private void Button_Click
            (
                object sender,
                EventArgs e
            )
        {
            string text = TextBox.Text.Trim();
            ListBox.Items.Clear();

            DaDataSuggestionClient client = new DaDataSuggestionClient
                (
                    "7b22d72f65cd0b4dc5405ae13077b05c06411887"
                );
            SuggestAddressResponse response = client.QueryAddress(text);
            foreach (SuggestAddressResponse.Suggestions suggestions in response.suggestionss)
            {
                //string item = suggestions.ToString();
                ListBox.Items.Add(suggestions);
            }
        }

        #endregion
    }
}
