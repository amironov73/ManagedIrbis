// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ColumnWidthAttribute.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Data
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnWidthAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Column width.
        /// </summary>
        public int Width { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ColumnWidthAttribute
            (
                int width
            )
        {
            Width = width;
        }

        #endregion
    }
}
