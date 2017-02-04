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

using System.Linq;

using AM;

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
            Get["/format/{database}/{mfns}/{format*}"] = FormatRecords;
            Get["/list"] = ListDatabases;
            Get["/max/{database}"] = GetMaxMfn;
            Get["/read/{database}/{mfn}"] = ReadRecord;
            Get["/search/{database}/{expression*}"] = SearchRecords;
            Get["/terms/{database}/{count}/{term*}"] = ReadTerms;
            Get["/version"] = GetServerVersion;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Get connection.
        /// </summary>
        protected virtual IrbisConnection GetConnection()
        {
            string connectionString = CM.AppSettings["connectionString"];
            IrbisConnection connection = new IrbisConnection(connectionString);

            Console.WriteLine("Connected");
            connection.Disposing += (sender, args) => Console.WriteLine("Disconnected");

            return connection;
        }

        /// <summary>
        /// Format records.
        /// </summary>
        protected virtual Response FormatRecords
            (
                dynamic parameters
            )
        {
            Console.WriteLine("CALLED: format");

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
                DatabaseInfoLite[] databases 
                    = DatabaseInfoLite.FromDatabaseInfo
                    (
                        connection.ListDatabases("dbnam3.mnu")
                    );

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
