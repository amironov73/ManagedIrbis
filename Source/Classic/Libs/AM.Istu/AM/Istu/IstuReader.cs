// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IstuReader.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.Validation;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [TableName("readers")]
    public abstract class IstuReader
    {
        #region Properties

        /// <summary>
        /// Фамилия, имя, отчество
        /// </summary>
        [Required]
        [DisplayName("ФИО")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        [DisplayName("Категория")]
        public abstract string Category { get; set; }

        /// <summary>
        /// Факультет
        /// </summary>
        [DisplayName("Факультет")]
        //[PropertyConverter(typeof(DepartmentInfo.Converter))]
        public abstract int Department { get; set; }

        /// <summary>
        /// Кафедра
        /// </summary>
        [DisplayName("Кафедра")]
        //[PropertyConverter(typeof(DepartmentInfo.Converter))]
        public abstract int Cathedra { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        [MapField("job")]
        [DisplayName("Должность")]
        public abstract string JobTitle { get; set; }

        /// <summary>
        /// Номер читательского билета
        /// </summary>
        [Required]
        [DisplayName("Номер билета")]
        public abstract string Ticket { get; set; }

        /// <summary>
        /// Пароль доступа к системе
        /// </summary>
        [DisplayName("Пароль")]
        public abstract string Password { get; set; }

        /// <summary>
        /// Необязательный комментарий в произвольной форме
        /// </summary>
        [DisplayName("Примечания")]
        public abstract string Comment { get; set; }

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
