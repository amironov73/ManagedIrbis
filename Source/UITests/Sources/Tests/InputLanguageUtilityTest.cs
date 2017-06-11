/* InputLanguageUtiltyTest.cs -- 
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
    public sealed class InputLanguageUtilityTest
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

                InputLanguageIndicator indicator
                    = new InputLanguageIndicator
                    {
                        Location = new Point(10, 10)
                    };
                form.Controls.Add(indicator);

                Button englishButton = new Button
                {
                    Location = new Point(40, 10),
                    Text = "English"
                };
                form.Controls.Add(englishButton);

                Button russianButton = new Button
                {
                    Location = new Point(140, 10),
                    Text = "Russian"
                };
                form.Controls.Add(russianButton);

                TextBox textBox = new TextBox
                {
                    Location = new Point(10, 100),
                    Width = 200
                };
                form.Controls.Add(textBox);

                englishButton.Click += (sender, args) =>
                {
                    InputLanguageUtility.SwitchToEnglish();
                };

                russianButton.Click += (sender, args) =>
                {
                    InputLanguageUtility.SwitchToRussian();
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
