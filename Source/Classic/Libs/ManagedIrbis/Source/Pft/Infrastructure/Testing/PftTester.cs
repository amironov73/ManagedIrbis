// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftTester.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WIN81 && !PORTABLE && !SILVERLIGHT

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Collections;
using AM.ConsoleIO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Testing
{
    /// <summary>
    /// Test for PFT formatting.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftTester
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
        public NonNullCollection<PftTest> Tests { get; private set; }

        /// <summary>
        /// Results.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftTestResult> Results { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTester
            (
                [NotNull] string folder
            )
        {
            Code.NotNullNorEmpty(folder, "folder");

            Provider = new LocalProvider();
            Folder = folder;
            Tests = new NonNullCollection<PftTest>();
            Results = new NonNullCollection<PftTestResult>();
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
                if (PftTest.IsDirectoryContainsTest(subDir))
                {
                    PftTest test = new PftTest(subDir);
                    Tests.Add(test);
                }
            }
        }

        /// <summary>
        /// Run the test.
        /// </summary>
        public PftTestResult RunTest
            (
                [NotNull] PftTest test
            )
        {
            Code.NotNull(test, "test");

            PftTestResult result = null;

            string name = Path.GetFileName(test.Folder);

            ConsoleColor foreColor = Console.ForegroundColor;
            ConsoleInput.ForegroundColor = ConsoleColor.Cyan;

            ConsoleInput.Write(string.Format("{0}: ", name));

            ConsoleInput.ForegroundColor = foreColor;

            try
            {
                result = test.Run(name);

                ConsoleInput.ForegroundColor = result.Failed
                    ? ConsoleColor.Red
                    : ConsoleColor.Green;

                ConsoleInput.WriteLine();
                ConsoleInput.WriteLine
                    (
                        result.Failed
                        ? "FAIL"
                        : "OK"
                    );

                ConsoleInput.ForegroundColor = foreColor;

                ConsoleInput.WriteLine(new string('=', 70));
                ConsoleInput.WriteLine();
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftTester::RunTest",
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
            foreach (PftTest test in Tests)
            {
                test.Provider = Provider;
                PftTestResult result = RunTest(test);
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
    }
}

#endif
