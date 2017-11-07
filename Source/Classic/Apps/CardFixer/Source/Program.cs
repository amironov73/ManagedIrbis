/* Program.cs
 */

#region Using directives

using System;
using System.Windows.Forms;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace CardFixer
{
    static class Program
    {
        public static void ShowException
            (
                Exception exception
            )
        {
            MessageBox.Show
                (
                    exception.ToString(),
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            Environment.FailFast("Ошибка");
        }

        static void Application_ThreadException
            (
                object sender,
                System.Threading.ThreadExceptionEventArgs e
            )
        {
            ShowException(e.Exception);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.ThreadException += Application_ThreadException;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }
    }
}