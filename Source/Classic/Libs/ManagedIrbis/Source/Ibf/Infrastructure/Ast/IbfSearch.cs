// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfSearch.cs -- 
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
    // Поиск и отбор записей.
    // http://sntnarciss.ru/irbis/spravka/wa0203050180.htm
    //
    // SEARCH - Выполнить поиск
    //
    // Команда для пакетных заданий АРМов Администратор(серверный)
    // и Администратор-клиент ИРБИС64 (версия 2013.1 и выше).
    //
    // Назначение команды:
    // Выполнить поиск
    // Оператор:
    // SEARCH
    // Операнды:
    //
    // SEXP_DIR, MFNFrom, MFNTО,SEXP_SEQ,JUMP
    // Где:
    // SEXP_DIR – поисковое выражение для прямого поиска
    // на языке запросов ИРБИС (может быть пустым)
    // SEXP_SEQ – поисковое выражение для уточняющего
    // последовательного поиска (может быть пустым)
    // MFNFrom – начальное значение диапазона MFN
    // для последовательного поиска(по умолчанию – 1)
    // MFNTo – конечное значение диапазона MFN для
    // последовательного поиска(по умолчанию – максимальный MFN БД)
    // JUMP – количество последующих пакетных команд,
    // которые должны быть пропущены в случае нулевого
    // результата поиска (по умолчанию – 0)
    //
    // Пример:
    // SEARCH “K=противопожарн$” * “K=оборудован$”,,,p(v10),2
    // Примечания:
    //
    // В серверном Администраторе команда не поддерживается.
    // Команда устанавливает контекст работы: РЕЗУЛЬТАТ ПОИСКА,
    // т.е для последующих команд – таких как EXPORTDB, COPYDB,
    // GLOBAL, PRINT, STAT, STATF – в качестве исходных документов
    // будет использоваться результат поиска.
    // Для того чтобы переключить контекст работы на БД ЦЕЛИКОМ,
    // необходимо задать команду OPENDB
    //

    /// <summary>
    /// Поиск и отбор записей.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IbfSearch
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
