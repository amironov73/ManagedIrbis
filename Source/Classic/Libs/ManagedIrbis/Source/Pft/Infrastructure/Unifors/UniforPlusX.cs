// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusX.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Search;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанная функция unifor('+X')
    // Параметры: А#Б.
    // Ищет в базе термин из первого параметра, перемещается по инверсному файлу.
    // Пока найденны термин не встанет дальше указанного текста.
    // Модифицирует параметр Б той же функцией, что unifor('+W').
    // Контекст не закрывает, можно продолжить поиск через NextTerm.
    //

    static class UniforPlusX
    {
        #region Public methods

        /// <summary>
        /// Инкремент строки.
        /// </summary>
        public static void SearchIncrement
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            // если в строке больше, чем 1 символ #, лишние части отбрасываются
            string[] parts = StringUtility.SplitString
                (
                    expression,
                    CommonSeparators.NumberSign,
                    StringSplitOptions.None
                );
            if (parts.Length < 2)
            {
                return;
            }

            string term = parts[0];
            if (string.IsNullOrEmpty(term))
            {
                return;
            }
            int result = 0;
            while (true)
            {
                IrbisProvider provider = context.Provider;

                // имитация NextTerm, поиск, начиная с указанного термина
                // с подхватом следующего
                // ищем по 10 терминов, чтобы меньше дергать базу
                TermParameters parameters = new TermParameters
                {
                    Database = provider.Database,
                    StartTerm = term,
                    NumberOfTerms = 10
                };
                TermInfo[] terms = provider.ReadTerms(parameters);
                if (terms.Length == 0)
                {
                    break;
                }
                for (int i = 0; i < terms.Length; i++)
                {
                    result = string.CompareOrdinal(terms[i].Text, parts[0]);
                    term = terms[i].Text;
                    if (result > 0)
                    {
                        break;
                    }
                }

                if (result == 1)
                {
                    break;
                }
            }

            // TODO Implement
            // contextIrbis64.UniforPlusXTerm = term;

            if (!string.IsNullOrEmpty(parts[1]))
            {
                string output = UniforPlusW.Increment(parts[1]);
                context.WriteAndSetFlag(node, output);
            }
        }

        #endregion
    }
}
