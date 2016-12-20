// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WordShortener.cs -- 
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
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Morphology
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class WordShortener
    {
        #region Properties

        /// <summary>
        /// Standard endings to cut off completely.
        /// </summary>
        public static string[] StandardFullEndings =
        {
            "авский", "авская", "авское", "авские",
            "адский", "адская", "адское", "адские",
            "ажный", "ажная", "ажное", "ажные",
            "азский", "азская", "азское", "азские",
            "айский", "айская", "айское", "айские",
            "альный", "альная", "альное", "альные",
            "альский", "альская", "альское", "альские",
            "анный", "анная", "анное", "анные",
            "аннский", "аннская", "аннское", "аннские",
            "арский", "арская", "арское", "арские",
            "атский", "атская", "атское", "атские",
            "ейский", "айская", "ейское", "ейские",
            "ельный", "ельная", "ельное", "ельные",
            "ельский", "ельская", "ельское", "ельские",
            "енный", "енная", "енное", "енные",
            "енский", "енская", "енское", "енские",
            "ентальный", "ентальная", "ентальное", "ентальные",
            "ерский", "ерская", "ерское", "ерские",
            "еский", "еская", "еское", "еские",
            "иальный", "иальная", "иальное", "иальные",
            "ийский", "ийская", "ийское", "ийские",
            "инский", "инская", "инское", "инские",
            "ионный", "ионная", "ионное", "ионные",
            "ирский", "ирская", "ирское", "ирские",
            "ительный", "ительная", "ительное", "ительные",
            "ический", "ическая", "ическое", "ические",
            "кий", "кая", "кое", "кие",
            "ний", "няя", "нее", "ние",
            "ной", "ная", "ное", "ные",
            "ный", "ная", "ное", "ные",
            "ованный", "ованная", "ованное", "ованные",
            "овский", "овская", "овское", "овские",
            "одский", "одская", "одское", "одские",
            "ольский", "ольская", "ольское", "ольские",
            "орский", "орская", "орское", "орские",
            "ский", "ская", "ское", "ские",
            "ской", "ская", "ское", "ские",
            "ческий", "ческая", "ческое", "ческие",

        };

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Shorten the word by standard full ending.
        /// </summary>
        [NotNull]
        public static string ShortenByStandardFullEnding
            (
                [NotNull] string word
            )
        {
            Code.NotNullNorEmpty(word, "word");

            string result = word;

            int wordLength = word.Length;
            if (wordLength < 4)
            {
                return word;
            }

            foreach (string ending in StandardFullEndings)
            {
                int endingLength = ending.Length;
                int tailStart = wordLength - endingLength;
                if (tailStart < 2)
                {
                    continue;
                }

                string tail = word.Substring(tailStart).ToLower();
                if (string.CompareOrdinal(tail, ending) != 0)
                {
                    continue;
                }

                string head = word.Substring(0, tailStart);
                char last = head[head.Length - 1];
                while ((tailStart < wordLength)
                        &&
                        (
                            last == 'а' || last == 'А' || last == 'е' || last == 'Е'
                        || last == 'ё' || last == 'Ё' || last == 'и' || last == 'И'
                        || last == 'й' || last == 'Й' || last == 'о' || last == 'О'
                        || last == 'э' || last == 'Э' || last == 'ы' || last == 'Ы'
                        || last == 'ю' || last == 'Ю' || last == 'я' || last == 'Я'
                        || last == 'ь' || last == 'Ь'
                        )
                    )
                {
                    head = head + word[tailStart];
                    tailStart++;
                }
                if (head[tailStart - 1] == head[tailStart - 2])
                {
                    head = head.Substring(0, tailStart - 1);
                    //tailStart--;
                }

                head = head + '.';

                if (result.Length > head.Length)
                {
                    result = head;
                }
            }

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
