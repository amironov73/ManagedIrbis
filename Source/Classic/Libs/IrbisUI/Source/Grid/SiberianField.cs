/* SiberianField.cs -- 
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
    public class SiberianField
    {
        #region Properties

        /// <summary>
        /// Field tag.
        /// </summary>
        [CanBeNull]
        public string Tag { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        [CanBeNull]
        public string Title { get; set; }

        /// <summary>
        /// Repeat.
        /// </summary>
        public int Repeat { get; set; }

        /// <summary>
        /// Parent (first) field.
        /// </summary>
        [CanBeNull]
        public SiberianField Parent { get; set; }

        /// <summary>
        /// Whether the field is repeatable?
        /// </summary>
        public bool Repeatable { get; set; }

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
        /// Create <see cref="SiberianField"/> from
        /// <see cref="WorksheetItem"/>.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public static SiberianField FromWorksheetItem
            (
                [NotNull] WorksheetItem item
            )
        {
            Code.NotNull(item, "item");

            SiberianField result = new SiberianField
            {
                Tag = item.Tag,
                Title = item.Title,
                Repeatable = item.Repeatable
            };

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
