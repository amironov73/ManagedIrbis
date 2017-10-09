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

#endregion

namespace PingMonitor
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

                Application.Run(new MainForm());
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        }
    }
}
