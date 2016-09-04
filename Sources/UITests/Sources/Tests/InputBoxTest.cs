/* InputBoxTest.cs -- 
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
    public sealed class InputBoxTest
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            string theValue = "Default value";
            DialogResult dialogResult = InputBox.Query
                (
                    "Testing the components",
                    "Enter something",
                    "Please, enter something",
                    ref theValue
                );

            string text = string.Format
                (
                    "Result: {0}{1}Value: {2}",
                    dialogResult,
                    Environment.NewLine,
                    theValue
                );
            MessageBox.Show(text);
        }

        #endregion
    }
}
