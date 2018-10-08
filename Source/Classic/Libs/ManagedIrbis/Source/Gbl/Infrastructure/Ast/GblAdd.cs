// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblAdd.cs --
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
    // http://sntnarciss.ru/irbis/spravka/pril00704010000.htm
    //
    // Добавление нового повторения поля или подполя
    // в заданное существующее поле.
    //
    // При этом выполняются следующие правила:
    // * Если задана МЕТКА ПОЛЯ и не задано подполе, то:
    // * столбец повторения поля блокируется как не имеющий смысла,
    // соответствующая строка в файле задания заполняется
    // символом-заполнителем;
    // * все строки, сформированные ФОРМАТОМ 1,
    // записываются как новые повторения поля.
    // * Если заданы МЕТКА ПОЛЯ с обозначением подполя,
    // то первая строка, которая формируется ФОРМАТОМ 1,
    // записывается как подполе в заданное повторение поля.
    // * Если заданного повторения нет в записи,
    // то формируется повое повторение метки с заданным подполем.
    // * Если ПОВТОРЕНИЕ задано признаком F, то:
    // * ФОРМАТ 1 формирует строки добавляемых данных
    // * номер строки определяет номер того повторения,
    // в которое будет добавлено заданное подполе с данными строки.
    // * Если повторений поля в записи меньше,
    // чем сформатированных строк, то лишние строки
    // не используются, если повторений больше,
    // чем строк, то лишние повторения не корректируются.
    //
    // Во всех случаях ФОРМАТ 2 не используется
    // и соответствующие строки в файле задания заполняются
    // символом-заполнителем.
    //
    // Оператор не позволяет приписывать данные в конец поля/подполя.
    // Для этого можно воспользоваться оператором CHA.
    //

    /// <summary>
    /// Добавление нового повторения поля или подполя
    /// в заданное существующее поле.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblAdd
        : GblFieldCommand
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "ADD";

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

        /// <inheritdoc cref="GblNode.Execute" />
        public override void Execute
            (
                GblContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeExecution(context);

            MarcRecord record = context.CurrentRecord.ThrowIfNull();
            string value = FormatRecord(context, Format1);
            if (!string.IsNullOrEmpty(value))
            {
                Log.Trace("GBL::ADD: " + value);


                if (SubfieldCode == SubField.NoCode)
                {
                    record.AddField(FieldTag, value);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

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
