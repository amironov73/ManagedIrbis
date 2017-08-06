// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblPutLog.cs -- 
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
    // Official documentation
    // http://sntnarciss.ru/irbis/spravka/irbishelp.html?pril00704090100.htm
    //
    // Формирование пользовательского протокола.
    //
    // Используется формат, подаваемый в колонке «Параметр1/Поле-подполе».
    // Результат форматирования дает очередную строку протокола.
    // В интерфейс добавлена новая опция выбора – «В протокол сообщения задания».
    // Если она отмечена, то в протокол будут попадать ТОЛЬКО строки,
    // формируемые данным оператором, сообщения от сервера будут подавляться.
    // При просмотре протокола добавлена возможность сохранить его в виде файла,
    // даже если эта опция не была заранее отмечена.
    //

    /// <summary>
    /// Формирование пользовательского протокола.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblPutLog
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "PUTLOG";

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
