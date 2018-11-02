// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisGuid.cs -- GUID handling in IRBIS64
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
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

            string result = record.FM(Tag);

            return result;
        }

        /// <summary>
        /// Create new GUID in IRBIS64 format.
        /// </summary>
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString("B").ToUpperInvariant();
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

            return Guid.Parse(text);
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
            return string.IsNullOrEmpty(text)
                ? (Guid?) null
                : Parse(text);
        }

        #endregion
    }
}