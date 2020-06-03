// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text;

using AM.Logging;
using AM.Security;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisUtility
    {
        #region Public methods

        /// <summary>
        /// Декодирование строки подключения, если она закодирована или зашифрована.
        /// </summary>
        /// <param name="possiblyEncrypted">Строка подключения.</param>
        /// <param name="password">Пароль. Если null, то используется пароль по умолчанию.</param>
        /// <returns>Расшифрованная строка подключения.</returns>
        [NotNull]
        public static string DecryptConnectionString
            (
                [NotNull] string possiblyEncrypted,
                [CanBeNull] string password
            )
        {
            Code.NotNullNorEmpty(possiblyEncrypted, "possiblyEncrypted");

            // Пустая строка зашифрованной быть не может.
            if (string.IsNullOrEmpty(possiblyEncrypted))
            {
                return possiblyEncrypted;
            }

            // С восклицательного знака начинается строка,
            // закодированная в банальный Base64.
            if (possiblyEncrypted[0] == '!')
            {
                var enc = possiblyEncrypted.Substring(1);
                var res = SecurityUtility.DecryptFromBase64(enc);
                return res;
            }

            // Зашифрованная строка должна начинаться со знака вопроса.
            if (possiblyEncrypted[0] != '?')
            {
                return possiblyEncrypted;
            }

            var encrypted = possiblyEncrypted.Substring(1);
            if (string.IsNullOrEmpty(password))
            {
                password = "irbis";
            }

            var result = SecurityUtility.Decrypt(encrypted, password);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public static string EncodePercentString
            (
                [CanBeNull] byte[] array
            )
        {
            if (ReferenceEquals(array, null)
                || array.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();

            foreach (byte b in array)
            {
                if (b >= 'A' && b <= 'Z'
                    || b >= 'a' && b <= 'z'
                    || b >= '0' && b <= '9'
                    )
                {
                    result.Append((char)b);
                }
                else
                {
                    result.AppendFormat
                        (
                            "%{0:X2}",
                            b
                        );
                }
            }

            return result.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public static byte[] DecodePercentString
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return new byte[0];
            }

            int predictedLength = text.Length / 2;
            using (MemoryStream stream = MemoryManager.GetMemoryStream(predictedLength))
            {
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    if (c != '%')
                    {
                        stream.WriteByte((byte) c);
                    }
                    else
                    {
                        if (i >= text.Length - 2)
                        {
                            Log.Error
                                (
                                    "IrbisUtility::DecodePercentString: "
                                    + "unexpected end of stream"
                                );

                            throw new FormatException("text");
                        }

                        byte b = byte.Parse
                            (
                                text.Substring(i + 1, 2),
                                NumberStyles.HexNumber
                            );
                        stream.WriteByte(b);
                        i += 2;
                    }
                }

                return stream.ToArray();
            }
        }

        #endregion
    }
}
