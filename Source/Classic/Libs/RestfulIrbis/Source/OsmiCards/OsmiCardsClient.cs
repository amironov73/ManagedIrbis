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

using RestSharp;
using RestSharp.Authenticators;

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
                [NotNull] string apiID,
                [NotNull] string apiKey
            )
        {
            Code.NotNullNorEmpty(baseUrl, "baseUrl");
            Code.NotNullNorEmpty(apiID, "apiID");
            Code.NotNullNorEmpty(apiKey, "apiKey");

            Connection = new RestClient(baseUrl)
            {
                Authenticator = new DigestAuthenticator
                    (
                        apiID,
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
        public string GetCardInfo
            (
                string cardNumber
            )
        {
            return null;
        }

        /// <summary>
        /// Запросить ссылку на загрузку карты.
        /// </summary>
        public string GetCardLink
            (
                string cardNumber
            )
        {
            return null;
        }

        /// <summary>
        /// Запросить список карт.
        /// </summary>
        public string[] GetCardList()
        {
            return null;
        }

        /// <summary>
        /// Запросить общие параметры сервиса.
        /// </summary>
        public string GetDefaults()
        {
            return null;
        }

        /// <summary>
        /// Запросить общую статистику.
        /// </summary>
        public string GetStat()
        {
            return null;
        }

        /// <summary>
        /// Запросить информацию о шаблоне.
        /// </summary>
        public string GetTemplateInfo
            (
                string templateName
            )
        {
            return null;
        }

        /// <summary>
        /// Запросить список доступных шаблонов.
        /// </summary>
        public string[] GetTemplateList()
        {
            return null;
        }

        /// <summary>
        /// Проверить подключение к сервису.
        /// </summary>
        public string Ping()
        {
            return null;
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
