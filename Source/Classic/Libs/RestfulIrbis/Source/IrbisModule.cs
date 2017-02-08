// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisModule.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4

#region Using directives

using System;
using System.IO;
using System.Text;
using System.Linq;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Nancy;

using Newtonsoft.Json;

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
            string connectionString = CM.AppSettings["connectionString"];
            IrbisConnection connection = new IrbisConnection();
            connection.ParseConnectionString(connectionString);
            _iniFile = connection.Connect();

            Console.WriteLine("Connected");
            connection.Disposing += (sender, args) => Console.WriteLine("Disconnected");

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
            Console.WriteLine("CALLED: format GET");

            using (IrbisConnection connection = GetConnection())
            {
                string database = parameters.database;
                string many = parameters.mfns.ToString().Trim();
                int[] mfns = many.Split
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
                string[] text = connection.FormatRecords
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
            Console.WriteLine("CALLED: format POST");

            using (IrbisConnection connection = GetConnection())
            {
                string database = parameters.database;
                string format = parameters.format;

                int[] mfns = RestUtility.ConvertRequestBody<int[]>(Request);
                string[] text = connection.FormatRecords
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
            Console.WriteLine("CALLED: list");

            using (IrbisConnection connection = GetConnection())
            {
                string dbname = CM.AppSettings["dbname"];
                if (string.IsNullOrEmpty(dbname))
                {
                    dbname = _iniFile["MAIN", "DBNNAMECAT"];
                }
                if (string.IsNullOrEmpty(dbname))
                {
                    dbname = "dbnam3.mnu";
                }

                DatabaseInfo[] databases = connection.ListDatabases(dbname);

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
            Console.WriteLine("CALLED: max");

            using (IrbisConnection connection = GetConnection())
            {
                connection.Database = parameters.database;
                int maxMfn = connection.GetMaxMfn();

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
            Console.WriteLine("CALLED: read");

            using (IrbisConnection connection = GetConnection())
            {
                connection.Database = parameters.database;
                int mfn = parameters.mfn;
                MarcRecord record = connection.ReadRecord(mfn);

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
            Console.WriteLine("CALLED: terms");

            using (IrbisConnection connection = GetConnection())
            {
                TermParameters tp = new TermParameters
                {
                    Database = parameters.database,
                    NumberOfTerms = parameters.count,
                    StartTerm = parameters.term
                };
                TermInfo[] terms = connection.ReadTerms(tp);

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
            Console.WriteLine("CALLED: scenario");

            using (IrbisConnection connection = GetConnection())
            {
                connection.Database = parameters.database;
                SearchScenario[] scenario = SearchScenario.ParseIniFile(_iniFile);

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
            Console.WriteLine("CALLED: search");

            using (IrbisConnection connection = GetConnection())
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
            Console.WriteLine("CALLED: version");

            using (IrbisConnection connection = GetConnection())
            {
                IrbisVersion version = connection.GetServerVersion();

                return Response.AsJson(version);
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}

#endif
