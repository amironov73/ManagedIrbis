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
            string[] subDirs = Directory.GetDirectories
                (
                    Folder,
                    "*",
                    SearchOption.TopDirectoryOnly
                );

            foreach (string subDir in subDirs)
            {
                PftTest test = new PftTest(subDir);
                Tests.Add(test);
            }
        }

        /// <summary>
        /// Run the test.
        /// </summary>
        public void RunTest
            (
                [NotNull] PftTest test
            )
        {
            Code.NotNull(test, "test");

            string name = Path.GetFileName(test.Folder);
            Console.Write("{0}: ", name);

            try
            {
                test.Run();
                Console.Write(" OK");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Run the tests.
        /// </summary>
        public void RunTests()
        {
            foreach (PftTest test in Tests)
            {
                RunTest(test);
            }
        }

        #endregion
    }
}
