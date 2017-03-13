// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftTester.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WIN81

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Collections;

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
        /// Environment.
        /// </summary>
        [NotNull]
        //public PftEnvironmentAbstraction Environment { get; private set; }
        public AbstractClient Environment { get; private set; }

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

            //Environment = new PftLocalEnvironment();
            Environment = new LocalClient();
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

#if CLASSIC || NETCORE

            ConsoleColor foreColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;

#endif

#if !UAP

            Console.Write("{0}: ", name);

#endif

#if CLASSIC || NETCORE

            Console.ForegroundColor = foreColor;

#endif

            try
            {
                result = test.Run(name);

#if CLASSIC || NETCORE

                Console.ForegroundColor = result.Failed
                    ? ConsoleColor.Red
                    : ConsoleColor.Green;

#endif

#if !UAP

                Console.WriteLine();
                Console.WriteLine
                    (
                        result.Failed
                        ? "FAIL"
                        : "OK"
                    );

#endif

#if CLASSIC || NETCORE

                Console.ForegroundColor = foreColor;

#endif

#if !UAP

                Console.WriteLine(new string('=', 70));
                Console.WriteLine();

#endif
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);

#if !UAP

                Console.WriteLine(exception);

#endif
            }

#if !UAP

            Console.WriteLine();

#endif

            return result;
        }

        /// <summary>
        /// Run the tests.
        /// </summary>
        public void RunTests()
        {
            foreach (PftTest test in Tests)
            {
                test.Environment = Environment;
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
                //[NotNull] PftEnvironmentAbstraction environment
                [NotNull] AbstractClient environment
            )
        {
            Code.NotNull(environment, "environment");

            Environment = environment;
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
