// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportTester.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.ConsoleIO;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReportTester
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// Folder name.
        /// </summary>
        [NotNull]
        public string Folder { get; private set; }

        /// <summary>
        /// Tests.
        /// </summary>
        [NotNull]
        public NonNullCollection<ReportTest> Tests { get; private set; }

        /// <summary>
        /// Results.
        /// </summary>
        [NotNull]
        public NonNullCollection<ReportTestResult> Results { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportTester
            (
                [NotNull] string folder
            )
        {
            Code.NotNullNorEmpty(folder, "folder");

            //Environment = new PftLocalEnvironment();
            Provider = new LocalProvider();
            Folder = folder;
            Tests = new NonNullCollection<ReportTest>();
            Results = new NonNullCollection<ReportTestResult>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Discover tests.
        /// </summary>
        public void DiscoverTests()
        {
            string[] directories = Directory.GetDirectories
                (
                    Folder,
                    "*"

#if !PocketPC && !WINMOBILE

, SearchOption.AllDirectories

#endif
);

            foreach (string subDir in directories)
            {
                if (ReportTest.IsDirectoryContainsTest(subDir))
                {
                    ReportTest test = new ReportTest(subDir);
                    Tests.Add(test);
                }
            }
        }

        /// <summary>
        /// Run the test.
        /// </summary>
        public ReportTestResult RunTest
            (
                [NotNull] ReportTest test
            )
        {
            Code.NotNull(test, "test");

            ReportTestResult result = null;

            string name = Path.GetFileName(test.Folder);

#if CLASSIC || NETCORE

            ConsoleColor foreColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;

#endif

            ConsoleInput.Write(string.Format("{0}: ", name));

#if CLASSIC || NETCORE

            Console.ForegroundColor = foreColor;

#endif

            try
            {
                test.Provider = Provider;
                result = test.Run(name);

#if CLASSIC || NETCORE

                Console.ForegroundColor = result.Failed
                    ? ConsoleColor.Red
                    : ConsoleColor.Green;

#endif

                ConsoleInput.WriteLine();
                ConsoleInput.WriteLine
                    (
                        result.Failed
                        ? "FAIL"
                        : "OK"
                    );

#if CLASSIC || NETCORE

                Console.ForegroundColor = foreColor;

#endif

                ConsoleInput.WriteLine(new string('=', 70));
                ConsoleInput.WriteLine();
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "ReportTester::RunTest",
                        exception
                    );

                Debug.WriteLine(exception);

                ConsoleInput.WriteLine(exception.ToString());
            }

            ConsoleInput.WriteLine();

            return result;
        }

        /// <summary>
        /// Run the tests.
        /// </summary>
        public void RunTests()
        {
            foreach (ReportTest test in Tests)
            {
                ReportTestResult result = RunTest(test);
                if (result != null)
                {
                    Results.Add(result);
                }
            }
        }

        /// <summary>
        /// Set environment.
        /// </summary>
        public void SetEnvironment
            (
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Provider = provider;
        }

        /// <summary>
        /// Write test results to the file.
        /// </summary>
        public void WriteResults
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

#if !PocketPC && !WINMOBILE

            using (StreamWriter writer = new StreamWriter
                (
                    new FileStream
                        (
                            fileName,
                            FileMode.Create,
                            FileAccess.Write
                        )
                ))
            {
                JArray array = JArray.FromObject(Results);
                string text = array.ToString(Formatting.Indented);
                writer.Write(text);
            }

#endif
        }

        #endregion

        #region Object members

        #endregion
    }
}
