﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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
using IrbisUI.Universal;
using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace CniInvent
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
