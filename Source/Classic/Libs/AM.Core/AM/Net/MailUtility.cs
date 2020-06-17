// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MailUtility.cs -- утилиты для работы с e-mail.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text.RegularExpressions;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Net
{
    /// <summary>
    /// Утилиты для работы с e-mail.
    /// </summary>
    public static class MailUtility
    {
        #region Public methods

        /// <summary>
        /// Очистка e-mail от лишних символов.
        /// </summary>
        [NotNull]
        public static string CleanupEmail
            (
                [NotNull] string email
            )
        {
            return email.Replace(" ", string.Empty);
        }

        /// <summary>
        /// Верификация (приблизительная) e-mail.
        /// </summary>
        public static bool VerifyEmail
            (
                [NotNull] string email
            )
        {
            Code.NotNullNorEmpty(email, "email");

            bool result = Regex.IsMatch
            (
                email,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase
            );

            return result;
        }

        #endregion
    }
}
