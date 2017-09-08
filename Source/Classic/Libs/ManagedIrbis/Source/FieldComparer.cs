// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldComparer.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FieldComparer
    {
        #region Nested classes

        class ByTagComparer
            : Comparer<RecordField>
        {
            /// <inheritdoc cref="Comparer{T}.Compare" />
            public override int Compare
                (
                    RecordField left,
                    RecordField right
                )
            {
                return left.ThrowIfNull("left").Tag
                    - right.ThrowIfNull("right").Tag;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compare <see cref="RecordField"/> by
        /// <see cref="RecordField.Tag"/>.
        /// </summary>
        [NotNull]
        public static Comparer<RecordField> ByTag()
        {
            return new ByTagComparer();
        }

        #endregion
    }
}
