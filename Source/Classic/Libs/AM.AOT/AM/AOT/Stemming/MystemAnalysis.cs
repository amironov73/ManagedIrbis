/* MystemAnalysis.cs -- format error codes.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

#endregion

namespace AM.AOT.Stemming
{
    /// <summary>
    /// Анализ слова.
    /// </summary>
    public sealed class MystemAnalysis
    {
        /// <summary>
        /// Основная форма слова
        /// </summary>
        [JsonProperty("lex")]
        public string Lexeme { get; set; }

        /// <summary>
        /// Относительный вес
        /// </summary>
        [JsonProperty("wt")]
        public double Weight { get; set; }

        /// <summary>
        /// Грамматический разбор слова
        /// </summary>
        [JsonProperty("gr")]
        public string Grammeme { get; set; }

        private string[] Split
        {
            get
            {
                if (string.IsNullOrEmpty(Grammeme))
                    return new[] { String.Empty };
                return Grammeme.Split(',');
            }
        }

        /// <summary>
        /// Часть речи.
        /// </summary>
        public string PartOfSpeech
        {
            get { return Split[0].Trim('='); }
        }

        /// <summary>
        /// Обсценное?
        /// </summary>
        public bool IsObscene
        {
            get { return Split.Contains("обсц"); }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format
                (
                    "{0}, {1}",
                    Lexeme,
                    PartOfSpeech
                );
        }
    }
}
