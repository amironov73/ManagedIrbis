// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* IrbisModule.cs -- NancyFX module for IRBIS REST server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4

#region Using directives

using System;
using System.Linq;

using AM;
using AM.IO;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Nancy;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace RestfulIrbis
{
    /// <summary>
    /// NancyFX module for IRBIS REST server.
    /// </summary>
    [PublicAPI]
    [CLSCompliant(false)]
    [MoonSharpUserData]
    public class IrbisModule
        : NancyModule
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisModule()
        {
            Get["/"] = parameters => Response.AsText
                (
                    "Hello! This is RestfulIrbis!"
                );
            Get["/format/{database}/{mfns}/{format*}"] = FormatRecordsGet;
            Post["/format/{database}/{format*}"] = FormatRecordsPost;
            Get["/list"] = ListDatabases;
            Get["/max/{database}"] = GetMaxMfn;
            Get["/read/{database}/{mfn}"] = ReadRecord;
            Get["/scenario/{database}"] = Scenario;
            Get["/search/{database}/{expression*}"] = SearchRecords;
            Get["/terms/{database}/{count}/{term*}"] = ReadTerms;
            Get["/version"] = GetServerVersion;

            After.AddItemToEndOfPipeline
                (
                    ctx => ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                            .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                            .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type")
                );
        }

        #endregion

        #region Private members

        private IniFile _iniFile;

        /// <summary>
        /// Get connection.
        /// </summary>
        protected virtual IrbisConnection GetConnection()
        {
            var connectionString = CM.AppSettings["connectionString"];
            var connection = new IrbisConnection();
            connection.ParseConnectionString(connectionString);
            _iniFile = connection.Connect();

            Log.Trace("IrbisModule: Connected");
            connection.Disposing += (sender, args) => Log.Trace("IrbisModule: Disconnected");

            return connection;
        }

        /// <summary>
        /// Format records (GET).
        /// </summary>
        protected virtual Response FormatRecordsGet
            (
                dynamic parameters
            )
        {
            Log.Trace("IrbisModule: format GET");

            using (var connection = GetConnection())
            {
                string database = parameters.database;
                string many = parameters.mfns.ToString().Trim();
                var mfns = many.Split
                    (
                        new []{',', ' ', ';'}, 
                        StringSplitOptions.RemoveEmptyEntries
                    )
                    .Select
                    (
                        NumericUtility.ParseInt32
                    )
                    .ToArray();
                string format = parameters.format;
                var text = connection.FormatRecords
                    (
                        database, 
                        format, 
                        mfns
                    );

                return Response.AsJson(text);
            }
        }

        /// <summary>
        /// Format records (POST).
        /// </summary>
        protected virtual Response FormatRecordsPost
            (
                dynamic parameters
            )
        {
            Log.Trace("IrbisModule: format POST");

            using (var connection = GetConnection())
            {
                string database = parameters.database;
                string format = parameters.format;

                var mfns = RestUtility.ConvertRequestBody<int[]>(Request);
                var text = connection.FormatRecords
                    (
                        database,
                        format,
                        mfns
                    );

                return Response.AsJson(text);
            }
        }

        /// <summary>
        /// List database.
        /// </summary>
        protected virtual Response ListDatabases
            (
                dynamic parameters
            )
        {
            Log.Trace("IrbisModule: list databases");

            using (var connection = GetConnection())
            {
                var dbname = CM.AppSettings["dbname"];
                if (string.IsNullOrEmpty(dbname))
                {
                    dbname = _iniFile["MAIN", "DBNNAMECAT"];
                }
                if (string.IsNullOrEmpty(dbname))
                {
                    dbname = "dbnam3.mnu";
                }

                var databases = connection.ListDatabases(dbname);

                return Response.AsJson(databases);
            }
        }

        /// <summary>
        /// Get max MFN for specified database.
        /// </summary>
        protected virtual Response GetMaxMfn
            (
                dynamic parameters
            )
        {
            Log.Trace("IrbisModule: max MFN");

            using (var connection = GetConnection())
            {
                connection.Database = parameters.database;
                var maxMfn = connection.GetMaxMfn();

                return Response.AsJson(maxMfn);
            }
        }

        /// <summary>
        /// Read specified record.
        /// </summary>
        protected virtual Response ReadRecord
            (
                dynamic parameters
            )
        {
            Log.Trace("IrbisModule: read record");

            using (var connection = GetConnection())
            {
                connection.Database = parameters.database;
                int mfn = parameters.mfn;
                var record = connection.ReadRecord(mfn);

                return Response.AsJson(record);
            }
        }

        /// <summary>
        /// Read terms.
        /// </summary>
        protected virtual Response ReadTerms
            (
                dynamic parameters
            )
        {
            Log.Trace("IrbisModule: read terms");

            using (var connection = GetConnection())
            {
                var termParameters = new TermParameters
                {
                    Database = parameters.database,
                    NumberOfTerms = parameters.count,
                    StartTerm = parameters.term
                };
                var terms = connection.ReadTerms(termParameters);

                return Response.AsJson(terms);
            }
        }

        /// <summary>
        /// Search scenario.
        /// </summary>
        protected virtual Response Scenario
            (
                dynamic parameters
            )
        {
            Log.Trace("IrbisModule: scenario");

            using (var connection = GetConnection())
            {
                connection.Database = parameters.database;
                var scenario = SearchScenario.ParseIniFile(_iniFile);

                return Response.AsJson(scenario);
            }
        }

        /// <summary>
        /// Search records.
        /// </summary>
        protected virtual Response SearchRecords
            (
                dynamic parameters
            )
        {
            Log.Trace("IrbisModule: search");

            using (var connection = GetConnection())
            {
                connection.Database = parameters.database;
                int[] found = connection.Search(parameters.expression);

                return Response.AsJson(found);
            }
        }

        /// <summary>
        /// Get server version.
        /// </summary>
        protected virtual Response GetServerVersion
            (
                dynamic parameters
            )
        {
            Log.Trace("IrbisModule: version");

            using (var connection = GetConnection())
            {
                var version = connection.GetServerVersion();

                return Response.AsJson(version);
            }
        }

        #endregion
    }
}

#endif
