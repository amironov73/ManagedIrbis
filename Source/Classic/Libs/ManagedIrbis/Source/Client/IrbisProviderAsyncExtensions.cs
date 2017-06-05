// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisProviderAsyncExtensions.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4 || NETCORE || DROID || UAP

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
using AM.Logging;
using AM.Runtime;
using AM.Text;
using AM.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Client
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisProviderAsyncExtensions
    {
        #region Public methods

        /// <summary>
        /// Configure.
        /// </summary>
        [NotNull]
        public static Task ConfigureAsync
            (
                [NotNull] this IrbisProvider provider,
                [NotNull] string configurationString
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNullNorEmpty(configurationString, "configurationString");

            Task result = Task.Factory.StartNew
                (
                    () => provider.Configure(configurationString)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Dispose the provider.
        /// </summary>
        [NotNull]
        public static Task DisposeAsync
            (
                [NotNull] this IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Task result = Task.Factory.StartNew
                (
                    () => provider.Dispose()
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Format record.
        /// </summary>
        [NotNull]
        public static Task<string> FormatRecordAsync
            (
                [NotNull] this IrbisProvider provider,
                [NotNull] MarcRecord record,
                [NotNull] string format
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(record, "record");
            Code.NotNull(format, "format");

            Task<string> result = Task.Factory.StartNew
                (
                    () => provider.FormatRecord(record, format)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Format records.
        /// </summary>
        [NotNull]
        public static Task<string[]> FormatRecordsAsync
            (
                [NotNull] this IrbisProvider provider,
                [NotNull] int[] mfns,
                [NotNull] string format
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(mfns, "mfns");
            Code.NotNull(format, "format");

            Task<string[]> result = Task.Factory.StartNew
                (
                    () => provider.FormatRecords(mfns, format)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Get maximal MFN.
        /// </summary>
        [NotNull]
        public static Task<int> GetMaxMfnAsync
            (
                [NotNull] this IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Task<int> result = Task.Factory.StartNew
                (
                    () => provider.GetMaxMfn()
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Get user INI-file.
        /// </summary>
        [NotNull]
        public static Task<IniFile> GetUserIniFileAsync
            (
                [NotNull] this IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Task<IniFile> result = Task.Factory.StartNew
                (
                    () => provider.GetUserIniFile()
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// List databases.
        /// </summary>
        [NotNull]
        public static Task<DatabaseInfo[]> ListDatabasesAsync
            (
                [NotNull] this IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Task<DatabaseInfo[]> result = Task.Factory.StartNew
                (
                    () => provider.ListDatabases()
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read the file.
        /// </summary>
        [NotNull]
        public static Task<string> ReadFileAsync
            (
                [NotNull] this IrbisProvider provider,
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(fileSpecification, "fileSpecification");

            Task<string> result = Task.Factory.StartNew
                (
                    () => provider.ReadFile(fileSpecification)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read INI-file.
        /// </summary>
        [NotNull]
        public static Task<IniFile> ReadIniFileAsync
            (
                [NotNull] this IrbisProvider provider,
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(fileSpecification, "fileSpecification");

            Task<IniFile> result = Task.Factory.StartNew
                (
                    () => provider.ReadIniFile(fileSpecification)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Reade menu file.
        /// </summary>
        [NotNull]
        public static Task<MenuFile> ReadMenuFileAsync
            (
                [NotNull] this IrbisProvider provider,
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(fileSpecification, "fileSpecification");

            Task<MenuFile> result = Task.Factory.StartNew
                (
                    () => provider.ReadMenuFile(fileSpecification)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read the record.
        /// </summary>
        [NotNull]
        public static Task<MarcRecord> ReadRecordAsync
            (
                [NotNull] this IrbisProvider provider,
                int mfn
            )
        {
            Code.NotNull(provider, "provider");

            Task<MarcRecord> result = Task.Factory.StartNew
                (
                    () => provider.ReadRecord(mfn)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read record version.
        /// </summary>
        [NotNull]
        public static Task<MarcRecord> ReadRecordVersionAsync
            (
                [NotNull] this IrbisProvider provider,
                int mfn,
                int version
            )
        {
            Code.NotNull(provider, "provider");
            Code.Positive(mfn, "mfn");
            Code.Nonnegative(version, "version");

            Task<MarcRecord> result = Task.Factory.StartNew
                (
                    () => provider.ReadRecordVersion(mfn, version)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read search scenarios.
        /// </summary>
        [NotNull]
        public static Task<SearchScenario[]> ReadSearchScenariosAsync
            (
                [NotNull] this IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Task<SearchScenario[]> result = Task.Factory.StartNew
                (
                    () => provider.ReadSearchScenarios()
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read terms.
        /// </summary>
        [NotNull]
        public static Task<TermInfo[]> ReadTermsAsync
            (
                [NotNull] this IrbisProvider provider,
                [NotNull] TermParameters parameters
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(parameters, "parameters");

            Task<TermInfo[]> result = Task.Factory.StartNew
                (
                    () => provider.ReadTerms(parameters)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Search for records.
        /// </summary>
        [NotNull]
        public static Task<int[]> SearchAsync
            (
                [NotNull] this IrbisProvider provider,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(provider, "provider");

            Task<int[]> result = Task.Factory.StartNew
                (
                    () => provider.Search(expression)
                )
                .ConfigureSafe();

            return result;
        }

        #endregion
    }
}

#endif
