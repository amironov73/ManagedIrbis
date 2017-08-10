// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfImportDb.cs -- 
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
    // http://sntnarciss.ru/irbis/spravka/wa0203050140.htm
    //
    //    Операнды:
    //
    // [0/#/@|1],FstName,[0|1],[0|1/2],FileName,[0|1], [0|1], [0|1], PftGblName
    // Где:
    // Первый операнд - исходный формат данных:
    // 0 - ISO-формат,
    // # - символ-разделитель полей,
    // @ - символ-разделитель записей;
    // 1 - текстовый формат.
    // FstName - имя ТВП переформатирования,
    // если пустое значение - переформатирование не используется.
    // Третий операнд - признак ФЛК:
    // 0 - не применять;
    // 1 - применять.
    // Четвертый операнд - вид кодировки:
    // 0 - DOS
    // 1 – Windows
    // 2 – UTF8
    // Пятый операнд - FileName - полное имя файла с исходными данными
    // Шестой операнд - признак Автоввода
    // (в Администраторе-клиенте не используется):
    // 0 - не применять;
    // 1 - применять.
    // Седьмой операнд – признак формирования протокола импорта:
    // 0 – не формировать
    // 1 – формировать
    // Восьмой операнд – признак слияния:
    // 0 – на основе ключевого формата
    // 1 – на основе глобального задания
    // (пустое значение) – слияние не применяется
    // PftGblName – имя формата или глобального задания для слияния.
    //
    // Пример:
    // ImportDB 0,,0,1, c:\temp\11.iso,0,0,1, test
    // Примечания:
    // В клиентском "Администраторе" не поддерживается
    // шестой операнд - признак АВТОВВОДА, т. е. автоввод выполняется
    // в соответствии с параметром AutoinFile в INI-файле клиента (irbisa.ini)
    //

    /// <summary>
    /// Импорт записей в базу данных.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IbfImportDb
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
