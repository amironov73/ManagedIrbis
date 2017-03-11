// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisRestClient.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4

#region Using directives

using AM;

using CodeJam;

using JetBrains.Annotations;

using Newtonsoft.Json;

using ManagedIrbis;

using ManagedIrbis.Client;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using RestSharp;

#endregion

namespace RestfulIrbis
{
    /// <summary>
    /// Client for IrbisModule.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class IrbisRestClient
        : AbstractClient
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        public IrbisRestClient
            (
                [NotNull] string url
            )
        {
            Code.NotNullNorEmpty(url, "url");

            _client = new RestClient(url);
            Database = "IBIS";

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
                [NotNull] string format
            )
        {
            Code.Positive(mfn, "mfn");
            Code.NotNullNorEmpty(format, "format");

            RestRequest request = new RestRequest
                (
                    "/format/{database}/{mfn}/{format}"
                );
            request.AddUrlSegment("database", Database);
            request.AddUrlSegment
                (
                    "mfn",
                    mfn.ToInvariantString()
                );
            request.AddUrlSegment("format", format);

            IRestResponse response = _client.Execute(request);
            string result = JsonConvert.DeserializeObject<string>
                (
                    response.Content
                );

            return result;
        }

        /// <inheritdoc/>
        public override string[] FormatRecords
            (
                int[] mfns,
                string format
            )
        {
            RestRequest request;

            Method method = Method.GET;
            if (mfns.Length > 10)
            {
                method = Method.POST;
                request = new RestRequest
                    (
                        "/format/{database}/{format}",
                        method
                    );

                request.RequestFormat = DataFormat.Json;
                request.AddUrlSegment("database", Database);
                request.AddUrlSegment("format", format);
                request.AddParameter
                    (
                        "application/json; charset=utf-8", 
                        JsonConvert.SerializeObject(mfns),
                        ParameterType.RequestBody
                    );
            }
            else
            {
                request = new RestRequest
                    (
                        "/format/{database}/{mfns}/{format}",
                        method
                    );
                request.AddUrlSegment("database", Database);
                request.AddUrlSegment
                    (
                        "mfns",
                        StringUtility.Join(",", mfns)
                    );
                request.AddUrlSegment("format", format);
            }

            IRestResponse response = _client.Execute(request);
            string[] result = JsonConvert.DeserializeObject<string[]>
                (
                    response.Content
                );

            return result;
        }

        /// <inheritdoc/>
        public override int GetMaxMfn()
        {
            RestRequest request
                = new RestRequest("/max/{database}");
            request.AddUrlSegment("database", Database);

            IRestResponse response = _client.Execute(request);
            int result = JsonConvert.DeserializeObject<int>
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
            DatabaseInfo[] result
                = JsonConvert.DeserializeObject<DatabaseInfo[]>(content);

            return result;
        }

        /// <inheritdoc/>
        public override MarcRecord ReadRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            RestRequest request
                = new RestRequest("/max/{database}/{mfn}");
            request.AddUrlSegment("database", Database);
            request.AddUrlSegment
                (
                    "mfn",
                    mfn.ToInvariantString()
                );

            IRestResponse response = _client.Execute(request);
            MarcRecord result
                = JsonConvert.DeserializeObject<MarcRecord>
                (
                    response.Content
                );

            return result;
        }

        /// <inheritdoc/>
        public override SearchScenario[] ReadSearchScenarios()
        {
            RestRequest request
                = new RestRequest("/scenario/{database}");
            request.AddUrlSegment("database", Database);

            IRestResponse response = _client.Execute(request);
            SearchScenario[] result
                = JsonConvert.DeserializeObject<SearchScenario[]>
                (
                    response.Content
                );

            return result;
        }

        /// <inheritdoc/>
        public override TermInfo[] ReadTerms
            (
                TermParameters parameters
            )
        {
            Code.NotNull(parameters, "parameters");

            string database = parameters.Database;
            if (string.IsNullOrEmpty(database))
            {
                database = Database;
            }

            RestRequest request
                = new RestRequest("/terms/{database}/{count}/{term*}");
            request.AddUrlSegment("database", database);
            request.AddUrlSegment
                (
                    "count",
                    parameters.NumberOfTerms.ToInvariantString()
                );
            request.AddUrlSegment("term", parameters.StartTerm);

            IRestResponse response = _client.Execute(request);
            TermInfo[] result
                = JsonConvert.DeserializeObject<TermInfo[]>
                (
                    response.Content
                );

            return result;
        }

        /// <inheritdoc/>
        public override int[] Search
            (
                string expression
            )
        {
            Code.NotNullNorEmpty(expression, "expression");

            RestRequest request
                = new RestRequest("/search/{database}/{expression*}");
            request.AddUrlSegment("database", Database);
            request.AddUrlSegment("expression", expression);

            IRestResponse response = _client.Execute(request);
            int[] result
                = JsonConvert.DeserializeObject<int[]>
                (
                    response.Content
                );

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}

#endif
