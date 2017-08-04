// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RepNode.cs -- 
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
    // http://sntnarciss.ru/irbis/spravka/pril00704020000.htm
    //
    // Замена целиком поля или подполя на новое значение,
    // которое задается ФОРМАТОМ 1.
    //
    // Выполняются следующие правила:
    // * Если ПОВТОРЕНИЕ задано не признаком ‘F’,
    // то заданное поле/подполе заменяется на строку,
    // которую формирует ФОРМАТ 1 (используется только первая строка,
    // остальные строки игнорируются).
    // * Если ПОВТОРЕНИЕ задано признаком ‘F’, то строки,
    // формируемые ФОРМАТОМ 1 заменяют повторения поля или подполя
    // в повторении. Причем, номер строки по формату 1 соответствует
    // номеру корректируемого повторения записи.
    // * Если повторений в записи больше чем строк формата 1,
    // то корректируются только те, для которых есть строки.
    // Если повторений в записи меньше чем строк ФОРМАТА 1,
    // то лишние строки ФОРМАТА 1 не используются.
    // * Если ФОРМАТ 1 дает пустую строку, то соответствующее
    // поле/подполе удаляется. Пустую строку следует получать,
    // используя оператор пропуска строки #.
    //
    // Во всех случаях ФОРМАТ 2 не используется и соответствующие
    // строки в файле задания заполняются символом-заполнителем.
    //

    /// <summary>
    /// Замена целиком поля или подполя.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblRep
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "REP";

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
