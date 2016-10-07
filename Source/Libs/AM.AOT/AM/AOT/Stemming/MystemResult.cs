/* MystemResult.cs -- format error codes.
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
    /// Результат анализа для одного слова.
    /// </summary>
    public sealed class MystemResult
    {
        /// <summary>
        /// Text.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Analysis.
        /// </summary>
        public MystemAnalysis[] Analysis { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(Text);
            foreach (MystemAnalysis analysis in Analysis)
            {
                result.Append("; ");
                result.Append(analysis);
            }

            return result.ToString();
        }
    }
}
