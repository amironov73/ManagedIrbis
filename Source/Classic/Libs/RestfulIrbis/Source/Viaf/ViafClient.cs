// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ViafRequester.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4 || NETCORE

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

#endregion

// ReSharper disable StringLiteralTypo

namespace RestfulIrbis.Viaf
{
    /// <summary>
    /// VIAF requester.
    /// </summary>
    [PublicAPI]
    public class ViafClient
    {
        #region Constants

        /// <summary>
        /// Base URL.
        /// </summary>
        public const string BaseUrl = "http://www.viaf.org/";

        #endregion

        #region Properties

        /// <summary>
        /// Connection
        /// </summary>
        [NotNull]
        public RestClient Connection { get; private set; }


        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ViafClient()
            : this (BaseUrl)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ViafClient
            (
                [NotNull] string baseUrl
            )
        {
            Code.NotNullNorEmpty(baseUrl, "baseUrl");

            Connection = new RestClient(baseUrl);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get suggestions for the name.
        /// </summary>
        [NotNull]
        public ViafSuggestResult[] GetSuggestions
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            RestRequest request = new RestRequest("/viaf/AutoSuggest?query={name}");
            request.AddUrlSegment("name", name);
            IRestResponse response = Connection.Execute(request);
            ViafSuggestResponse viaf
                = JsonConvert.DeserializeObject<ViafSuggestResponse>(response.Content);

            return viaf.SuggestResults;
        }

        /// <summary>
        /// Get Authority Cluster Data.
        /// </summary>
        [NotNull]
        public JObject GetAuthorityClusterData
            (
                [NotNull] string recordId
            )
        {
            Code.NotNullNorEmpty(recordId, "recordId");

            RestRequest request = new RestRequest("/viaf/{id}/");
            request.AddUrlSegment("id", recordId);
            request.AddHeader("Accept", "application/json");
            IRestResponse response = Connection.Execute(request);
            JObject result = JObject.Parse(response.Content);

            return result;
        }

        #endregion
    }
}

#endif
