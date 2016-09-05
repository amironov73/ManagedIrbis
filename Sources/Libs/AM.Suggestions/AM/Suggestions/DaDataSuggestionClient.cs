/* DaDataSuggestionClient.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

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
        const string SUGGESTIONS_URL = "{0}/suggest";
        const string ADDRESS_RESOURCE = "address";
        const string PARTY_RESOURCE = "party";
        const string BANK_RESOURCE = "bank";
        const string FIO_RESOURCE = "fio";
        const string EMAIL_RESOURCE = "email";

        RestClient client;
        string token;
        ContentType contentType = ContentType.XML;

        public IWebProxy Proxy
        {
            get { return client.Proxy; }
            set { client.Proxy = value; }
        }

        static DaDataSuggestionClient()
        {
            // use SSL v3
            ServicePointManager.SecurityProtocol
                = SecurityProtocolType.Ssl3;
        }

        public DaDataSuggestionClient
            (
                string token,
                string baseUrl
            )
        {
            this.token = token;
            client = new RestClient
                (
                    String.Format(SUGGESTIONS_URL, baseUrl)
                );
        }

        public SuggestAddressResponse QueryAddress(string address)
        {
            var request = new RestRequest(ADDRESS_RESOURCE, Method.POST);
            var query = new SuggestQuery(address);
            return Execute<SuggestAddressResponse>(request, query, this.contentType);
        }

        public SuggestBankResponse QueryBank(string bank)
        {
            var request = new RestRequest(BANK_RESOURCE, Method.POST);
            var query = new SuggestQuery(bank);
            return Execute<SuggestBankResponse>(request, query, this.contentType);
        }

        public SuggestEmailResponse QueryEmail(string email)
        {
            var request = new RestRequest(EMAIL_RESOURCE, Method.POST);
            var query = new SuggestQuery(email);
            return Execute<SuggestEmailResponse>(request, query, this.contentType);
        }

        public SuggestFioResponse QueryFio(string fio)
        {
            var request = new RestRequest(FIO_RESOURCE, Method.POST);
            var query = new SuggestQuery(fio);
            return Execute<SuggestFioResponse>(request, query, this.contentType);
        }

        public SuggestPartyResponse QueryParty(string party)
        {
            var request = new RestRequest(PARTY_RESOURCE, Method.POST);
            var query = new SuggestQuery(party);
            return Execute<SuggestPartyResponse>(request, query, this.contentType);
        }

        private T Execute<T>(RestRequest request, SuggestQuery query, ContentType contentType) where T : new()
        {
            request.AddHeader("Authorization", "Token " + this.token);
            request.AddHeader("Content-Type", contentType.Name);
            request.AddHeader("Accept", contentType.Name);
            request.RequestFormat = contentType.Format;
            request.XmlSerializer.ContentType = contentType.Name;
            request.AddBody(query);
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            return response.Data;
        }
    }
}
