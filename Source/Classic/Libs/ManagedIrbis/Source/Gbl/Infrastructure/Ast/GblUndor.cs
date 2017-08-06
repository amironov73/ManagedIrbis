// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblUndor.cs -- 
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
    //
    // Переход к одной из предыдущих копий записи (откат)
    //
    // Количество выполняемых назад шагов задается параметром,
    // который определяется как значение формата в колонке
    // «Параметр1/Поле-подполе».
    // Значение параметра, как результат форматирования,
    // может быть следующим:
    // N - откат к N-й, относительно существующей,
    // копии записи (N= 1 - предыдущая копия)
    // * - откат к исходной копии записи
    // Пусто - нет действий
    //
    // Примеры использования команды
    // В каждом случае Формат строки параметра вырабатывает
    // значение, задающее число шагов назад.
    //
    // Пример 1:
    // В команде задается явным образом количество шагов отката,
    // например, для отката на два шага:
    // UNDOR
    // ‘2’
    //
    // Пример 2:
    // Использование совместно с новым форматным выходом
    // ФОРМАТИРОВАНИЕ ПРЕДЫДУЩЕЙ КОПИИ – «&uf(‘4…». 
    // Откатиться к копии записи, в которой выполняется некое условие,
    // предположим, наличие в поле 700^A текста ‘иванов’.
    // Это можно выполнить следующими операторами.
    // В глобальной переменной 1 будет количество шагов
    // к копиям записи. Начальное значение пусто,
    // т. е. было выполнено &uf(‘+7W1#’).
    // Организуется цикл по копиям записи
    // REPEAT
    // // в переменную G1 - номер очередной копии
    // &uf('+7W1#',, f(val(G1)+1,0,0))
    // UNTIL
    // // выход из цикла, если условие выполнено или исчерпаны копии
    // if s(&uf('4',, G1,,',v700^A')): 'иванов' or val(G1)>=val(&uf('4')) then '0' else '1' fi
    // // в G1 кол-во шагов к нужной копии
    // UNDOR
    // G1
    //
    // Пример 3:
    // Использование в повторяющейся группе совместно
    // с «&uf(‘4…». Откатиться к копии записи,
    // в которой поле 210^D содержало значение «1991».
    // UNDOR
    // &uf('+7W1#0'),,,,,,,( &uf('+7W1#',, f(val(&uf('AG1#1'))+1,0,0) )   if &uf('4,v210^D')='1991' or val(&uf('AG1#1')) >= val(&uf(‘4’))    then break fi ),,,,,,,,,,G1
    //
    // Счетчик повторений
    // Переменная 1 является счетчиком повторений в группе.
    // Сначала она опустошается, далее в каждом проходе группы
    // она увеличивается на 1, также берется очередная копия
    // записи(&uf(’4…..) и в ней проверяется поставленное условие.
    // Выход из группы (break) выполняется, если условие выполнено
    // или исчерпаны копии. При выходе из группы
    // в переменной 1 количество шагов, на которые откатывается запись.
    //

    /// <summary>
    /// Переход к одной из предыдущих копий записи (откат).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblUndor
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "UNDOR";

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
