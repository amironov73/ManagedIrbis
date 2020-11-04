// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UseNameofExpression

/* DicardsConfiguration.cs -- common DICARDS related configuration
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Text;

using AM;
using AM.Json;

using JetBrains.Annotations;

using CodeJam;

using Newtonsoft.Json;

#endregion

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// Общие настройки, связанные с DICARDS.
    /// </summary>
    public sealed class DicardsConfiguration
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Строка подключения к серверу ИРБИС64.
        /// </summary>
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// для подключения к DICARDS API.
        /// </summary>
        [JsonProperty("apiID")]
        public string ApiId { get; set; }

        /// <summary>
        /// Ключ для подключания к DICARDS API.
        /// </summary>
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        /// <summary>
        /// Базовый URL для подключения к DICARDS API.
        /// </summary>
        [JsonProperty("baseUri")]
        public string BaseUri { get; set; }

        /// <summary>
        /// Группа для извлечения читателей
        /// из репозитория DICARDS.
        /// </summary>
        [JsonProperty ("group")]
        public string Group { get; set; }

        /// <summary>
        /// Префикс для читательских билетов
        /// для читателей, извлеченных их репозитория DICARDS.
        /// </summary>
        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        /// <summary>
        /// Категория для читателей, извлеченных
        /// из репозитория DICARDS.
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Имя шаблона для карт DICARDS.
        /// </summary>
        [JsonProperty("template")]
        public string Template { get; set; }

        /// <summary>
        /// Сообщение о необходимости возвращать книги.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Формат для формирования списка задолженной литературы.
        /// </summary>
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// Поле карты, в которое помещается список задолженной литературы.
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Чтение конфигурации из указанного файла.
        /// </summary>
        [NotNull]
        public static DicardsConfiguration LoadConfiguration
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            var result = JsonUtility.ReadObjectFromFile<DicardsConfiguration>(fileName);
            result.ApiId = Unprotect(result.ApiId);
            result.ApiKey = Unprotect(result.ApiKey);
            result.ConnectionString = Unprotect(result.ConnectionString);

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

            var clone = (DicardsConfiguration) MemberwiseClone();
            clone.ApiId = Protect(ApiId);
            clone.ApiKey = Protect(ApiKey);
            clone.ConnectionString = Protect(ConnectionString);

            JsonUtility.SaveObjectToFile(clone, fileName);
        }

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

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            bool result = !string.IsNullOrEmpty(BaseUri)
                && !string.IsNullOrEmpty(ApiId)
                && !string.IsNullOrEmpty(ApiKey)
                && !string.IsNullOrEmpty(ConnectionString);

            if (throwOnError && !result)
            {
                throw new VerificationException();
            }

            return result;
        }

        #endregion
    }
}
