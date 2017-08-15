// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfStatF.cs -- 
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
    // http://sntnarciss.ru/irbis/spravka/wa0203050260.htm
    //
    // STATF - Создать выходную статистическую форму
    // и вывести результат (на бумагу, в файл, на E-mail)
    //
    // Команда для пакетных заданий АРМов Администратор (серверный)
    // и Администратор-клиент ИРБИС64
    // (версия 2013.1 и выше выделены ВСЕ заглавными буквами курсивом).
    //
    // Назначение команды:
    // Создать выходную статистическую форму
    // и вывести результат(на бумагу, в файл, на E-mail)
    //
    // Оператор:
    // STATF
    // Операнды:
    // StatForm,modefield,mfnfrom,mfnto,[0|1/FileName|2/E-mail/Subject]
    // Где:
    // StatForm – имя стат.формы.
    // modefield – исходные параметры для стат.формы в виде
    // модельного поля с подполями.
    // (может быть пустым, если стат. форма не требует исходных параметров)
    // mfnfrom,mfnto – диапазон MFN исходных документов.
    // Пятый операнд – выходной носитель: (аналогично предыдущей команде)
    // Пример:
    // STATF Form3,^A20120101^B20131231,,,0
    // Примечания:
    // В серверном Администраторе команда не поддерживается.
    //

    /// <summary>
    /// Статистическая форма.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IbfStatF
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
