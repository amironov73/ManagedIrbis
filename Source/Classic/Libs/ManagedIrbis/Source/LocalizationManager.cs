// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalizationManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class LocalizationManager
    {
        #region Constants

        /// <summary>
        /// Delimiter.
        /// </summary>
        public const string Delimiter = "~~";

        #endregion

        #region Properties

        /// <summary>
        /// Current language code.
        /// </summary>
        [CanBeNull]
        public string CurrentLanguage { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalizationManager()
        {
            CurrentLanguage = "RU";
            _languages
                = new CaseInsensitiveDictionary<LanguageFile>();
        }

        #endregion

        #region Private members

        private readonly CaseInsensitiveDictionary<LanguageFile>
            _languages;

        #endregion

        #region Public methods

        /// <summary>
        /// Get <see cref="LanguageFile"/> for given code.
        /// </summary>
        [CanBeNull]
        public LanguageFile GetLanguageFile
            (
                [NotNull] string code
            )
        {
            Code.NotNullNorEmpty(code, "code");

            LanguageFile result;
            _languages.TryGetValue(code, out result);

            return result;
        }

        /// <summary>
        /// List available languages.
        /// </summary>
        [NotNull]
        public string[] ListAvailableLanguages()
        {
            string[] result = _languages
                .Select(pair => pair.Key)
                .OrderBy(line => line)
                .ToArray();

            return result;
        }

        /// <summary>
        /// Translate the text.
        /// </summary>
        [CanBeNull]
        public string Translate
            (
                [CanBeNull] string text
            )
        {
            return Translate
                (
                    text,
                    CurrentLanguage.ThrowIfNull("CurrentLanguage")
                );
        }

        /// <summary>
        /// Translate the text.
        /// </summary>
        [CanBeNull]
        public string Translate
            (
                [CanBeNull] string text,
                [NotNull] string code
            )
        {
            Code.NotNullNorEmpty(code, "code");

            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            LanguageFile language = GetLanguageFile(code);

            StringBuilder result
                = new StringBuilder(text.Length);
            TextNavigator navigator = new TextNavigator(text);
            string chunk;

            while (!navigator.IsEOF)
            {
                chunk = navigator.ReadTo(Delimiter);
                if (ReferenceEquals(chunk, null))
                {
                    // Не нашли разделителя, прекращаем
                    break;
                }

                if (!string.IsNullOrEmpty(chunk))
                {
                    result.Append(chunk);
                }

                // Если не конец текста, значит,
                // мы нашли разделитель
                if (!navigator.IsEOF)
                {
                    chunk = navigator.ReadTo(Delimiter);

                    if (ReferenceEquals(chunk, null))
                    {
                        // Если конец текста, значит,
                        // это был одинарный разделитель
                        result.Append(Delimiter);
                        break;
                    }

                    if (!string.IsNullOrEmpty(chunk))
                    {
                        // Это текст между двух разделителей

                        if (ReferenceEquals(language, null))
                        {
                            // Ничего не переводим,
                            // просто срезаем разделители

                            result.Append(chunk);
                        }
                        else
                        {
                            string translated
                                = language.GetTranslation
                                (
                                    chunk
                                );
                            if (!string.IsNullOrEmpty(translated))
                            {
                                result.Append(translated);
                            }
                        }
                    }
                }
            }
            chunk = navigator.GetRemainingText();
            if (!string.IsNullOrEmpty(chunk))
            {
                result.Append(chunk);
            }

            return result.ToString();
        }

        #endregion
    }
}
