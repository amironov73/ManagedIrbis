/* DaDataSuggestionClient.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Net;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using RestSharp;

#endregion

namespace AM.Suggestions
{
    using DaData;

#pragma warning disable 1591

    /// <summary>
    /// Client for dadata.ru
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DaDataSuggestionClient
    {
        #region Constants

        /// <summary>
        /// URL for DaData API.
        /// </summary>
        public const string DaDataApiUrl = "https://dadata.ru/api/v2";

        private const string SuggestionsUrl = "{0}/suggest";
        private const string Address = "address";
        private const string Party = "party";
        private const string Bank = "bank";
        private const string Fio = "fio";
        private const string Email = "email";

        #endregion

        #region Properties

        /// <summary>
        /// Proxy specification.
        /// </summary>
        [CanBeNull]
        public IWebProxy Proxy
        {
            get { return _client.Proxy; }
            set { _client.Proxy = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static DaDataSuggestionClient()
        {
            // use SSL v3
            ServicePointManager.SecurityProtocol
                = SecurityProtocolType.Ssl3;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DaDataSuggestionClient
            (
                [NotNull] string token
            )
            : this(token, "https://dadata.ru/api/v2")
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DaDataSuggestionClient
        (
            [NotNull] string token,
            [NotNull] string baseUrl
        )
        {
            Code.NotNullNorEmpty(token, "token");
            Code.NotNullNorEmpty(baseUrl, "baseUrl");

            _token = token;
            _client = new RestClient
            (
                string.Format(SuggestionsUrl, baseUrl)
            );
            _contentType = ContentType.XML;
        }

        #endregion

        #region Private members

        private readonly RestClient _client;
        private readonly string _token;
        private readonly ContentType _contentType;

        private T Execute<T>
            (
                RestRequest request,
                SuggestQuery query,
                ContentType contentType
            )
            where T : new()
        {
            request.AddHeader("Authorization", "Token " + _token);
            request.AddHeader("Content-Type", contentType.Name);
            request.AddHeader("Accept", contentType.Name);
            request.RequestFormat = contentType.Format;
            request.XmlSerializer.ContentType = contentType.Name;
            request.AddBody(query);
            IRestResponse<T> response = _client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            return response.Data;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Query address suggestion.
        /// </summary>
        [NotNull]
        public SuggestAddressResponse QueryAddress
            (
                [NotNull] string address
            )
        {
            Code.NotNullNorEmpty(address, "address");

            RestRequest request = new RestRequest
                (
                    Address,
                    Method.POST
                );
            SuggestQuery query = new SuggestQuery(address);
            SuggestAddressResponse result
                = Execute<SuggestAddressResponse>
                (
                    request,
                    query,
                    _contentType
                );

            return result;
        }

        /// <summary>
        /// Query bank suggestion.
        /// </summary>
        [NotNull]
        public SuggestBankResponse QueryBank
            (
                [NotNull] string bank
            )
        {
            Code.NotNullNorEmpty(bank, "bank");

            RestRequest request = new RestRequest(Bank, Method.POST);
            SuggestQuery query = new SuggestQuery(bank);
            SuggestBankResponse result = Execute<SuggestBankResponse>
                (
                    request,
                    query,
                    _contentType
                );

            return result;
        }

        /// <summary>
        /// Query e-mail suggestion.
        /// </summary>
        [NotNull]
        public SuggestEmailResponse QueryEmail
            (
                [NotNull] string email
            )
        {
            Code.NotNullNorEmpty(email, "email");

            RestRequest request = new RestRequest(Email, Method.POST);
            SuggestQuery query = new SuggestQuery(email);
            SuggestEmailResponse result = Execute<SuggestEmailResponse>
                (
                    request,
                    query,
                    _contentType
                );

            return result;
        }

        /// <summary>
        /// Query name suggestion.
        /// </summary>
        [NotNull]
        public SuggestFioResponse QueryFio
            (
                [NotNull] string fio
            )
        {
            Code.NotNullNorEmpty(fio, "fio");

            RestRequest request = new RestRequest(Fio, Method.POST);
            SuggestQuery query = new SuggestQuery(fio);
            SuggestFioResponse result = Execute<SuggestFioResponse>
                (
                    request,
                    query,
                    _contentType
                );

            return result;
        }

        /// <summary>
        /// Query party suggestion.
        /// </summary>
        [NotNull]
        public SuggestPartyResponse QueryParty
            (
                [NotNull] string party
            )
        {
            Code.NotNullNorEmpty(party, "party");

            RestRequest request = new RestRequest(Party, Method.POST);
            SuggestQuery query = new SuggestQuery(party);
            SuggestPartyResponse result = Execute<SuggestPartyResponse>
                (
                    request,
                    query,
                    _contentType
                );

            return result;
        }

        #endregion
    }
}
