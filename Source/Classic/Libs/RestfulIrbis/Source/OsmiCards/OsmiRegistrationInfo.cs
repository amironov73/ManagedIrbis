// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OsmiRegistrationInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable StringLiteralTypo

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// Данные о зарегистрированном пользователе системы
    /// DiCards.
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
