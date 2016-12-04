// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianSubField.cs -- 
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
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Worksheet;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianSubField
    {
        #region Properties

        /// <summary>
        /// Subfield code.
        /// </summary>
        public char Code { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        [CanBeNull]
        public string Title { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        public string Value { get; set; }

        /// <summary>
        /// Original value.
        /// </summary>
        [CanBeNull]
        public string OriginalValue { get; set; }

        /// <summary>
        /// Editing mode?
        /// </summary>
        [CanBeNull]
        public string Mode { get; set; }

        /// <summary>
        /// Modified?
        /// </summary>
        public bool Modified { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Create <see cref="SiberianSubField"/> from
        /// <see cref="WorksheetItem"/>.
        /// </summary>
        [NotNull]
        public static SiberianSubField FromWorksheetItem
            (
                [NotNull] WorksheetItem item
            )
        {
            CodeJam.Code.NotNull(item, "item");

            SiberianSubField result = new SiberianSubField
            {
                Code = item.Tag[0],
                Title = item.Title
            };

            return result;
        }


        #endregion

        #region Object members

        #endregion
    }
}
