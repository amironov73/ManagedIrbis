// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Windows.Forms;

using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraEditors;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace Inventory2017
{
    static class Program
    {
        static void _ThreadException
            (
                object sender,
                ThreadExceptionEventArgs eventArgs
            )
        {
            ExceptionBox.Show(eventArgs.Exception);
            Environment.FailFast
                (
                    "Shutting down",
                    eventArgs.Exception
                );
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.SetUnhandledExceptionMode
                    (
                        UnhandledExceptionMode.Automatic
                    );
                Application.ThreadException += _ThreadException;

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
                ExceptionBox.Show(exception);
            }
        }
    }
}
