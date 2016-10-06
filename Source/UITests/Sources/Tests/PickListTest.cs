/* PickListTest.cs -- 
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
    public sealed class PickListTest
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

                PickList pickList = new PickList
                {
                    Location = new Point(10, 10),
                    Size = new Size(300, 200)
                };
                pickList.AvailableItems.AddRange(new object[]
                {
                    "first",
                    "second",
                    "third",
                    "fourth",
                    "fifth",
                    "sixth"
                });
                form.Controls.Add(pickList);

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
