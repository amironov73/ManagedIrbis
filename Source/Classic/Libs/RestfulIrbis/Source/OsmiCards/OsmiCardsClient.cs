// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* OsmiCardsClient.cs -- клиент DiCARDS
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json.Linq;

using RestSharp;

#endregion

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// Клиент DiCARDS.
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

        // =========================================================

        /// <summary>
        /// Проверить ранее выданный PIN-код.
        /// </summary>
        public void CheckPinCode()
        {
            RestRequest request = new RestRequest
                (
                    "/activation/checkpin",
                    Method.POST
                );

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Создать новую карту.
        /// </summary>
        public void CreateCard
            (
                [NotNull] string cardNumber,
                [NotNull] string template
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");
            Code.NotNullNorEmpty(template, "template");

            RestRequest request = new RestRequest
                (
                    "/passes/{number}/{template}",
                    Method.POST
                )
            {
                RequestFormat = DataFormat.Json
            };
            request.AddUrlSegment("number", cardNumber);
            request.AddUrlSegment("template", template);

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Создать новую карту.
        /// </summary>
        public void CreateCard
            (
                [NotNull] string cardNumber,
                [NotNull] string template,
                [NotNull] string jsonText
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");
            Code.NotNullNorEmpty(template, "template");
            Code.NotNullNorEmpty(jsonText, "jsonText");

            RestRequest request = new RestRequest
                (
                    "/passes/{number}/{template}",
                    Method.POST
                );
            request.AddUrlSegment("number", cardNumber);
            request.AddUrlSegment("template", template);
            request.AddQueryParameter("withValues", "true");
            request.SetJsonRequestBody(jsonText);

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Создать новый шаблон.
        /// </summary>
        public void CreateTemplate
            (
                [NotNull] string templateName,
                [NotNull] string jsonText
            )
        {
            Code.NotNullNorEmpty(templateName, "templateName");
            Code.NotNullNorEmpty(jsonText, "jsonText");

            RestRequest request = new RestRequest
                (
                    "/templates/{template}",
                    Method.POST
                );
            request.AddUrlSegment("template", templateName);
            request.SetJsonRequestBody(jsonText);

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Удалить карту.
        /// </summary>
        public void DeleteCard
            (
                [NotNull] string cardNumber,
                bool push
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");

            string url = "/passes/{number}";
            if (push)
            {
                url += "/push";
            }

            RestRequest request = new RestRequest
                (
                    url,
                    Method.DELETE
                );
            request.AddUrlSegment("number", cardNumber);

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Запросить информацию по карте.
        /// </summary>
        [CanBeNull]
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

        // =========================================================

        /// <summary>
        /// Запросить "сырую" информацию по карте.
        /// </summary>
        [CanBeNull]
        public JObject GetRawCard
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
            JObject result = JObject.Parse(response.Content);

            return result;
        }

        // =========================================================

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

            return result["link"]?.Value<string>();
        }

        // =========================================================

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

            return result["cards"]?.Values<string>().ToArray();
        }

        // =========================================================

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

        // =========================================================

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

            return result["images"]?.ToObject<OsmiImage[]>();
        }

        // =========================================================

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

        // =========================================================

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
            string content = string.Empty;
            HttpStatusCode statusCode = HttpStatusCode.NoContent;

            JObject result;
            try
            {
                IRestResponse response = Connection.Execute(request);
                content = response.Content ?? string.Empty;
                statusCode = response.StatusCode;
                result = JObject.Parse(content);
            }
            catch (Exception inner)
            {
                Encoding encoding = Encoding.UTF8;
                ArsMagnaException outer = new ArsMagnaException("Error Get template info", inner);
                outer.Attach(new BinaryAttachment("content", encoding.GetBytes(content)));
                outer.Attach(new BinaryAttachment("statusCode", encoding.GetBytes(statusCode.ToString())));

                throw outer;
            }

            return result;
        }

        // =========================================================

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

            return result["templates"]?.Values<string>().ToArray();
        }

        // =========================================================

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

        // =========================================================

        /// <summary>
        /// Текстовый поиск по содержимому полей карт.
        /// </summary>
        public string[] SearchCards
            (
                [NotNull] string text
            )
        {
            RestRequest request = new RestRequest
                (
                    "/search/passes",
                    Method.POST
                );

            JObject requestJObject = new JObject
            {
                {"text", text}
            };
            request.SetJsonRequestBody
                (
                    requestJObject.ToString()
                );

            IRestResponse response = Connection.Execute(request);
            JArray responseArray = JArray.Parse(response.Content);

            List<string> result = new List<string>();
            foreach (JObject element in responseArray
                .Children<JObject>())
            {
                var item = element["serial"]?.Value<string>();
                if (!string.IsNullOrEmpty(item))
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        // =========================================================

        /// <summary>
        /// Отправить ссылку на загрузку карты по email.
        /// </summary>
        public void SendCardMail
            (
                [NotNull] string cardNumber,
                [NotNull] string email
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");
            Code.NotNullNorEmpty(email, "email");

            RestRequest request = new RestRequest
                (
                    "/passes/{number}/email/{email}",
                    Method.GET
                );
            request.AddUrlSegment("number", cardNumber);
            request.AddUrlSegment("email", email);

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Отправить ссылку на загрузку карты по СМС.
        /// </summary>
        public void SendCardSms
            (
                [NotNull] string cardNumber,
                [NotNull] string phoneNumber
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");
            Code.NotNullNorEmpty(phoneNumber, "phoneNumber");

            RestRequest request = new RestRequest
                (
                    "/passes/{number}/sms/{phone}",
                    Method.GET
                );
            request.AddUrlSegment("number", cardNumber);
            request.AddUrlSegment("phone", phoneNumber);

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Отправить PIN-код по СМС.
        /// </summary>
        public void SendPinCode
            (
                [NotNull] string phoneNumber
            )
        {
            Code.NotNullNorEmpty(phoneNumber, "phoneNumber");

            RestRequest request = new RestRequest
                (
                    "/activation/sendpin/{phone}",
                    Method.POST
                );

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

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
                );

            JObject obj = new JObject();
            object[] serials = cardNumbers
                .Cast<object>()
                .ToArray();
            obj.Add("serials", new JArray(serials));
            obj.Add("message", messageText);
            request.SetJsonRequestBody
                (
                    obj.ToString()
                );

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Переместить карту на другой шаблон.
        /// </summary>
        public void SetCardTemplate
            (
                [NotNull] string cardNumber,
                [NotNull] string template,
                bool push
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");
            Code.NotNullNorEmpty(template, "template");

            string url = "/passes/move/{number}/{template}";
            if (push)
            {
                url += "/push";
            }

            RestRequest request = new RestRequest
                (
                    url,
                    Method.PUT
                );
            request.AddUrlSegment("number", cardNumber);
            request.AddUrlSegment("template", template);

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Изменить общие параметры сервиса.
        /// </summary>
        public void SetDefaults
            (
                [NotNull] string newSettings
            )
        {
            Code.NotNullNorEmpty(newSettings, "newSettings");

            RestRequest request = new RestRequest
                (
                    "/defaults",
                    Method.PUT
                );
            request.SetJsonRequestBody(newSettings);

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Обновить значения карты.
        /// </summary>
        public void UpdateCard
            (
                [NotNull] string cardNumber,
                [NotNull] string jsonText,
                bool push
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");
            Code.NotNullNorEmpty(jsonText, "jsonText");

            var url = "/passes/{number}";
            if (push)
            {
                url += "/push";
            }

            var request = new RestRequest
                (
                    url,
                    Method.PUT
                );
            request.AddUrlSegment("number", cardNumber);
            request.SetJsonRequestBody(jsonText);

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Обновить значения шаблона.
        /// </summary>
        public void UpdateTemplate
            (
                [NotNull] string templateName,
                [NotNull] string jsonText,
                bool push
            )
        {
            Code.NotNullNorEmpty(templateName, "templateName");
            Code.NotNullNorEmpty(jsonText, "jsonText");

            var url = "/templates/{template}";
            if (push)
            {
                url += "/push";
            }

            var request = new RestRequest
                (
                    url,
                    Method.PUT
                );
            request.AddUrlSegment("template", templateName);
            request.SetJsonRequestBody(jsonText);

            /* IRestResponse response = */
            Connection.Execute(request);
        }

        // =========================================================

        /// <summary>
        /// Получение регистрационных данных для карт.
        ///
        /// Эта команда позволяет получить ранее сохраненные
        /// регистрационные данные для карт, которые использовали
        /// параметры полей из заданной группы. Данные возвращаются
        /// только для карт со статусом <code>–registered–</code>.
        /// </summary>
        /// <param name="groupName">Имя группы</param>
        /// <returns>Массив регистрационных данных.</returns>
        [NotNull]
        public OsmiRegistrationInfo[] GetRegistrations
            (
                [NotNull] string groupName
            )
        {
            Code.NotNullNorEmpty(groupName, "groupName");

            var request = new RestRequest
                (
                    "/registration/data/{group}",
                    Method.GET
                );
            request.AddUrlSegment("group", groupName);
            var response = Connection.Execute(request);
            var content = JObject.Parse(response.Content);
            var registrations = (JArray) content["registrations"];
            if (ReferenceEquals(registrations, null))
            {
                return EmptyArray<OsmiRegistrationInfo>.Value;
            }

            var result = new List<OsmiRegistrationInfo>();
            foreach (var registration in registrations)
            {
                var info = OsmiRegistrationInfo.FromJson((JObject) registration);
                result.Add(info);
            }

            return result.ToArray();
        }

        // =========================================================

        /// <summary>
        /// Удаление регистрационных данных карт.
        ///
        /// Эта команда удаляет ранее сохраненные регистрационные
        /// данные карт.
        /// </summary>
        /// <param name="numbers">Список серийных номеров карт.</param>
        public void DeleteRegistrations
            (
                [NotNull] string[] numbers
            )
        {
            Code.NotNull(numbers, "numbers");

            RestRequest request = new RestRequest
                (
                    "/registration/deletedata",
                    Method.POST
                );
        }

        // =========================================================

        #endregion

        #region Object members

        #endregion
    }
}

#endif
