// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfExportDb.cs -- 
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
    // http://sntnarciss.ru/irbis/spravka/wa0203050150.htm
    //
    // Операнды:
    //
    // 0|1/mfnfrom/mfnto],FstName,[0/#/@|1|2],[0|1|2],FileName
    // Где:
    // Первый операнд - исходные документы:
    // 0 - все
    // 1 - диапазон документов,
    // mfnfrom - начальный MFN,
    // mfnto - конечный MFN.
    // FstName - имя ТВП переформатирования,
    // если пустое значение - переформатирование не используется.
    // Третий операнд - выходной формат данных:
    // 0 - ISO-формат,
    // # - символ-разделитель полей,
    // @ - символ-разделитель записей;
    // 1 - текстовый формат
    // 2 - XML
    // Четвертый операнд - вид кодировки:
    // 0 - DOS
    // 1 – Windows
    // 2 – UTF8
    // FileName - полное имя файла с выходными данными
    //
    // Пример:
    // ExportDB 1/21/500, UMARCEW,0,1, c:\temp\22.iso
    //

    /// <summary>
    /// Экспорт записей из базы данных.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IbfExportDb
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
