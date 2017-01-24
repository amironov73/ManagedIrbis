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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using AM.Windows.Forms;

using ManagedIrbis;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace EasyBinder
{
    static class Program
    {
        public static void ShowException
            (
                Exception exception
            )
        {
            ExceptionBox.Show(exception);

            Environment.FailFast("Ошибка");
        }

        static void _ThreadException
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
                Application.ThreadException += _ThreadException;

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
