// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportTest.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */


#if !PORTABLE

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
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReportTest
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
        public const string RecordFileName = "records.txt";

        #endregion

        #region Properties

        /// <summary>
        /// Environment.
        /// </summary>
        [CanBeNull]
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
        public ReportTest
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
        public ReportTestResult Run
            (
                [NotNull] string name
            )
        {
            ReportTestResult result = new ReportTestResult
            {
                Name = name,
                StartTime = DateTime.Now
            };

            try
            {
                string descriptionFile = GetFullName(DescriptionFileName);
                if (File.Exists(descriptionFile))
                {
                    string description = FileUtility.ReadAllText
                        (
                            descriptionFile,
                            IrbisEncoding.Utf8
                        );
                    result.Description = description;
                    Console.Write(description);
                }

                string recordFile = GetFullName(RecordFileName);

                if (ReferenceEquals(recordFile, null))
                {
                    throw new IrbisException
                        (
                            "GetFullName returns null"
                        );
                }

                MarcRecord[] records = PlainText.ReadRecords
                    (
                        recordFile,
                        IrbisEncoding.Utf8
                    );

                string reportFile = GetFullName(InputFileName);
                IrbisReport report = IrbisReport.LoadJsonFile(reportFile);

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

                AbstractClient environment = Environment
                    .ThrowIfNull("environment not set");
                ReportContext context = new ReportContext(environment);
                context.Records.AddRange(records);

                context.Verify(true);
                report.Verify(true);

                report.Render(context);
                string output = context.Output.Text.DosToUnix();
                result.Output = output;

                if (expected != null)
                {
                    if (output != expected)
                    {
                        result.Failed = true;

                        Console.WriteLine();
                        Console.WriteLine("!!! FAILED !!!");
                        Console.WriteLine();
                        Console.WriteLine(output);
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception exception)
            {
                result.Failed = true;
                result.Exception = exception.ToString();

                Console.WriteLine();
                Console.WriteLine("!!! FAILED !!!");
                Console.WriteLine();
                Console.WriteLine(exception);
                Console.WriteLine();
            }

            result.FinishTime = DateTime.Now;
            result.Duration = result.FinishTime - result.StartTime;

            return result;
        }


        #endregion

        #region Object members

        #endregion
    }
}

#endif
