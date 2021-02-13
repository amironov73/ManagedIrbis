// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IrbisRestClient.cs -- client for IrbisModule
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4 || ANDROID

#region Using directives

using AM;
using AM.Logging;

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
        : IrbisProvider
    {
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
            Code.NotNullNorEmpty(url, nameof(url));

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
        [NotNull]
        public string FormatRecord
            (
                int mfn,
                [NotNull] string format
            )
        {
            Log.Trace("IrbisRestClient: format record");

            Code.Positive(mfn, nameof(mfn));
            Code.NotNullNorEmpty(format, nameof(format));

            var request = new RestRequest
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

            var response = _client.Execute(request);

            var result = JsonConvert.DeserializeObject<string>
                (
                    response.Content
                );

            return result;
        }

        /// <inheritdoc/>
        [ItemNotNull]
        public override string[] FormatRecords
            (
                [NotNull] int[] mfns,
                [NotNull] string format
            )
        {
            Log.Trace("IrbisRestClient: format records");

            RestRequest request;

            var method = Method.GET;
            if (mfns.Length > 10)
            {
                method = Method.POST;
                request = new RestRequest
                    (
                        "/format/{database}/{format}",
                        method
                    )
                {
                    RequestFormat = DataFormat.Json
                };

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

            var response = _client.Execute(request);
            var result = JsonConvert.DeserializeObject<string[]>
                (
                    response.Content
                );

            return result;
        }

        /// <inheritdoc/>
        public override int GetMaxMfn()
        {
            Log.Trace("IrbisRestClient: max MFN");

            var request = new RestRequest("/max/{database}");
            request.AddUrlSegment("database", Database);

            var response = _client.Execute(request);
            var result = JsonConvert.DeserializeObject<int>
                (
                    response.Content
                );

            return result;
        }

        /// <inheritdoc/>
        public override DatabaseInfo[] ListDatabases()
        {
            Log.Trace("IrbisRestClient: list databases");

            var request = new RestRequest("/list");

            var response = _client.Execute(request);
            var content = response.Content;
            var result
                = JsonConvert.DeserializeObject<DatabaseInfo[]>(content);

            return result;
        }

        /// <inheritdoc/>
        [NotNull]
        public override MarcRecord ReadRecord
            (
                int mfn
            )
        {
            Log.Trace("IrbisRestClient: read record");

            Code.Positive(mfn, nameof(mfn));

            var request
                = new RestRequest("/max/{database}/{mfn}");
            request.AddUrlSegment("database", Database);
            request.AddUrlSegment
                (
                    "mfn",
                    mfn.ToInvariantString()
                );

            var response = _client.Execute(request);
            var result
                = JsonConvert.DeserializeObject<MarcRecord>
                (
                    response.Content
                );

            return result;
        }

        /// <inheritdoc/>
        [NotNull]
        public override SearchScenario[] ReadSearchScenarios()
        {
            Log.Trace("IrbisRestClient: read search scenario");

            var request
                = new RestRequest("/scenario/{database}");
            request.AddUrlSegment("database", Database);

            var response = _client.Execute(request);
            var result
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
            Log.Trace("IrbisRestClient: read terms");

            Code.NotNull(parameters, nameof(parameters));

            var database = parameters.Database;
            if (string.IsNullOrEmpty(database))
            {
                database = Database;
            }

            var request
                = new RestRequest("/terms/{database}/{count}/{term*}");
            request.AddUrlSegment("database", database);
            request.AddUrlSegment
                (
                    "count",
                    parameters.NumberOfTerms.ToInvariantString()
                );
            request.AddUrlSegment("term", parameters.StartTerm);

            var response = _client.Execute(request);
            var result
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
            Log.Trace("IrbisRestClient: search");

            Code.NotNullNorEmpty(expression, nameof(expression));

            var request
                = new RestRequest("/search/{database}/{expression*}");
            request.AddUrlSegment("database", Database);
            request.AddUrlSegment("expression", expression);

            var response = _client.Execute(request);
            var result
                = JsonConvert.DeserializeObject<int[]>
                (
                    response.Content
                );

            return result;
        }

#endregion
    }
}

#endif
