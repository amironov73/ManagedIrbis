// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfGlobal.cs -- 
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
    // Глобальная корректировка.
    // http://sntnarciss.ru/irbis/spravka/wa0203050200.htm
    //
    // GLOBAL - Выполнить глобальную корректи-ровку
    //
    // Команда для пакетных заданий АРМов Администратор(серверный)
    // и Администратор-клиент ИРБИС64(версия 2013.1 и выше).
    //
    // Назначение команды:
    // Выполнить глобальную корректировку
    // Оператор:
    // GLOBAL
    // Операнды:
    //
    // Gblname,mfnfrom,mfnto,[0|1],[0|1],[0|1],FileName,query
    // Где:
    // Gblname - имя задания на глобальную корректировку
    // (в случае серверного Администратора – полный путь и имя);
    // mfnfrom, mfnto - диапазон корректируемых MFN
    // (по умолчанию 1 и maxMFN);
    // [0|1] - признак актуализации(применять/не применять);
    // [0|1] - признак ФЛК(применять/не применять);
    // [0|1] - признак автоввода(применять/не применять);
    // FileName - имя файла протокола с полным путем
    // (в Администраторе-клиент не используется);
    // query - поисковый запрос на языке ИРБИС
    // (в Администраторе-клиент не используется)
    //
    // Пример:
    // GLOBAL C:\irbiswrk\test.gbl,,,0,0,0,C:\irbiswrk\111.txt,”K=автомат$”
    //

    /// <summary>
    /// Глобальная корректировка.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IbfGlobal
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
