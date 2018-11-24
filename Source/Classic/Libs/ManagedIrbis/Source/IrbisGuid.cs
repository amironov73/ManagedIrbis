// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisGuid.cs -- GUID handling in IRBIS64
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    //
    // Поле GUID - уникальный идентификатор записи.
    //
    // Поле GUID не показывается с помощью команды V,
    // отсутствует с точки зрения функций P и A
    // Не показывается &uf('0'), &uf('1'), &uf('A'),
    // &uf('P'), &uf('+4') и &uf('++0').
    // Но показывается &uf('+0').
    //

    /// <summary>
    /// GUID handling in IRBIS64.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisGuid
    {
        #region Constants

        /// <summary>
        /// Метка поля для GUID.
        /// </summary>
        public const int Tag = 2147483647;

        /// <summary>
        /// Метка поля для GUID (строка).
        /// </summary>
        public const string TagString = "2147483647";

        #endregion

        #region Public methods

        /// <summary>
        /// Get GUID from the <see cref="MarcRecord"/>.
        /// </summary>
        [CanBeNull]
        public static string Get
            (
                [CanBeNull] MarcRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                return null;
            }

            string text = record.FM(Tag);
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            Guid guid = Parse(text);

            return StringUtility.ToUpperInvariant(guid.ToString("D"));
        }

        /// <summary>
        /// Create new GUID in IRBIS64 format.
        /// </summary>
        public static string NewGuid()
        {
            return StringUtility.ToUpperInvariant(Guid.NewGuid().ToString("B"));
        }

        /// <summary>
        /// Parse the text.
        /// </summary>
        public static Guid Parse
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

#if FW35 || WINMOBILE || POCKETPC

            return new Guid(text);

#else

            return Guid.Parse(text);

#endif
        }

        /// <summary>
        /// Parse the record.
        /// </summary>
        public static Guid? Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            string text = record.FM(Tag);

            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return Parse(text);
        }

        #endregion
    }
}