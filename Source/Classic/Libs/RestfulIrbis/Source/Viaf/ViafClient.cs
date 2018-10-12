// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ViafClient.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

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
    //
    // VIAF (англ. Virtual International Authority File - Виртуальный
    // международный авторитетный файл) - виртуальный каталог
    // международного нормативного контроля (информации о произведениях
    // и их авторах). В разработке проекта участвовало несколько
    // крупнейших мировых библиотек, в том числе Немецкая национальная
    // библиотека, Библиотека Конгресса США.
    //
    // VIAF является международно признанной системой классификации.
    // Это совместный проект нескольких национальных библиотек и управляется
    // Онлайновым компьютерным библиотечным центром (OCLC).
    // Проект был инициирован Немецкой национальной библиотекой
    // и Библиотекой Конгресса США. Проект основан в 2000 году.
    //

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
        public ViafData GetAuthorityClusterData
            (
                [NotNull] string recordId
            )
        {
            Code.NotNullNorEmpty(recordId, "recordId");

            RestRequest request = new RestRequest("/viaf/{id}/");
            request.AddUrlSegment("id", recordId);
            request.AddHeader("Accept", "application/json");
            IRestResponse response = Connection.Execute(request);
            JObject obj = JObject.Parse(response.Content);
            ViafData result = ViafData.Parse(obj);

            return result;
        }

        #endregion
    }
}

