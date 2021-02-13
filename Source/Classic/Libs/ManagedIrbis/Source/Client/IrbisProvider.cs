// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisProvider.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

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
using AM.IOC;
using AM.Logging;
using AM.PlatformAbstraction;
using AM.Runtime;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Authentication;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;
using ManagedIrbis.Search;

#if !UAP
using ManagedIrbis.Properties;
#endif

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class IrbisProvider
        : MarshalByRefObject,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// Get busy state for the provider.
        /// </summary>
        [CanBeNull]
        public virtual BusyState BusyState
        {
            get { return null; }
        }

        /// <summary>
        /// Connected?
        /// </summary>
        public virtual bool Connected { get { return true; } }

        /// <summary>
        /// Current database.
        /// </summary>
        [NotNull]
        public virtual string Database { get; set; }

        /// <summary>
        /// Platform abstraction.
        /// </summary>
        [NotNull]
        public PlatformAbstractionLayer PlatformAbstraction { get; set; }

        /// <summary>
        /// Additional services.
        /// </summary>
        [NotNull]
        public ServiceRepository Services { get; private set; }

        /// <summary>
        /// Resolves the credentials.
        /// </summary>
        [CanBeNull]
        public ICredentialsResolver CredentialsResolver { get;set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected IrbisProvider()
        {
            Log.Trace("IrbisProvider::Constructor");

            Services = new ServiceRepository();
            PlatformAbstraction = new PlatformAbstractionLayer();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Acquire formatter instance.
        /// </summary>
        [CanBeNull]
        public virtual IPftFormatter AcquireFormatter()
        {
#if !UAP
            Log.Warn("IrbisProvider::AcquireFormatter: " + Resources.IrbisProvider_NotOverridden);
#endif

            return null;
        }

        /// <summary>
        /// Configure the provider.
        /// </summary>
        public virtual void Configure
            (
                [NotNull] string configurationString
            )
        {
            // Nothing to do here

#if !UAP
            Log.Warn("IrbisProvider::Configure: " + Resources.IrbisProvider_NotOverridden);
#endif
        }

        /// <summary>
        /// Exact search.
        /// </summary>
        [NotNull]
        public virtual TermLink[] ExactSearchLinks
            (
                [NotNull] string term
            )
        {
#if !UAP
            Log.Warn("IrbisProvider::ExactSearchLinks: " + Resources.IrbisProvider_NotOverridden);
#endif

            return EmptyArray<TermLink>.Value;
        }

        /// <summary>
        /// Exact search with trim.
        /// </summary>
        [NotNull]
        public virtual TermLink[] ExactSearchTrimLinks
            (
                [NotNull] string term,
                int limit
            )
        {
#if !UAP
            Log.Warn("IrbisProvider::ExactSearchTrimLinks: " + Resources.IrbisProvider_NotOverridden);
#endif

            return EmptyArray<TermLink>.Value;
        }

        /// <summary>
        /// File exist?
        /// </summary>
        public virtual bool FileExist
            (
                [NotNull] FileSpecification specification
            )
        {
#if !UAP
            Log.Warn("IrbisProvider::FileExist: " + Resources.IrbisProvider_NotOverridden);
#endif

            return false;
        }

        /// <summary>
        /// Format given record.
        /// </summary>
        [CanBeNull]
        public virtual string FormatRecord
            (
                [NotNull] MarcRecord record,
                string format
            )
        {
#if !UAP
            Log.Warn("IrbisProvider::FormatRecord: " + Resources.IrbisProvider_NotOverridden);
#endif

            return string.Empty;
        }

        /// <summary>
        /// Format records.
        /// </summary>
        [NotNull]
        public virtual string[] FormatRecords
            (
                int[] mfns,
                string format
            )
        {
#if !UAP
            Log.Warn("IrbisProvider::FormatRecords: " + Resources.IrbisProvider_NotOverridden);
#endif

            return StringUtility.EmptyArray;
        }

        /// <summary>
        /// Get alphabet table.
        /// </summary>
        [NotNull]
        public virtual IrbisAlphabetTable GetAlphabetTable()
        {
#if !UAP
            Log.Warn("IrbisProvider::GetAlphabetTable: " + Resources.IrbisProvider_NotOverridden);
#endif

            return new IrbisAlphabetTable();
        }

        /// <summary>
        /// Get catalog state for specified database.
        /// </summary>
        [NotNull]
        public virtual CatalogState GetCatalogState
            (
                [NotNull] string database
            )
        {
            Code.NotNullNorEmpty(database, "database");

#if !UAP
            Log.Warn("IrbisProvider::GetCatalogState: " + Resources.IrbisProvider_NotOverridden);
#endif

            CatalogState result = new CatalogState
            {
                Database = database,
                Date = PlatformAbstraction.Today(),
                Records = new RecordState[0]
            };

            return result;
        }

        /// <summary>
        /// Get generation of the provider.
        /// </summary>
        [NotNull]
        public virtual string GetGeneration()
        {
            return "64";
        }

        /// <summary>
        /// Get maximal MFN.
        /// </summary>
        public virtual int GetMaxMfn()
        {
#if !UAP
            Log.Warn("IrbisProvider::GetMaxMfn: " + Resources.IrbisProvider_NotOverridden);
#endif

            return 0;
        }

        /// <summary>
        /// Get path for file searching.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public virtual string[] GetFileSearchPath()
        {
#if !UAP
            Log.Warn("IrbisProvider::GetFileSearchPath: " + Resources.IrbisProvider_NotOverridden);
#endif

            return StringUtility.EmptyArray;
        }

        /// <summary>
        /// Get stop words.
        /// </summary>
        [NotNull]
        public virtual IrbisStopWords GetStopWords()
        {
#if !UAP
            Log.Warn("IrbisProvider::GetStopWords: " + Resources.IrbisProvider_NotOverridden);
#endif

            return new IrbisStopWords();
        }

        /// <summary>
        /// Get upper case table.
        /// </summary>
        [NotNull]
        public virtual IrbisUpperCaseTable GetUpperCaseTable()
        {
#if !UAP
            Log.Warn("IrbisProvider::GetUpperCaseTable: " + Resources.IrbisProvider_NotOverridden);
#endif

            return new IrbisUpperCaseTable();
        }

        /// <summary>
        /// Get user server INI-file.
        /// </summary>
        [NotNull]
        public virtual IniFile GetUserIniFile()
        {
#if !UAP
            Log.Warn("IrbisProvider::GetUserIniFile: " + Resources.IrbisProvider_NotOverridden);
#endif

            IniFile result = new IniFile();

            return result;
        }

        /// <summary>
        /// List databases.
        /// </summary>
        [NotNull]
        public virtual DatabaseInfo[] ListDatabases()
        {
#if !UAP
            Log.Warn("IrbisProvider::ListDatabases: " + Resources.IrbisProvider_NotOverridden);
#endif

            return EmptyArray<DatabaseInfo>.Value;
        }

        /// <summary>
        /// No operation.
        /// </summary>
        public virtual void NoOp()
        {
            // Nothing to do.
        }

        /// <summary>
        /// Read file.
        /// </summary>
        [CanBeNull]
        public virtual string ReadFile
            (
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(fileSpecification, "fileSpecification");

#if !UAP
            Log.Warn("IrbisProvider::ReadFile: " + Resources.IrbisProvider_NotOverridden);
#endif

            return null;
        }

        /// <summary>
        /// Read INI-file.
        /// </summary>
        [CanBeNull]
        public virtual IniFile ReadIniFile
            (
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(fileSpecification, "fileSpecification");

            IniFile result = null;

            string text = ReadFile(fileSpecification);
            if (!string.IsNullOrEmpty(text))
            {
                result = new IniFile
                {
                    FileName = fileSpecification.FileName
                };
                StringReader reader = new StringReader(text);
                result.Read(reader);
            }

            return result;
        }

        /// <summary>
        /// Read MNU file.
        /// </summary>
        [CanBeNull]
        public virtual MenuFile ReadMenuFile
            (
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(fileSpecification, "fileSpecification");

            MenuFile result = null;

            string text = ReadFile(fileSpecification);
            if (!string.IsNullOrEmpty(text))
            {
                StringReader reader = new StringReader(text);
                result = MenuFile.ParseStream(reader);
            }

            return result;
        }

        /// <summary>
        /// Read record.
        /// </summary>
        [CanBeNull]
        public virtual MarcRecord ReadRecord
            (
                int mfn
            )
        {
#if !UAP
            Log.Warn("IrbisProvider::ReadRecord: " + Resources.IrbisProvider_NotOverridden);
#endif

            return null;
        }

        /// <summary>
        /// Read record version.
        /// </summary>
        [CanBeNull]
        public virtual MarcRecord ReadRecordVersion
            (
                int mfn,
                int version
            )
        {
#if !UAP
            Log.Warn("IrbisProvider::ReadRecordVersion: " + Resources.IrbisProvider_NotOverridden);
#endif

            return null;
        }

        /// <summary>
        /// Read search scenarios for the database.
        /// </summary>
        [CanBeNull]
        public virtual SearchScenario[] ReadSearchScenarios()
        {
#if !UAP
            Log.Warn("IrbisProvider::ReadSearchScenarios: " + Resources.IrbisProvider_NotOverridden);
#endif

            return EmptyArray<SearchScenario>.Value;
        }

        /// <summary>
        /// Read terms.
        /// </summary>
        [NotNull]
        public virtual TermInfo[] ReadTerms
            (
                [NotNull] TermParameters parameters
            )
        {
#if !UAP
            Log.Warn("IrbisProvider::ReadTerms: " + Resources.IrbisProvider_NotOverridden);
#endif

            return EmptyArray<TermInfo>.Value;
        }

        /// <summary>
        /// Reconnect.
        /// </summary>
        public virtual void Reconnect()
        {
#if !UAP
            Log.Warn("IrbisProvider::Reconnect: " + Resources.IrbisProvider_NotOverridden);
#endif

            // Nothing to do here
        }

        /// <summary>
        /// Release the formatter.
        /// </summary>
        public virtual void ReleaseFormatter
            (
                [CanBeNull] IPftFormatter formatter
            )
        {
#if !UAP
            Log.Warn("IrbisProvider::ReleaseFormatter: " + Resources.IrbisProvider_NotOverridden);
#endif

            // Nothing to do here
        }

        /// <summary>
        /// Search records.
        /// </summary>
        [NotNull]
        public virtual int[] Search
            (
                [CanBeNull] string expression
            )
        {
#if !UAP
            Log.Warn("IrbisProvider::Search: " + Resources.IrbisProvider_NotOverridden);
#endif

            return EmptyArray<int>.Value;
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public virtual void WriteRecord
            (
                [NotNull] MarcRecord record
            )
        {
#if !UAP
            Log.Warn("IrbisProvider::WriteRecord: " + Resources.IrbisProvider_NotOverridden);
#endif

            // Nothing to do here
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public virtual void Dispose()
        {
            Log.Trace("IrbisProvider::Dispose");

            PlatformAbstraction.Dispose();
        }

        #endregion
    }
}
