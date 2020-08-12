// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* OsmiRegistrationInfo.cs -- Данные о зарегистрированном пользователе
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Json;
using AM.Net;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// Данные о зарегистрированном пользователе системы
    /// DiCARDS.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class OsmiRegistrationInfo
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Номер карты.
        /// </summary>
        [JsonProperty("serialNo")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [JsonProperty("Middlename")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [JsonProperty("Surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Пол.
        /// </summary>
        [JsonProperty("Sex")]
        public string Gender { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        [JsonProperty("birthdate")]
        public string BirthDate { get; set; }

        /// <summary>
        /// Электронная почта.
        /// </summary>
        [JsonProperty("Email")]
        public string Email { get; set; }

        /// <summary>
        /// Телефон.
        /// </summary>
        [JsonProperty("Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Телефон подтвержден?
        /// </summary>
        [JsonProperty("PhoneCheck")]
        public string PhoneCheck { get; set; }

        /// <summary>
        /// Оферта подтверждена?
        /// </summary>
        [JsonProperty("OfertaCheck")]
        public string OfertaCheck { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Декодируем JSON от DiCARDS.
        /// </summary>
        [NotNull]
        public static OsmiRegistrationInfo FromJson
            (
                [NotNull] JObject obj
            )
        {
            Code.NotNull(obj, "obj");

            var result = new OsmiRegistrationInfo
            {
                Name = obj.GetString("Имя").NullForEmpty(),
                Surname = obj.GetString("Фамилия").NullForEmpty(),
                MiddleName = obj.GetString("Отчество").NullForEmpty(),
                Gender = obj.GetString("Пол").NullForEmpty(),
                BirthDate = obj.GetString("Дата_рождения").NullForEmpty(),
                Phone = obj.GetString("Телефон").NullForEmpty(),
                PhoneCheck = obj.GetString("Телефон_проверен").NullForEmpty(),
                OfertaCheck = obj.GetString("Оферта_принята").NullForEmpty(),
                SerialNumber = obj.GetString("serialNo").NullForEmpty()
            };

            if (!string.IsNullOrEmpty(result.Email))
            {
                result.Email = MailUtility.CleanupEmail(result.Email);
            }

            if (!string.IsNullOrEmpty(result.Phone))
            {
                result.Phone = PhoneUtility.CleanupNumber(result.Phone);
            }

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
            // TODO implement
            return true;
        }

        #endregion
    }
}
