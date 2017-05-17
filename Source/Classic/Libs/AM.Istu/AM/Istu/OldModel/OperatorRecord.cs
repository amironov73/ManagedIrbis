// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OperatorRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Configuration;
using AM.Data;

using BLToolkit.Mapping;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class OperatorRecord
    {
        #region Properties

        ///<summary>
        /// Идентификатор оператора.
        ///</summary>
        [MapField("id")]
        public int ID { get; set; }

        ///<summary>
        /// ФИО.
        ///</summary>
        [MapField("fio")]
        public string Name { get; set; }

        ///<summary>
        /// Комментарий.
        ///</summary>
        [MapField("comment")]
        public string Comment { get; set; }

        ///<summary>
        /// Штрих-код.
        ///</summary>
        [MapField("barcode")]
        public string Barcode { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return Name.NullableToVisibleString();
        }

        #endregion
    }
}
