// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NonEmptyStringCollection.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.ObjectModel;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class NonEmptyStringCollection
        : Collection<string>
    {
        #region Collection<string> members

        /// <inheritdoc />
        protected override void InsertItem
            (
                int index,
                string item
            )
        {
            if (string.IsNullOrEmpty(item))
            {
                throw new ArgumentOutOfRangeException("item");
            }

            base.InsertItem(index, item);
        }

        /// <inheritdoc />
        protected override void SetItem
            (
                int index,
                string item
            )
        {
            if (string.IsNullOrEmpty(item))
            {
                throw new ArgumentOutOfRangeException("item");
            }

            base.SetItem(index, item);
        }

        #endregion
    }
}
