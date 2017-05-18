// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftTest.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WIN81 && !PORTABLE

#region Using directives

using System;
using System.IO;

using AM;
using AM.ConsoleIO;
using AM.IO;
using AM.Logging;
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
        /// Provider.
        /// </summary>
        [CanBeNull]
        public IrbisProvider Provider { get; set; }

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
                if (ReferenceEquals(Provider, null))
                {
                    throw new PftException("environment not set");
                }

                string descriptionFile = GetFullName(DescriptionFileName);
                if (File.Exists(descriptionFile))
                {
                    string description = FileUtility.ReadAllText
                        (
                            descriptionFile,
                            IrbisEncoding.Utf8
                        );
                    result.Description = description;
                }

                string recordFile = GetFullName(RecordFileName);

                if (ReferenceEquals(recordFile, null))
                {
                    throw new IrbisException
                        (
                            "GetFullName returns null"
                        );
                }

                MarcRecord record = PlainText.ReadOneRecord
                    (
                        recordFile,
                        IrbisEncoding.Utf8
                    )
                    .ThrowIfNull("ReadOneRecord returns null");

                string pftFile = GetFullName(InputFileName);
                string input = FileUtility.ReadAllText
                    (
                        pftFile,
                        IrbisEncoding.Utf8
                    )
                    .DosToUnix()
                    .ThrowIfNull("input");
                result.Input = input;

                ConsoleInput.WriteLine(input);
                ConsoleInput.WriteLine();

                PftLexer lexer = new PftLexer();
                PftTokenList tokenList = lexer.Tokenize(input);
                StringWriter writer = new StringWriter();
                tokenList.Dump(writer);
                result.Tokens = writer.ToString()
                    .DosToUnix()
                    .ThrowIfNull("tokens");

                ConsoleInput.WriteLine(result.Tokens);
                ConsoleInput.WriteLine();

                PftParser parser = new PftParser(tokenList);
                PftProgram program = parser.Parse();
                writer = new StringWriter();
                result.Ast = writer.ToString()
                    .DosToUnix()
                    .ThrowIfNull("ast");

                ConsoleInput.WriteLine(result.Ast);
                ConsoleInput.WriteLine();

                string expectedFile = GetFullName(ExpectedFileName);
                string expected = null;
                if (File.Exists(expectedFile))
                {
                    expected = FileUtility.ReadAllText
                        (
                            expectedFile,
                            IrbisEncoding.Utf8
                        )
                        .DosToUnix()
                        .ThrowIfNull("expected");
                    result.Expected = expected;
                }

                IrbisProvider provider = Provider
                    .ThrowIfNull("Provider");

                string output;
                using (PftFormatter formatter = new PftFormatter
                {
                    Program = program
                })
                {
                    formatter.SetEnvironment(provider);
                    output = formatter.Format(record)
                        .DosToUnix()
                        .ThrowIfNull("output");
                }
                result.Output = output;

                ConsoleInput.WriteLine(output);

                if (expected != null)
                {
                    if (output != expected)
                    {
                        result.Failed = true;

                        ConsoleInput.WriteLine();
                        ConsoleInput.WriteLine("!!! FAILED !!!");
                        ConsoleInput.WriteLine();
                        ConsoleInput.WriteLine(expected);
                        ConsoleInput.WriteLine();
                    }
                }
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftTest::Run",
                        exception
                    );

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

#endif
