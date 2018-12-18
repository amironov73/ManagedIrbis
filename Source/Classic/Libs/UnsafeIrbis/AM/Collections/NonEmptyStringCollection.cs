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

using UnsafeAM.Logging;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Collections
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public sealed class NonEmptyStringCollection
        : Collection<string>
    {
        #region Collection<string> members

        /// <inheritdoc cref="Collection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                string item
            )
        {
            if (string.IsNullOrEmpty(item))
            {
                Log.Error
                    (
                        "NonEmptyStringCollection::InsertItem: "
                        + "item is empty"
                    );

                throw new ArgumentOutOfRangeException(nameof(item));
            }

            base.InsertItem(index, item);
        }

        /// <inheritdoc cref="Collection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                string item
            )
        {
            if (string.IsNullOrEmpty(item))
            {
                Log.Error
                    (
                        "NonEmptyStringCollection::SetItem: "
                        + "item is empty"
                    );

                throw new ArgumentOutOfRangeException(nameof(item));
            }

            base.SetItem(index, item);
        }

        #endregion
    }
}
