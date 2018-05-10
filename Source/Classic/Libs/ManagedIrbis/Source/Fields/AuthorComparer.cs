// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AuthorComparer.cs -- compare authors
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable PossibleNullReferenceException

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Compare authors.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class AuthorComparer
    {
        #region Nested classes

        class FullNameComparer
            : IComparer<AuthorInfo>
        {
            #region IComparer<T> members

            public int Compare
                (
                    AuthorInfo x,
                    AuthorInfo y
                )
            {
                Code.NotNull(x, "x");
                Code.NotNull(y, "y");

                return string.CompareOrdinal
                    (
                        x.FullName,
                        y.FullName
                    );
            }

            #endregion
        }

        class FamilyNameComparer
            : IComparer<AuthorInfo>
        {
            #region IComparer<T> members

            public int Compare
                (
                    AuthorInfo x,
                    AuthorInfo y
                )
            {
                Code.NotNull(x, "x");
                Code.NotNull(y, "y");

                return string.CompareOrdinal
                    (
                        x.FamilyName,
                        y.FamilyName
                    );
            }

            #endregion
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compare <see cref="AuthorInfo"/>
        /// by <see cref="AuthorInfo.FamilyName"/> field.
        /// </summary>
        [NotNull]
        public static IComparer<AuthorInfo> ByFamilyName()
        {
            return new FamilyNameComparer();
        }

        /// <summary>
        /// Compare <see cref="AuthorInfo"/>
        /// by <see cref="AuthorInfo.FullName"/> field.
        /// </summary>
        [NotNull]
        public static IComparer<AuthorInfo> ByFullName()
        {
            return new FullNameComparer();
        }

        #endregion
    }
}
