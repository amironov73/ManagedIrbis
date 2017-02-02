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

using RestSharp;

#endregion

namespace RestfulIrbis
{
    /// <summary>
    /// 
    /// </summary>
    public class IrbisRestClient
    {
        #region Properties

        public string Database { get; set; }

        #endregion

        #region Construction

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

        #endregion

        #region Private members

        private readonly RestClient _client;

        #endregion

        #region Public methods

        public string FormatRecord
            (
                string format,
                int mfn
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

        public DatabaseInfo[] ListDatabases()
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
