/* DictionaryFormTest.cs -- 
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
    public sealed class DictionaryFormTest
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            using (DictionaryForm form = new DictionaryForm())
            using (IrbisConnection connection = new IrbisConnection())
            {
                connection.ParseConnectionString
                    (
                        "host=127.0.0.1;port=6666;"
                        + "user=1;password=1;db=IBIS;"
                    );
                connection.Connect();

                TermParameters parameters = new TermParameters
                {
                    Database = "IBIS",
                    StartTerm = "K=",
                    NumberOfTerms = 100
                };
                TermInfo[] terms = connection.ReadTerms(parameters);
                terms = TermInfo.TrimPrefix(terms, "K=");
                form.SetTerms(terms);

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
