// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DisciplineInfo.cs -- 
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

namespace AM.Istu.BookSupply
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class DisciplineInfo
    {
        #region Properties

        /// <summary>
        /// Компонент (федеральный и т. д.)
        /// </summary>
        public int Component { get; set; }

        /// <summary>
        /// Цикл
        /// </summary>
        public int Cycle { get; set; }

        /// <summary>
        /// Код направления
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Вид обучения
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        /// Форма обучения
        /// </summary>
        public int Form { get; set; }

        /// <summary>
        /// Назначение числа студентов
        /// </summary>
        // float?
        public int Students { get; set; }

        /// <summary>
        /// Специальность
        /// </summary>
        public string Speciality { get; set; }

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
