// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblCorrec.cs -- 
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
    // http://sntnarciss.ru/irbis/spravka/pril00704070000.htm
    //
    // Корректировка других записей
    //
    // Оператор, выполняясь на текущей записи,
    // вызывает на корректировку другие записи,
    // отобранные по поисковым терминам из текущей или другой,
    // доступной в системе, базы данных.
    //
    // За этим оператором должна следовать группа операторов,
    // до завершающего оператора END, которые и будут выполнять
    // корректировку. В файле задания оператор описывается
    // четырьмя строками, которые содержат следующие данные:
    //
    // 1. Имя оператора – CORREC
    // 2. Формат – результатом форматирования текущей записи
    // должна быть текстовая строка, задающая имя той базы данных,
    // в которой следует отобрать записи для пакетной корректировки.
    // Если строка – ‘*’, то этой базой данных останется текущая.
    // 3. Формат – результатом форматирования текущей записи должна
    // быть строка, которая передается в корректируемые записи 
    // в виде «модельного» поля с меткой 1001. Т.е.это способ
    // передачи данных от текущей записи в корректируемые.
    // Следует не забывать в последнем операторе группы удалять
    // поле 1001.
    // 4. Формат – результатом форматирования текущей записи
    // должны быть строки, которые будут рассматриваться как
    // термины словаря другой (или той же) базы данных.
    // Записи, связанные с этими терминами, будут далее
    // корректироваться. Если последним символом термина будет символ
    // ‘$’ (усечение), то отбор записей на корректировку будет
    // аналогичен проведению в другой базе данных поиска ‘термин$’
    // 5. Можно задать пятую строку, в которой указывается количество
    // корректируемых записей, если надо корректировать
    // не все отобранные записи.
    //

    /// <summary>
    /// Из текущей записи, вызывает на корректировку другие записи,
    /// отобранные по поисковым терминам из текущей или другой,
    /// доступной в системе, базы данных.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblCorrec
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "CORREC";

        #endregion

        #region Properties

        /// <summary>
        /// Children nodes.
        /// </summary>
        [NotNull]
        public GblNodeCollection Children { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblCorrec()
        {
            Children = new GblNodeCollection(this);
        }

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
