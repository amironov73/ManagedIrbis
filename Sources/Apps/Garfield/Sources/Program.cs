/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AM;
using AM.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace Garfield
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

        private static void Initialize
            (
                string[] arguments
            )
        {
            string executableFolder = Path.GetDirectoryName
                (
                    Application.ExecutablePath
                )
                .ThrowIfNull();
            string fileName = Path.Combine
                (
                    executableFolder,
                    "cirbisc.ini"
                );
            if (arguments.Length != 0)
            {
                fileName = arguments[0];
            }

            Center.IniFile = LocalCatalogerIniFile.Load(fileName);

            Center.MainForm = new MainForm();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
            try
            {
                Application.ThreadException += _ThreadException;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Initialize(arguments);

                Application.Run(Center.MainForm);
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        }
    }
}
