// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DublinRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.DublinCore
{
    /// <summary>
    /// DC field tag.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class DublinTag
    {
        #region Constants

        /// <summary>
        /// Название/заглавие.
        /// </summary>
        public const string Title = "Title";

        /// <summary>
        /// Создатель/автор.
        /// </summary>
        public const string Creator = "Creator";

        /// <summary>
        /// Тема.
        /// </summary>
        public const string Subject = "Subject";

        /// <summary>
        /// Описание.
        /// </summary>
        public const string Description = "Description";

        /// <summary>
        /// Издатель.
        /// </summary>
        public const string Publisher = "Publisher";

        /// <summary>
        /// Внесший вклад.
        /// </summary>
        public const string Contributor = "Contributor";

        /// <summary>
        /// Дата.
        /// </summary>
        public const string Date = "Date";

        /// <summary>
        /// Тип.
        /// </summary>
        public const string Type = "Type";

        /// <summary>
        /// Формат документа.
        /// </summary>
        public const string Format = "Format";

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public const string Identifier = "Identifier";

        /// <summary>
        /// Источник.
        /// </summary>
        public const string Source = "Источник";

        /// <summary>
        /// Язык.
        /// </summary>
        public const string Language = "Language";

        /// <summary>
        /// Отношения.
        /// </summary>
        public const string Relation = "Relation";

        /// <summary>
        /// Покрытие.
        /// </summary>
        public const string Coverage = "Coverage";

        /// <summary>
        /// Авторские права.
        /// </summary>
        public const string Rights = "Rights";

        /// <summary>
        /// Аудитория (зрители).
        /// </summary>
        public const string Audience = "Audience";

        /// <summary>
        /// Происхождение.
        /// </summary>
        public const string Provenance = "Provenance";

        /// <summary>
        /// Правообладатель.
        /// </summary>
        public const string RightsHolder = "RightsHolder";

        #endregion

        #region Properties

        /// <summary>
        /// Все известные теги.
        /// </summary>
        [NotNull]
        public static string[] AllKnown =
        {
            Title, Creator, Subject, Description, Publisher,
            Contributor, Date, Type, Format, Identifier, Source,
            Language, Relation, Coverage, Rights, Audience,
            Provenance, RightsHolder
        };

        #endregion
    }
}
