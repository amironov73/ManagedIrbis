// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DelNode.cs -- 
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
    // http://sntnarciss.ru/irbis/spravka/pril00704040000.htm
    //
    // Удаление поля или подполя в поле.
    //
    // Выполняются следующие правила:
    //
    // * Удаляется заданное повторение поля или в поле заданное подполе.
    // Если в поле несколько заданных подполей, то удаляются все.
    // * Если ПОВТОРЕНИЕ поля задано не признаком F,
    // то ФОРМАТ 1 и ФОРМАТ 2 не используются, соответствующие
    // столбцы блокируются и соответствующие строки
    // в файле задания заполняются символом-заполнителем.
    // * Если ПОВТОРЕНИЕ задано признаком F, то удаляются
    // повторения в зависимости от значения строк,
    // полученных ФОРМАТОМ 1. Если значение строки  ‘1’,
    // то соответствующее по порядку повторение удаляется, иначе нет.
    //
    // ФОРМАТ 2 не используется.
    //

    /// <summary>
    /// Удаляет поле или подполе в поле.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblDel
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "DEL";

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
