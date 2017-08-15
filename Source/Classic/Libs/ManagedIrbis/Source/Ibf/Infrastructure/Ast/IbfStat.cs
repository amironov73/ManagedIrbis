// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfStat.cs -- 
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
    // http://sntnarciss.ru/irbis/spravka/wa0203050240.htm
    //
    // STAT - Выполнить задание на статистику
    // и вывести результат (на бумагу, в файл, на E-mail)
    // Команда для пакетных заданий АРМов Администратор (серверный)
    // и Администратор-клиент ИРБИС64
    // (версия 2013.1 и выше выделены ВСЕ заглавными буквами курсивом).
    //
    // Назначение команды:
    // Выполнить задание на статистику и вывести результат
    // (на бумагу, в файл, на E-mail)
    // Оператор:
    // STAT
    // Операнды:
    //
    // StatList,mfnfrom,mfnto,[0|1/FileName|2/E-mail/Subject]
    // Где:
    // StatList – перечень заданий на статистики в виде:
    // Stat1|Stat2|….|StatN
    // StatN – описание одной статистики в виде:
    // TAG/LENGTH/NUMB/SORT
    // где:
    // TAG – метка поля/подполя или явный формат в уникальных ограничителях
    // (который не должен содержать символов OPERANDSEP)
    // LENGTH – длина анализируемого значения(по умолчанию – 10),
    // NUMB – максимальное кол-во значений(по умолчанию – 1000),
    // SORT – тип сортировки:
    // 0 – без сортировки,
    // 1 – сортировка по значению,
    // 2 – сортировка по кол-ву(убывание),
    // 3 – сортировка по кол-ву(возрастание);
    // mfnfrom,mfnto – диапазон MFN исходных документов.
    // Четвертый операнд – выходной носитель:
    // (аналогично предыдущей команде).
    //
    // Пример:
    // STAT 900^C|"(v102/)"/3/200/2,,,1/c:\irbiswrk\stat.txt
    // Примечания:
    // В серверном Администраторе команда не поддерживается.
    //

    /// <summary>
    /// Статистика по базе данных.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IbfStat
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
