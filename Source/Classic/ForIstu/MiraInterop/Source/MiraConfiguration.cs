// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* MiraConfiguration.cs -- конфигурация для связи с системой MIRA
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Text;

using AM;
using AM.Json;

using CodeJam;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace MiraInterop
{
    /*
     * https://int.istu.edu/rest/25444/30zse8y4dmao4akd/library.send.message.get/
     * ?key=60669f73e0c77e0a7daeabd570502e0f
     * &miraid=2416266
     * &text=Проверка системы оповещния библиотеки о текущей задолженности читеталя. За вами числится задолженность перед библиотекой: 2 книги. Порядок возврата смотрите на сайте библиотеки: http://library.istu.edu/2020/05/28/o-vozvrate-knig-v-biblioteku/
     * &title=Тест системы. Задолженность перед библиотекой
     * &option=mail
     */

    /// <summary>
    /// Конфигурация для связи с системой MIRA.
    /// </summary>
    public class MiraConfiguration
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Строка подключения к серверу ИРБИС64.
        /// </summary>
        [JsonProperty("irbisConnectionString")]
        public string IrbisConnectionString { get; set; }

        /// <summary>
        /// Строка подключения к серверу MSSQL.
        /// </summary>
        [JsonProperty("mssqlConnectionString")]
        public string MssqlConnectionString { get; set; }

        /// <summary>
        /// Базовый URL.
        /// </summary>
        [JsonProperty("baseUri")]
        public string BaseUri { get; set; }

        /// <summary>
        /// Ключ.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Текст сообщения с возможным форматированием.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Заголовок сообщения.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Опция.
        /// </summary>
        [JsonProperty("option")]
        public string Option { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Примитивная защита от подглядывания паролей и прочего.
        /// Работает только против совсем неопытных пользователей.
        /// </summary>
        public static string Protect
            (
                [CanBeNull] string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var bytes = Encoding.UTF8.GetBytes(value);
            var result = "!" + Convert.ToBase64String(bytes);

            return result;
        }

        /// <summary>
        /// Раскодирование (при необходимости) строкового значения,
        /// закодированного методом <see cref="Protect"/>.
        /// </summary>
        [CanBeNull]
        public static string Unprotect
            (
                [CanBeNull] string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.FirstChar() != '!')
            {
                return value;
            }

            var bytes = Convert.FromBase64String(value.Substring(1));
            var result = Encoding.UTF8.GetString(bytes);

            return result;
        }

        /// <summary>
        /// Чтение конфигурации из указанного файла.
        /// </summary>
        [NotNull]
        public static MiraConfiguration LoadConfiguration
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            var result = JsonUtility.ReadObjectFromFile<MiraConfiguration>(fileName);
            result.IrbisConnectionString = Unprotect(result.IrbisConnectionString);
            result.MssqlConnectionString = Unprotect(result.MssqlConnectionString);
            result.BaseUri = Unprotect(result.BaseUri);
            result.Key = Unprotect(result.Key);

            return result;
        }

        /// <summary>
        /// Запись конфигурации в указанный файл.
        /// </summary>
        public void SaveConfiguration
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            var clone = (MiraConfiguration)MemberwiseClone();
            clone.IrbisConnectionString = Protect(IrbisConnectionString);
            clone.MssqlConnectionString = Protect(MssqlConnectionString);
            clone.BaseUri = Protect(BaseUri);
            clone.Key = Protect(Key);

            JsonUtility.SaveObjectToFile(clone, fileName);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            bool result = !string.IsNullOrEmpty(BaseUri)
                          && !string.IsNullOrEmpty(Key)
                          && !string.IsNullOrEmpty(Message)
                          && !string.IsNullOrEmpty(Title);

            if (throwOnError && !result)
            {
                throw new VerificationException();
            }

            return result;
        }

        #endregion
    }
}
