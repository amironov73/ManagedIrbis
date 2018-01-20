// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WordInfo.cs -- 
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json.Linq;

#endregion

namespace AM.AOT.Distributional
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class WordInfo
    {
        #region Properties

        /// <summary>
        /// Word.
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// Part of speech.
        /// </summary>
        public string PartOfSpeech { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public float Value { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the <see cref="JProperty"/>
        /// </summary>
        [NotNull]
        public static WordInfo Parse
            (
                [NotNull] JProperty property
            )
        {
            Code.NotNull(property, "property");

            string text = property.Name;
            string[] parts = text.Split('_');
            if (parts.Length != 2)
            {
                throw new Exception();
            }
            WordInfo result = new WordInfo
            {
                Word = parts[0],
                PartOfSpeech = parts[1],
#if WINMOBILE || PocketPC
                Value = (float)NumericUtility.ParseDecimal(property.Value)
#else
                Value = property.Value.ToObject<float>()
#endif
            };

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
