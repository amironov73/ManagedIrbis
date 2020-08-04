// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Threading;
using System.Windows.Forms;
using AM;
using AM.Windows.Forms;

#endregion

namespace OsmiRegistration
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

        public static void Configure (IWin32Window ownerWindow)
        {
            using (ConfigurationForm form = new ConfigurationForm())
            {
                if (ReferenceEquals(ownerWindow, null))
                {
                    form.ShowInTaskbar = true;
                }

                form.ShowDialog(ownerWindow);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main (string[] args)
        {
            try
            {
                Application.ThreadException += _ThreadException;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (args.Length != 0)
                {
                    if (args[0].SameString("-config"))
                    {
                        Configure(null);
                    }

                    return;
                }

                Application.Run(new MainForm());
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        }
    }
}
