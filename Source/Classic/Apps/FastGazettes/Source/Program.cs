// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;

using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraEditors;

using IrbisUI.Universal;

using CM = System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable StringLiteralTypo

namespace FastGazettes
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
        static void Main(string[] args)
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);
            Application.ThreadException += Application_ThreadException;

            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                DevExpress.UserSkins.BonusSkins.Register();
                SkinManager.EnableFormSkins();
                string skinName = CM.AppSettings["form-skin"];
                UserLookAndFeel.Default.SkinName = skinName;

                UniversalForm form = new MainForm();
                UniversalForm.Run(form, args);
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }
    }
}
