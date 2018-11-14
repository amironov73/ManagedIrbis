/* DictionaryFormTest2.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Windows.Forms;

using IrbisUI;

using ManagedIrbis;

#endregion

// ReSharper disable StringLiteralTypo
// ReSharper disable LocalizableElement

namespace UITests
{
    public sealed class DictionaryFormTest2
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

                TermAdapter adapter = new TermAdapter(connection, "K=");
                string chosen = DictionaryForm.ChooseTerm
                    (
                        ownerWindow,
                        adapter,
                        "бетон"
                    );
                if (!string.IsNullOrEmpty(chosen))
                {
                    MessageBox.Show("Chosen: " + chosen);
                }
            }
        }

        #endregion
    }
}
