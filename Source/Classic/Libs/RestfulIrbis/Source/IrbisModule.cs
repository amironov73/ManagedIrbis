// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisModule.cs -- 
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
using AM;
using ManagedIrbis;
using ManagedIrbis.Search;

using Nancy;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace RestfulIrbis
{
    public class IrbisModule
        : NancyModule
    {
        #region Properties

        #endregion

        #region Construction

        public IrbisModule()
        {
            Get["/"] = parameters => Response.AsText("Hello! This is RestfulIrbis!");
            Get["/format/{database}/{mfns}/{format*}"] = _FormatMany;
            Get["/list"] = _ListDatabases;
            Get["/max/{database}"] = _MaxMfn;
            Get["/read/{database}/{mfn}"] = _Read;
            Get["/search/{database}/{expression*}"] = _Search;
            Get["/terms/{database}/{count}/{term*}"] = _Terms;
            Get["/version"] = _ServerVersion;
        }

        #endregion

        #region Private members

        protected virtual IrbisConnection GetConnection()
        {
            string connectionString = CM.AppSettings["connectionString"];
            IrbisConnection connection = new IrbisConnection(connectionString);

            Console.WriteLine("Connected");
            connection.Disposing += (sender, args) => Console.WriteLine("Disconnected");

            return connection;
        }

        //public Response _Format
        //    (
        //        dynamic parameters
        //    )
        //{
        //    Console.WriteLine("CALLED: format");

        //    using (IrbisConnection connection = GetConnection())
        //    {
        //        connection.Database = parameters.database;
        //        int mfn = parameters.mfn;
        //        string format = parameters.format;
        //        string text = connection.FormatRecord(format, mfn);

        //        return Response.AsJson(text);
        //    }
        //}

        public Response _FormatMany
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
                        one => NumericUtility.ParseInt32(one)
                    )
                    .ToArray();
                string format = parameters.format;
                string[] text = connection.FormatRecords(database, format, mfns);

                return Response.AsJson(text);
            }
        }

        private Response _ListDatabases
            (
                dynamic parameters
            )
        {
            Console.WriteLine("CALLED: list");

            using (IrbisConnection connection = GetConnection())
            {
                DatabaseInfoLite[] databases = DatabaseInfoLite.FromDatabaseInfo
                    (
                        connection.ListDatabases("dbnam3.mnu")
                    );

                return Response.AsJson(databases);
            }
        }

        private Response _MaxMfn
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

        public Response _Read
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

        public Response _Terms
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

        public Response _Search
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

        public Response _ServerVersion
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
