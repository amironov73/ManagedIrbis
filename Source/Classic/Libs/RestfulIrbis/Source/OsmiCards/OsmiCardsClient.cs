// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OsmiCardsClient.cs -- 
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json.Linq;

using RestSharp;

#endregion

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class OsmiCardsClient
    {
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
        public OsmiCardsClient
            (
                [NotNull] string baseUrl,
                [NotNull] string apiId,
                [NotNull] string apiKey
            )
        {
            Code.NotNullNorEmpty(baseUrl, "baseUrl");
            Code.NotNullNorEmpty(apiId, "apiId");
            Code.NotNullNorEmpty(apiKey, "apiKey");

            Connection = new RestClient(baseUrl)
            {
                Authenticator = new DigestAuthenticator
                    (
                        apiId,
                        apiKey
                    )
            };
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Проверить ранее выданный PIN-код.
        /// </summary>
        public void CheckPinCode()
        {
        }

        /// <summary>
        /// Создать новую карту.
        /// </summary>
        public void CreateCard
            (
                string card
            )
        {
        }

        /// <summary>
        /// Создать новый шаблон.
        /// </summary>
        public void CreateTemplate
            (
                string template
            )
        {
        }

        /// <summary>
        /// Удалить карту.
        /// </summary>
        public void DeleteCard
            (
                string card,
                bool push
            )
        {
        }

        /// <summary>
        /// Запросить информацию по карте.
        /// </summary>
        public OsmiCard GetCardInfo
            (
                [NotNull] string cardNumber
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");

            RestRequest request = new RestRequest
                (
                    "/passes/{number}",
                    Method.GET
                );
            request.AddUrlSegment("number", cardNumber);
            IRestResponse response = Connection.Execute(request);
            JObject jObject = JObject.Parse(response.Content);
            OsmiCard result = OsmiCard.FromJObject(jObject);

            return result;
        }

        /// <summary>
        /// Запросить ссылку на загрузку карты.
        /// </summary>
        public string GetCardLink
            (
                [NotNull] string cardNumber
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");

            RestRequest request = new RestRequest
                (
                    "/passes/{number}/link",
                    Method.GET
                );
            request.AddUrlSegment("number", cardNumber);
            IRestResponse response = Connection.Execute(request);
            JObject result = JObject.Parse(response.Content);

            return result["link"].Value<string>();
        }

        /// <summary>
        /// Запросить список карт.
        /// </summary>
        public string[] GetCardList()
        {
            RestRequest request = new RestRequest
                (
                    "/passes",
                    Method.GET
                );
            IRestResponse response = Connection.Execute(request);
            JObject result = JObject.Parse(response.Content);

            return result["cards"].Values<string>().ToArray();
        }

        /// <summary>
        /// Запросить общие параметры сервиса.
        /// </summary>
        public JObject GetDefaults()
        {
            RestRequest request = new RestRequest
                (
                    "/defaults/all",
                    Method.GET
                );
            IRestResponse response = Connection.Execute(request);
            JObject result = JObject.Parse(response.Content);

            return result;
        }

        /// <summary>
        /// Запросить список доступных графических файлов.
        /// </summary>
        public OsmiImage[] GetImages()
        {
            RestRequest request = new RestRequest
                (
                    "/images",
                    Method.GET
                );
            IRestResponse response = Connection.Execute(request);
            JObject result = JObject.Parse(response.Content);

            return result["images"].ToObject<OsmiImage[]>();
        }

        /// <summary>
        /// Запросить общую статистику.
        /// </summary>
        public JObject GetStat()
        {
            RestRequest request = new RestRequest
                (
                    "/stats/general",
                    Method.GET
                );
            IRestResponse response = Connection.Execute(request);
            JObject result = JObject.Parse(response.Content);

            return result;
        }

        /// <summary>
        /// Запросить информацию о шаблоне.
        /// </summary>
        public JObject GetTemplateInfo
            (
                [NotNull] string templateName
            )
        {
            Code.NotNullNorEmpty(templateName, "templateName");

            RestRequest request = new RestRequest
                (
                    "/templates/{name}",
                    Method.GET
                );
            request.AddUrlSegment("name", templateName);
            IRestResponse response = Connection.Execute(request);
            JObject result = JObject.Parse(response.Content);

            return result;
        }

        /// <summary>
        /// Запросить список доступных шаблонов.
        /// </summary>
        public string[] GetTemplateList()
        {
            RestRequest request = new RestRequest
                (
                    "/templates",
                    Method.GET
                );
            IRestResponse response = Connection.Execute(request);
            JObject result = JObject.Parse(response.Content);

            return result["templates"].Values<string>().ToArray();
        }

        /// <summary>
        /// Проверить подключение к сервису.
        /// </summary>
        public JObject Ping()
        {
            RestRequest request = new RestRequest
                (
                    "ping",
                    Method.GET
                );
            IRestResponse response = Connection.Execute(request);
            JObject result = JObject.Parse(response.Content);

            return result;
        }

        /// <summary>
        /// Текстовый поиск по содержимому полей карт.
        /// </summary>
        public string[] SearchCards()
        {
            return null;
        }

        /// <summary>
        /// Отправить ссылку на загрузку карты по email.
        /// </summary>
        public void SendCardMail
            (
                string cardNumber,
                string message
            )
        {
        }

        /// <summary>
        /// Отправить ссылку на загрузку карты по СМС.
        /// </summary>
        public void SendCardSms
            (
                string cardNumber,
                string message
            )
        {
        }

        /// <summary>
        /// Отправить PIN-код по СМС.
        /// </summary>
        public void SendPinCode
            (
                string phoneNumber
            )
        {
        }

        /// <summary>
        /// Отправить push-сообщение на указанные карты.
        /// </summary>
        public void SendPushMessage
            (
                [NotNull] string[] cardNumbers,
                [NotNull] string messageText
            )
        {
            Code.NotNull(cardNumbers, "cardNumbers");
            Code.NotNullNorEmpty(messageText, "messageText");

            RestRequest request = new RestRequest
                (
                    "/marketing/pushmessage",
                    Method.POST
                )
            {
                RequestFormat = DataFormat.Json
            };

            JObject obj = new JObject();
            obj.Add("serials", new JArray(cardNumbers));
            obj.Add("message", messageText);
            request.AddParameter
                (
                    "application/json; charset=utf-8", 
                    obj.ToString(), 
                    ParameterType.RequestBody
                );

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        /// <summary>
        /// Переместить карту на другой шаблон.
        /// </summary>
        public void SetCardTemplate
            (
                string cardName,
                string template,
                bool push
            )
        {
        }

        /// <summary>
        /// Изменить общие параметры сервиса.
        /// </summary>
        public void SetDefaults
            (
                string newSettings
            )
        {
        }

        /// <summary>
        /// Обновить значения карты.
        /// </summary>
        public void UpdateCard
            (
                string card,
                bool push
            )
        {
        }

        /// <summary>
        /// Обновить значения шаблона.
        /// </summary>
        public void UpdateTemplate
            (
                string template,
                bool push
            )
        {
        }

        #endregion

        #region Object members

        #endregion
    }
}
