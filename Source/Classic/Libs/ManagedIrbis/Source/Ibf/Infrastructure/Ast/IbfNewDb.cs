// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfNewDb.cs -- 
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

namespace ManagedIrbis.Ibf.Infrastructure.Ast
{
    //
    // http://sntnarciss.ru/irbis/spravka/wa0203050130.htm
    //
    // Операнды:
    // Dbname,FullName,[0|1], [0|1|2],<имя_БД-образца>
    // Где:
    // Первый операнд - Dbname - имя БД;
    // Второй операнд - FullName - Полное название БД;
    // Третий операнд - доступность БД читателю:
    // 0 - не доступна читателям
    // 1 - доступна читателям;
    // Четвертый параметр – вид БД(не поддерживается в Администраторе-клиент):
    // 0 – БД электронного каталога
    // 1 – Произвольная БД
    // 2 - БД по образцу существующей
    // Пятый операнд - имя БД образца;
    // Пример:
    // NewDB TEST, Тестовая,0,0
    // Примечания:
    // В Администраторе-клиент не поддерживается четвертый операнд,
    // т.е. создать можно только БД ЭК.
    //

    /// <summary>
    /// Создание новой базы данных.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IbfNewDb
        : IbfNode
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IbfNode members

        /// <inheritdoc cref="IbfNode.Execute" />
        public override void Execute
            (
                IbfContext context
            )
        {
            OnBeforeExecution(context);

            OnAfterExecution(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}
