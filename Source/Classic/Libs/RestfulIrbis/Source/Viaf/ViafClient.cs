// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ViafClient.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !UAP

#region Using directives

using AM.Logging;

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
            Log.Trace($"ViafClient: constructor: {baseUrl}");

            Code.NotNullNorEmpty(baseUrl, nameof(baseUrl));

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
            Log.Trace("ViafClient: get suggestions");

            Code.NotNullNorEmpty(name, nameof(name));

            var request = new RestRequest("/viaf/AutoSuggest?query={name}");
            request.AddUrlSegment("name", name);
            var response = Connection.Execute(request);
            var viaf
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
            Log.Trace("ViafClient: get authority cluster data");

            Code.NotNullNorEmpty(recordId, nameof(recordId));

            var request = new RestRequest("/viaf/{id}/");
            request.AddUrlSegment("id", recordId);
            request.AddHeader("Accept", "application/json");
            var response = Connection.Execute(request);
            var obj = JObject.Parse(response.Content);
            var result = ViafData.Parse(obj);

            return result;
        }

        #endregion
    }
}

#endif
