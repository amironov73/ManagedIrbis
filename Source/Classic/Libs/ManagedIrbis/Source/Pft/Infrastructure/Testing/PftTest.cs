/* PftTest.cs --
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
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Testing
{
    /// <summary>
    /// Single test for PFT formatting.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftTest
    {
        #region Constants

        /// <summary>
        /// Description file name.
        /// </summary>
        public const string DescriptionFileName = "description.txt";

        /// <summary>
        /// Expected result file name.
        /// </summary>
        public const string ExpectedFileName = "expected.txt";

        /// <summary>
        /// Input file name.
        /// </summary>
        public const string InputFileName = "input.txt";

        /// <summary>
        /// Record file name.
        /// </summary>
        public const string RecordFileName = "record.txt";

        #endregion

        #region Properties

        /// <summary>
        /// Environment.
        /// </summary>
        [CanBeNull]
        //public PftEnvironmentAbstraction Environment { get; set; }
        public AbstractClient Environment { get; set; }

        /// <summary>
        /// Folder name.
        /// </summary>
        [NotNull]
        public string Folder { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTest
            (
                [NotNull] string folder
            )
        {
            Code.NotNullNorEmpty(folder, "folder");

            Folder = Path.GetFullPath(folder);
        }

        #endregion

        #region Private members

        private string GetFullName
            (
                string shortName
            )
        {
            return Path.Combine
                (
                    Folder,
                    shortName
                );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Whether the directory contains test?
        /// </summary>
        public static bool IsDirectoryContainsTest
            (
                [NotNull] string directory
            )
        {
            Code.NotNullNorEmpty(directory, "directory");

            bool result =
                File.Exists(Path.Combine(directory, DescriptionFileName))
                && File.Exists(Path.Combine(directory, RecordFileName))
                && File.Exists(Path.Combine(directory, InputFileName));

            return result;
        }

        /// <summary>
        /// Run the test.
        /// </summary>
        public PftTestResult Run
            (
                [NotNull] string name
            )
        {
            PftTestResult result = new PftTestResult
            {
                Name = name,
                StartTime = DateTime.Now
            };

            try
            {
                if (ReferenceEquals(Environment, null))
                {
                    throw new PftException("environment not set");
                }

                string descriptionFile = GetFullName(DescriptionFileName);
                if (File.Exists(descriptionFile))
                {
                    string description = File.ReadAllText(descriptionFile);
                    result.Description = description;
                }

                string recordFile = GetFullName(RecordFileName);
                MarcRecord record = PlainText.ReadOneRecord
                    (
                        recordFile,
                        IrbisEncoding.Utf8
                    )
                    .ThrowIfNull("record");
                //result.Record = record;

                string pftFile = GetFullName(InputFileName);
                string input = File.ReadAllText
                    (
                        pftFile,
                        IrbisEncoding.Utf8
                    )
                    .DosToUnix()
                    .ThrowIfNull("input");
                result.Input = input;

                Console.WriteLine(input);
                Console.WriteLine();

                PftLexer lexer = new PftLexer();
                PftTokenList tokenList = lexer.Tokenize(input);
                StringWriter writer = new StringWriter();
                tokenList.Dump(writer);
                result.Tokens = writer.ToString()
                    .DosToUnix()
                    .ThrowIfNull("tokens");
                Console.WriteLine(result.Tokens);
                Console.WriteLine();

                PftParser parser = new PftParser(tokenList);
                PftProgram program = parser.Parse();
                writer = new StringWriter();
                result.Ast = writer.ToString()
                    .DosToUnix()
                    .ThrowIfNull("ast");
                Console.WriteLine(result.Ast);
                Console.WriteLine();

                string expectedFile = GetFullName(ExpectedFileName);
                string expected = null;
                if (File.Exists(expectedFile))
                {
                    expected= File.ReadAllText
                        (
                            expectedFile,
                            IrbisEncoding.Utf8
                        )
                        .DosToUnix()
                        .ThrowIfNull("expected");
                    result.Expected = expected;
                }

                PftFormatter formatter = new PftFormatter
                {
                    Program = program
                };
                formatter.SetEnvironment(Environment);
                string output = formatter.Format(record)
                    .DosToUnix()
                    .ThrowIfNull("output");
                result.Output = output;
                Console.WriteLine(output);

                if (expected != null)
                {
                    if (output != expected)
                    {
                        result.Failed = true;

                        Console.WriteLine();
                        Console.WriteLine("!!! FAILED !!!");
                        Console.WriteLine();
                        Console.WriteLine(expected);
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception exception)
            {
                result.Failed = true;
                result.Exception = exception.ToString();
            }

            result.FinishTime = DateTime.Now;
            result.Duration = result.FinishTime - result.StartTime;

            return result;
        }

        #endregion
    }
}
