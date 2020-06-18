// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MailUtility.cs -- утилиты для работы с e-mail.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;
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
        /// Перевод кириллических символов в латиницу.
        /// </summary>
        [NotNull]
        public static string CleanupEmail
            (
                [NotNull] string email
            )
        {
            var result = new StringBuilder(email.Length);
            foreach (var c in email)
            {
                // Пробелов и служебных символов вообще не должно быть
                if (c <= ' ')
                {
                    continue;
                }

                // Превращаем кириллические буквы, похожие на латиницу,
                // в визуально совпадающие латинские
                switch (c)
                {
                    case 'А':
                        result.Append('A');
                        break;

                    case 'а':
                        result.Append('a');
                        break;

                    case 'В':
                        result.Append('B');
                        break;

                    case 'в':
                        result.Append('b');
                        break;

                    case 'С':
                        result.Append('C');
                        break;

                    case 'с':
                        result.Append('c');
                        break;

                    case 'Е':
                        result.Append('E');
                        break;

                    case 'е':
                        result.Append('e');
                        break;

                    case 'Н':
                        result.Append('H');
                        break;

                    case 'п':
                        result.Append('n');
                        break;

                    case 'К':
                        result.Append('K');
                        break;

                    case 'к':
                        result.Append('k');
                        break;

                    case 'М':
                        result.Append('M');
                        break;

                    case 'м':
                        result.Append('m');
                        break;

                    case 'О':
                        result.Append('O');
                        break;

                    case 'о':
                        result.Append('o');
                        break;

                    case 'Р':
                        result.Append('P');
                        break;

                    case 'р':
                        result.Append('p');
                        break;

                    case 'Т':
                        result.Append('T');
                        break;

                    case 'т':
                        result.Append('t');
                        break;

                    case 'Х':
                        result.Append('x');
                        break;

                    case 'х':
                        result.Append('x');
                        break;

                    case 'У':
                        result.Append('Y');
                        break;

                    case 'у':
                        result.Append('y');
                        break;

                    default:
                        if (c < 256)
                        {
                            result.Append(char.ToUpper(c));
                        }
                        break;
                }
            }

            return result.ToString();
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
