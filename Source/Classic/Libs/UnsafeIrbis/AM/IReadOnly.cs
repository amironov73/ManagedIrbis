// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IReadOnly -- common interface for object that can be read-only.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using UnsafeAM;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM
{
    /// <summary>
    /// Common interface for object that can be read-only.
    /// </summary>
    public interface IReadOnly<T>
    {
        /// <summary>
        /// Creates the read-only clone of the object.
        /// </summary>
        [NotNull]
        T AsReadOnly();

        /// <summary>
        /// Whether the object is read-only.
        /// </summary>
        bool ReadOnly { get; }

        /// <summary>
        /// Marks the object as read-only.
        /// </summary>
        void SetReadOnly();

        /// <summary>
        /// Throws <see cref="ReadOnlyException"/>
        /// if the object is read-only.
        /// </summary>
        void ThrowIfReadOnly();
    }
}
