// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisRestClient.cs -- 
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

using Newtonsoft.Json;

using ManagedIrbis;
using ManagedIrbis.Client;

using RestSharp;

#endregion

namespace RestfulIrbis
{
    /// <summary>
    /// 
    /// </summary>
    public class IrbisRestClient
        : AbstractClient
    {
        #region Properties

        #endregion

        #region Construction

        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        public IrbisRestClient
            (
                string url
            )
        {
            _client = new RestClient(url);
            Database = "IBIS";

            //Get["/format/{database}/{mfn}/{format*}"] = _Format;
            //Get["/list"] = _ListDatabases;
            //Get["/max/{database}"] = _MaxMfn;
            //Get["/read/{database}/{mfn}"] = _Read;
            //Get["/search/{database}/{expression*}"] = _Search;
            //Get["/terms/{database}/{count}/{term*}"] = _Terms;
            //Get["/version"] = _ServerVersion;
        }
        // ReSharper restore DoNotCallOverridableMethodsInConstructor

        #endregion

        #region Private members

        private readonly RestClient _client;

        #endregion

        #region Public methods

        /// <summary>
        /// Format record by MFN.
        /// </summary>
        public string FormatRecord
            (
                int mfn,
                string format
            )
        {
            RestRequest request 
                = new RestRequest("/format/{database}/{mfn}/{format}");
            request.AddUrlSegment("database", Database);
            request.AddUrlSegment("mfn", mfn.ToInvariantString());
            request.AddUrlSegment("format", format);

            IRestResponse response = _client.Execute(request);
            string result = response.Content;

            return result;
        }

        /// <inheritdoc/>
        public override string[] FormatRecords
            (
                int[] mfns,
                string format
            )
        {
            RestRequest request
                = new RestRequest("/format/{database}/{mfns}/{format}");
            request.AddUrlSegment("database", Database);
            request.AddUrlSegment
                (
                    "mfns", 
                    StringUtility.Join(",", mfns)
                );
            request.AddUrlSegment("format", format);

            IRestResponse response = _client.Execute(request);
            string[] result = JsonConvert.DeserializeObject<string[]>
                (
                    response.Content
                );

            return result;
        }

        /// <inheritdoc/>
        public override DatabaseInfo[] ListDatabases()
        {
            RestRequest request = new RestRequest("/list");
            IRestResponse response = _client.Execute(request);
            string content = response.Content;
            DatabaseInfoLite[] databases 
                = JsonConvert.DeserializeObject<DatabaseInfoLite[]>(content);
            DatabaseInfo[] result
                = DatabaseInfoLite.ToDatabaseInfo(databases);

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
