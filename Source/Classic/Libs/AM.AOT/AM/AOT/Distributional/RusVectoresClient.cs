// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RusVectoresClient.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json.Linq;

using RestSharp;

#endregion

namespace AM.AOT.Distributional
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RusVectoresClient
    {
        #region Constants

        /// <summary>
        /// Default base URL.
        /// </summary>
        public const string DefaultBaseUrl = "http://rusvectores.org/";

        #endregion

        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public RestClient Connection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RusVectoresClient()
            : this(DefaultBaseUrl)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RusVectoresClient
            (
                [NotNull] string baseUrl
            )
        {
            Code.NotNullNorEmpty(baseUrl, "baseUrl");

            Connection = new RestClient(baseUrl);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        // =========================================================

        /// <summary>
        /// Get nearest words.
        /// </summary>
        /// <example>{
        /// "ruscorpora": {
        /// "бетон_NOUN": {
        /// "цемент_NOUN": 0.721661627293,
        /// "бетонный_ADJ": 0.710907399654,
        /// "железобетон_NOUN": 0.674247562885,
        /// "пенобетон_NOUN": 0.642662227154,
        /// "цементный_ADJ": 0.638867855072,
        /// "железобетонный_ADJ": 0.638613522053,
        /// "заполнитель_NOUN": 0.628975093365,
        /// "портландцемент_NOUN": 0.627707123756,
        /// "кирпич_NOUN": 0.626487612724,
        /// "перлитовый_ADJ": 0.626266539097
        /// } } }
        /// </example>
        [NotNull]
        public WordInfo[] GetNearestWords
            (
                [NotNull] string modelName,
                [NotNull] string word
            )
        {
            Code.NotNullNorEmpty(modelName, "modelName");
            Code.NotNullNorEmpty(word, "word");

            RestRequest request = new RestRequest
                (
                    "{model}/{word}/api/json",
                    Method.GET
                );
            request.AddUrlSegment("model", modelName);
            request.AddUrlSegment("word", word);

            IRestResponse response = Connection.Execute(request);

            WordInfo[] result = new WordInfo[0];

            try
            {
                JObject jObject = JObject.Parse(response.Content);

                result
                    = (jObject.First.First.First.First).Children()
                        .OfType<JProperty>()
                        // ReSharper disable ConvertClosureToMethodGroup
                        .Select(prop => WordInfo.Parse(prop))
                        // ReSharper restore ConvertClosureToMethodGroup
                        .ToArray();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}

#endif
