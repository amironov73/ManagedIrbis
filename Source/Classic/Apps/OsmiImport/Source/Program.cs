// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- entry point
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Logging;
using AM.Logging.NLog;

#endregion

namespace OsmiImport
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        static void SetupLogging()
        {
            Log.SetLogger(new NLogger());
        }

        static void DoImport()
        {

        }

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args"></param>
        static int Main(string[] args)
        {
            int returnCode = 0;

            SetupLogging();

            Log.Trace("Program starts");

            try
            {
                DoImport();
            }
            catch (Exception exception)
            {
                Log.TraceException("Global exception", exception);
                returnCode = 1;
            }

            Log.Info("Return code=" + returnCode);
            Log.Trace("Program exits");

            return returnCode;
        }
    }
}
