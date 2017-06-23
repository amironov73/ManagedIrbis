// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using Newtonsoft.Json;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace PipeEditor
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

        static Form _CreateForm
            (
                string[] commandArgs
            )
        {
            if (commandArgs.Length != 0)
            {
                string encodingName = commandArgs[0];
                Encoding encoding;

                switch (encodingName)
                {
                    case "default":
                    case "ansi":
                    case "ANSI":
                        encoding = Encoding.Default;
                        break;

                    default:
                        if (encodingName.IsPositiveInteger())
                        {
                            int codePage
                                = NumericUtility.ParseInt32(encodingName);
                            encoding = Encoding.GetEncoding(codePage);
                        }
                        else
                        {
                            encoding = Encoding.GetEncoding(encodingName);
                        }
                        break;
                }

                Console.InputEncoding = encoding;
            }

            string text = Console.In.ReadToEnd();
            PlainTextForm result = new PlainTextForm(text)
            {
                Icon = Properties.Resources.Pipe
            };
            ToolStripButton button = new ToolStripButton
            (
                "Send to console and close (F2)",
                Properties.Resources.PipeEnd.ToBitmap()
            );
            EventHandler handler1 = (sender, args) =>
            {
                string modifiedText = result.Text;
                Console.Out.Write(modifiedText);
                result.Close();
            };
            button.Click += handler1;
            result.Editor.TextBox.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.F2)
                {
                    handler1(sender, args);
                }
            };
            result.AddButton(button);

            return result;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main
            (
                string[] args
            )
        {
            try
            {
                Application.ThreadException += _ThreadException;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Form form = _CreateForm(args);
                Application.Run(form);
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        }
    }
}
