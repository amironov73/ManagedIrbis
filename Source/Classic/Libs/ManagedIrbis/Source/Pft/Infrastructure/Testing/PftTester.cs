/* PftTester.cs --
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
using AM.Collections;
using CodeJam;

using JetBrains.Annotations;

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
                    "*",
                    SearchOption.TopDirectoryOnly
                );

            foreach (string subDir in directories)
            {
                PftTest test = new PftTest(subDir);
                Tests.Add(test);
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("{0}: ", name);
            Console.ForegroundColor = foreColor;

            try
            {
                result = test.Run(name);
                Console.ForegroundColor = result.Failed
                    ? ConsoleColor.Red
                    : ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine
                    (
                        result.Failed
                        ? "FAIL"
                        : "OK"
                    );
                Console.ForegroundColor = foreColor;
                Console.WriteLine(new string('=', 70));
                Console.WriteLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.WriteLine();

            return result;
        }

        /// <summary>
        /// Run the tests.
        /// </summary>
        public void RunTests()
        {
            foreach (PftTest test in Tests)
            {
                PftTestResult result = RunTest(test);
                if (result != null)
                {
                    Results.Add(result);
                }
            }
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

            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                JArray array = JArray.FromObject(Results);
                string text = array.ToString(Formatting.Indented);
                writer.Write(text);
            }
        }

        #endregion
    }
}
