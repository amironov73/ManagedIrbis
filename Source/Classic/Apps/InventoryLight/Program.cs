/* Program.cs
 */

#region Using directives

using System;
using System.Windows.Forms;

using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraEditors;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace InventoryLight
{
    static class Program
    {
        public static void ShowException
            (
                Exception exception
            )
        {
            XtraMessageBox.Show
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
                DevExpress.UserSkins.BonusSkins.Register();
                SkinManager.EnableFormSkins();
                string skinName = CM.AppSettings["form-skin"];
                UserLookAndFeel.Default.SkinName = skinName;
                Application.Run(new MainForm());
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }
    }
}
