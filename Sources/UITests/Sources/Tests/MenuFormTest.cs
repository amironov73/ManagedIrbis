/* MenuFormTest.cs -- 
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
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    public sealed class MenuFormTest
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            using (MenuForm form = new MenuForm())
            //using (IrbisConnection connection = new IrbisConnection())
            {
                //connection.ParseConnectionString
                //    (
                //        "host=127.0.0.1;port=6666;"
                //        + "user=1;password=1;db=IBIS;"
                //    );
                //connection.Connect();

                FileSpecification specification
                    = new FileSpecification
                    (
                        IrbisPath.Data,
                        "dbnam2.mnu"
                    );

                //MenuFile menuFile = MenuFile.ReadFromServer
                //    (
                //        connection,
                //        specification
                //    );
                MenuFile menuFile = MenuFile.ParseLocalFile
                    (
                        @"..\..\..\..\..\TestData\dbnam1.mnu",
                        IrbisEncoding.Ansi
                    );
                form.SetEntries(menuFile.Entries);

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
