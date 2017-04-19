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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace ExelReporTester
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("ExcelReportTester <folder>");
                return;
            }

            string testPath = args[0];

            string connectionString 
                = CM.AppSettings["connectionString"];
            using (IrbisConnection connection
                = new IrbisConnection(connectionString))
            {
                IrbisProvider provider
                    = new ConnectedClient(connection);

                IrbisReport report = IrbisReport.LoadJsonFile
                (
                    Path.Combine
                    (
                        testPath,
                        "input.txt"
                    )
                );

                MarcRecord[] records = connection.ReadRecords
                    (
                        connection.Database,
                        Enumerable.Range(1, 10)
                    );

                ExcelDriver driver = new ExcelDriver
                {
                    FileName = "report.xlsx"
                };

                ReportContext context = new ReportContext(provider);
                context.Records.AddRange(records);
                context.SetDriver(driver);
                report.Render(context);
            }
        }
    }
}
