/* IrbisOpt.cs -- .opt files handling
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    // Оптимизированный формат – это механизм автоматического
    // переключения форматов показа документов в зависимости
    // от их вида. Переключение производится в соответствии
    // с содержанием специального файла, имя которого
    // определяется параметром PFTOPT. Данный файл оптимизации
    // является текстовым и имеет следующую структуру:
    //
    // <метка>|<формат>|@<имя_формата>
    // <длина>
    // <значение_1> <имя формата_1>
    // <значение_2> <имя формата_2>
    // <значение_3> <имя формата_3>
    // *****
    //
    // где:
    // <метка>|<формат>|@<имя_формата> - ключ, который может
    // задаваться тремя способами:
    // <метка> - метка поля, значение которого определяет
    // вид документа;
    // <формат> - непосредственный формат, с помощью которого
    // определяется значение для вида документа;
    // @<имя_формата> - имя формата с предшествующим символом @,
    // с помощью которого определяется значение для вида документа.
    // <длина> - макс.длина значения для вида документа;
    // <значение_n> <имя формата_n> - значение (вид документа)
    // и соответствующий ему формат, разделенные символом пробела.
    // При этом в элементе <значение_n> могут содержаться символы
    // маскирования «+» (означающие, что на соответствующем месте
    // может быть любой символ).
    // Для БД электронного каталога (IBIS) предлагаются два
    // оптимизационных файла:
    // PFTW.OPT – включает RTF-форматы;
    // 	PFTW_H.OPT – включает HTML-форматы.

    // В исходном состоянии системы в качестве оптимизированного
    // определены HTML-форматы (т.е. PFTOPT=PFTW_H.OPT).
    // Для перехода на RTF-форматы (в качестве оптимизированного)
    // необходимо установить PFTOPT=PFTW.OPT.


    // 920
    // 5
    // PAZK  PAZK42
    // PVK   PVK42
    // SPEC  SPEC42
    // J     !RPJ51
    // NJ    !NJ31
    // NJP   !NJ31
    // NJK   !NJ31
    // AUNTD AUNTD42
    // ASP   ASP42
    // MUSP  MUSP
    // SZPRF SZPRF
    // BOUNI BOUNI
    // IBIS  IBIS
    // +++++ PAZK42
    // *****


    /// <summary>
    /// OPT files handling
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisOpt
    {
        #region Nested classes

        #endregion

        #region Properties

        /// <summary>
        /// Length of worksheet i
        /// </summary>
        public int WorksheetLength { get; set; }

        /// <summary>
        /// Tag that identifies worksheet.
        /// Common used: 920
        /// </summary>
        public string WorksheetTag { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
