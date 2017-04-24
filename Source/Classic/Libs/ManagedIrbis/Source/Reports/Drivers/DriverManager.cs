// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DriverManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class DriverManager
    {
        #region Constants

        /// <summary>
        /// CSV driver.
        /// </summary>
        public const string Csv = "CSV";

        /// <summary>
        /// Dataset driver.
        /// </summary>
        public const string Dataset = "Dataset";

        /// <summary>
        /// HTML driver.
        /// </summary>
        public const string Html = "HTML";

        /// <summary>
        /// LaTex driver.
        /// </summary>
        public const string Latex = "LaTex";

        /// <summary>
        /// Markdown driver.
        /// </summary>
        public const string Markdown = "Markdown";

        /// <summary>
        /// Plain text driver.
        /// </summary>
        public const string PlainText = "PlainText";

        /// <summary>
        /// RTf driver.
        /// </summary>
        public const string Rtf = "RTF";

        #endregion

        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public static Dictionary<string, Type> Registry
        {
            get; private set;
        }

        #endregion

        #region Construction

        static DriverManager()
        {
            Registry = new Dictionary<string, Type>
            {
#if !NETCORE

                { Dataset, typeof(DatasetDriver) },

#endif

                { Csv, typeof(CsvDriver) },
                { Html, typeof(HtmlDriver) },
                { Latex, typeof(LatexDriver) },
                { Markdown, typeof(MarkdownDriver) },
                { PlainText, typeof(PlainTextDriver) },
                { Rtf, typeof(RtfDriver) },
            };
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get <see cref="ReportDriver"/> by name.
        /// </summary>
        [CanBeNull]
        public static ReportDriver GetDriver
            (
                [NotNull] string name,
                bool throwOnError
            )
        {
            Code.NotNull(name, "name");

            Type type;
            if (!Registry.TryGetValue(name, out type))
            {
                if (throwOnError)
                {
                    throw new IrbisException
                        (
                            "Driver not found: " + name
                        );
                }

                return null;
            }

            ReportDriver result
                = (ReportDriver) Activator.CreateInstance(type);

            return result;
        }

        #endregion
    }
}
