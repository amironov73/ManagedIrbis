// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChaNode.cs -- 
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
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Gbl.Infrastructure.Ast
{
    //
    // Official documentation:
    // http://sntnarciss.ru/irbis/spravka/pril00704030000.htm
    //
    // Замена данных в поле или в подполе.
    //
    // Отличие двух операторов в том, что CHAC выполняется
    // с учетом регистра.Для определения заменяемых
    // и заменяющих данных используются ФОРМАТ 1 и ФОРМАТ 2.
    //
    // Выполняются следующие правила:
    // * Если ПОВТОРЕНИЕ задано не признаком ‘F’, то:
    // * первая строка ФОРМАТА 1 является той строкой, которая
    // ищется в заданном поле/подполе (в заданном повторении
    // или во всех повторениях) – строка A;
    // * первая строка ФОРМАТА 2 является строкой, которая
    // должна заменить найденную строку – строка B.
    // * Если строка А пустая, то строка В приписывается
    // в конец корректируемого поля/подполя.
    // * Если строка В пустая, то строка А удаляется.
    // Пустую строку следует получать, используя оператор
    // пропуска строки #.
    //
    // * Если ПОВТОРЕНИЕ задано признаком ‘F’, то:
    // * строки, получаемые ФОРМАТОМ 1 (строки Ai),
    // ищутся в соответствующих по порядку повторениях поля;
    // * строки, получаемые ФОРМАТОМ 2 (строки Bi),
    // заменяют в соответствующих их порядку повторениях
    // строки Ai, т.е.строка Bi заменит в i-ом повторении
    // заданного поля или в подполе заданного поля строку Ai;
    // * если строка Ai пустая, то строка Bi приписывается,
    // если строка Bi пустая, то строка Ai удаляется.
    // * Если в поле заданное для корректировки подполе
    // встречается несколько раз, то корректируются все подполя.
    // * Если строка А встречается в поле/подполе несколько раз,
    // то заменяются все строки А.
    // * Поиск строки A в тексте записи проводится без учета
    // регистра (перед сравнением все переводится в верхний регистр).
    // Строка B пишется в запись в том регистре, в котором задана.
    //

    /// <summary>
    /// Замена данных в поле или в подполе.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblCha
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "CHA";

        #endregion

        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region GblNode members

        /// <summary>
        /// Execute the node.
        /// </summary>
        public override void Execute
            (
                GblContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeExecution(context);

            // Nothing to do here

            OnAfterExecution(context);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Mnemonic;
        }

        #endregion
    }
}
