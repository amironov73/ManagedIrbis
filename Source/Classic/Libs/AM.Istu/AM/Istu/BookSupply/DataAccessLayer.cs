// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataAccessLayer.cs -- 
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

using BLToolkit.Data;
using BLToolkit.Data.Linq;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Istu.BookSupply
{
    /// <summary>
    /// Слой доступа к БД.
    /// </summary>
    public class DataAccessLayer
        : DbManager
    {
        #region Properties

        /// <summary>
        /// Привязки книг.
        /// </summary>
        [NotNull]
        public Table<BookBinding> BookBindings
        {
            get { return GetTable<BookBinding>(); }
        }

        /// <summary>
        /// Факультеты.
        /// </summary>
        [NotNull]
        public Table<DepartmentInfo> Departments
        {
            get { return GetTable<DepartmentInfo>(); }
        }

        /// <summary>
        /// Привязки групп.
        /// </summary>
        [NotNull]
        public Table<GroupBinding> GroupBindings
        {
            get { return GetTable<GroupBinding>(); }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        [NotNull]
        public DepartmentInfo[] ListDepartments()
        {
            return Departments.ToArray();
        }

        #endregion


        #region Object members

        #endregion
    }
}
