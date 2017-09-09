// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ControlCenter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.Configuration;
using AM.Text;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Readers;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace Shortage
{
    public static class ControlCenter
    {
        #region Properties

        /// <summary>
        /// Connection string for IRBIS-server.
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Debug output.
        /// </summary>
        public static AbstractOutput Output { get; set; }

        /// <summary>
        /// Формат БО.
        /// </summary>
        public static string Format { get; set; }

        /// <summary>
        /// Префикс регистра (номера КСУ).
        /// </summary>
        public static string Prefix { get; set; }

        /// <summary>
        /// Экземпляры.
        /// </summary>
        public static List<ExemplarInfo> Exemplars { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static int _ExemplarComparison
            (
                ExemplarInfo left,
                ExemplarInfo right
            )
        {
            int result = NumberText.Compare
                (
                    left.Number,
                    right.Number
                );

            if (result == 0)
            {
                result = NumberText.Compare
                    (
                        left.Description,
                        right.Description
                    );
            }

            return result;
        }

        private static int _TermComparison
            (
                TermInfo left,
                TermInfo right
            )
        {
            return NumberText.Compare
                (
                    left.Text,
                    right.Text
                );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get <see cref="IrbisConnection"/>.
        /// </summary>
        [NotNull]
        public static IrbisConnection GetIrbisConnection()
        {
            IrbisConnection result
                = new IrbisConnection(ConnectionString);

            return result;
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        public static void Initialize()
        {
            ConnectionString 
                = ConfigurationUtility.RequireString("connectionString");
            Prefix = ConfigurationUtility.RequireString("prefix");
            Format = ConfigurationUtility.RequireString("format");
        }

        public static List<ExemplarInfo> GetExemplars
            (
                [NotNull] string ksu
            )
        {
            Code.NotNullNorEmpty(ksu, "ksu");

            List<ExemplarInfo> result;

            using (IrbisConnection connection = GetIrbisConnection())
            {
                ExemplarManager manager 
                    = new ExemplarManager(connection, Output)
                    {
                        Format = Format
                    };

                string expression = string.Format
                    (
                        "\"{0}{1}\"",
                        Prefix,
                        ksu
                    );

                 result = BatchRecordReader.Search
                    (
                        connection,
                        connection.Database,
                        expression,
                        500
                    )
                    .SelectMany
                        (
                            x => ExemplarInfo.Parse (x)
                        )
                    .Where
                        (
                            exemplar => exemplar.KsuNumber1.SameString(ksu)
                        )
                    .Select
                        (
                            exemplar => manager.Extend(exemplar, null)
                        )
                    .ToList();

                result.Sort(_ExemplarComparison);
            }

            return result;
        }

        public static TermInfo[] GetTerms
            (
                [NotNull] string year
            )
        {
            Code.NotNull(year, "year");

            int prefixLength = Prefix.Length;
            string start = Prefix + year;
            TermInfo[] result;
            using (IrbisConnection connection = GetIrbisConnection())
            {
                TermParameters parameters = new TermParameters
                {
                    Database = connection.Database,
                    StartTerm = start,
                    NumberOfTerms = 1000
                };
                result = connection.ReadTerms(parameters);
            }

            result = result
                    .Where
                    (
                        term => term.Text
                            .ThrowIfNull("term.Text")
                            .StartsWith(start)
                    )
                    .Select
                    (
                        term => new TermInfo
                        {
                            Count = term.Count,
                            Text = term.Text
                                .ThrowIfNull("term.Text")
                                .Substring
                                (
                                    prefixLength,
                                    term.Text
                                        .ThrowIfNull("term.Text")
                                        .Length - prefixLength
                                )
                        }
                    )
                    .ToArray();

            Array.Sort(result, _TermComparison);

            return result;
        }

        public static List<ExemplarInfo> ListUnmarked()
        {
            if (ReferenceEquals(Exemplars, null))
            {
                return new List<ExemplarInfo>();
            }

            List<ExemplarInfo> result = Exemplars.Where
                (
                    exemplar => !exemplar.Marked
                )
                .ToList();

            return result;
        }

        public static List<ExemplarInfo> ReadExemplars
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = File.ReadAllText(fileName);
            List<ExemplarInfo> result = JsonConvert
                .DeserializeObject<List<ExemplarInfo>>(text);

            return result;
        }

        public static void SaveExemplars
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            if (ReferenceEquals(Exemplars, null))
            {
                return;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            string text = JsonConvert.SerializeObject
                (
                    Exemplars,
                    Formatting.Indented,
                    settings
                );
            File.WriteAllText
                (
                    fileName,
                    text
                );
        }

        /// <summary>
        /// Write debug line.
        /// </summary>
        public static void WriteLine
            (
                [NotNull] string format,
                params object[] arguments
            )
        {
            Code.NotNull(format, "format");

            if (!ReferenceEquals(Output, null))
            {
                Output.WriteLine(format, arguments);
            }
        }

        #endregion
    }
}
