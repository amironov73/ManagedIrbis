// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GroupBinding.cs -- 
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

using BLToolkit.DataAccess;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Istu.BookSupply
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [TableName("group_binding")]
    public class GroupBinding
        : ObjectWithID
    {
        #region Properties

        /// <summary>
        /// Идентификатор учебной дисциплины
        /// </summary>
        public int Discipline { get; set; }

        /// <summary>
        /// Шифр группы
        /// </summary>
        //[MapField ( "code" )]
        public string Group { get; set; }

        /// <summary>
        /// Семестры (битовые флаги)
        /// </summary>
        public Semester Semester { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
