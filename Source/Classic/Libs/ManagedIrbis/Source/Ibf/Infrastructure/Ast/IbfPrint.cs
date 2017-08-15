// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfPrint.cs -- 
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
    // http://sntnarciss.ru/irbis/spravka/wa0203050220.htm
    //
    // PRINT - Вывести документы
    // Команда для пакетных заданий АРМов Администратор (серверный)
    // и Администратор-клиент ИРБИС64
    // (версия 2013.1 и выше выделены ВСЕ заглавными буквами курсивом).
    //
    // Назначение команды:
    // Вывести документы на печать(на бумагу, в файл, на E-mail)
    // Оператор:
    // PRINT
    //
    // Операнды:
    // [0|1],FormName,mfnfrom,mfnto,header1/header2/header3,modefield,[0|1/FileName|2/E-mail/Subject]
    // Где:
    // Первый операнд – вид печати:
    // 0 – список
    // 1 – табличная форма
    // FormName  - имя формата/табличной формы.
    // Mfnfrom, mfnto – диапазон MFN исходных документов.
    // header1/header2/header3 – заголовки выходной формы.
    // Modefield – исходные параметры для табличной формы
    // в виде модельного поля с подполями.
    // (может быть пустым, если таб.форма не требует исходных параметров)
    // Седьмой операнд – выходной носитель:
    // 0 – бумага
    // 1/FileName – файл (путь и имя файла)
    // 2/E-mail/Subject – эл.почта (адрес и тема)
    //
    // Пример:
    // PRINT 1,TABIUW,,,Заголовок 1/Заголовок 2/Заголовок 3,^A2000/101^BАБ,2/alio @gpntb.ru/Отчет
    // Примечания:
    // В серверном Администраторе команда не поддерживается
    //

    /// <summary>
    /// Вывод документов на печать.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IbfPrint
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
