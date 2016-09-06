/* SuggestingTextBoxTest.cs -- 
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
    public sealed class SuggestingTextBoxTest
        : IUITest
    {
        #region IUITest members

        public DaDataSuggestionClient Client;

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            Client = new DaDataSuggestionClient
                (
                    "7b22d72f65cd0b4dc5405ae13077b05c06411887"
                );

            using (Form form = new Form())
            {
                form.Size = new Size(800, 600);

                SuggestingTextBox suggestingBox = new SuggestingTextBox
                {
                    Location = new Point(10, 10),
                    Width = 200
                };
                form.Controls.Add(suggestingBox);

                TextBox textBox = new TextBox
                {
                    Location = new Point(310, 10),
                    Width = 300
                };
                form.Controls.Add(textBox);

                suggestingBox.SuggestionNeeded += _SuggestionNeeded;

                form.ShowDialog(ownerWindow);
            }
        }

        private void _SuggestionNeeded
            (
                object sender,
                EventArgs e
            )
        {
            SuggestingTextBox textBox = (SuggestingTextBox) sender;
            string text = textBox.Text;

            SuggestAddressResponse response = Client.QueryAddress(text);

            List<string> items = new List<string>();
            foreach (SuggestAddressResponse.Suggestions suggestions in response.suggestionss)
            {
                items.Add(suggestions.ToString());
            }
            textBox.SetItems(items);
            textBox.OpenIfNeeded();
        }

        #endregion
    }
}
