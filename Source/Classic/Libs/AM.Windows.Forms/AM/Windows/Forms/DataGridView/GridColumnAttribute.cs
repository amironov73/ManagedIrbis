// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GridColumnAttribute.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [AttributeUsage(AttributeTargets.Property)]
    public class GridColumnAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the type of the column.
        /// </summary>
        [CanBeNull]
        public Type ColumnType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this 
        /// <see cref="GridColumnAttribute"/> is frozen.
        /// </summary>
        [DefaultValue(false)]
        public bool Frozen { get; set; }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>The header text.</value>
        [CanBeNull]
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        [DefaultValue(-1)]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        [DefaultValue(false)]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this 
        /// <see cref="GridColumnAttribute"/> is resizeable.
        /// </summary>
        [DefaultValue(false)]
        public bool Resizeable { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="GridColumnAttribute"/> class.
        /// </summary>
        public GridColumnAttribute()
        {
            Index = -1;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GridColumnAttribute
            (
                [NotNull] string headerText
            )
            : this()
        {
            Code.NotNull(headerText, "headerText");

            HeaderText = headerText;
        }

        #endregion
    }
}
