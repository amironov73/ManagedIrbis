// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TitleComparer.cs -- compare titles
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
    /// Compare titles.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class TitleComparer
    {
        #region Nested classes

        class FullTitleComparer
            : IComparer<TitleInfo>
        {
            #region IComparer<T> members

            public int Compare
                (
                    TitleInfo x,
                    TitleInfo y
                )
            {
                Code.NotNull(x, "x");
                Code.NotNull(y, "y");

                return string.CompareOrdinal
                    (
                        x.FullTitle,
                        y.FullTitle
                    );
            }

            #endregion
        }

        class TitleOnlyComparer
            : IComparer<TitleInfo>
        {
            #region IComparer<T> members

            public int Compare
                (
                    TitleInfo x,
                    TitleInfo y
                )
            {
                Code.NotNull(x, "x");
                Code.NotNull(y, "y");

                return string.CompareOrdinal
                    (
                        x.Title,
                        y.Title
                    );
            }

            #endregion
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compare <see cref="TitleInfo"/>
        /// by <see cref="TitleInfo.FullTitle"/> field.
        /// </summary>
        [NotNull]
        public static IComparer<TitleInfo> FullTitle()
        {
            return new FullTitleComparer();
        }

        /// <summary>
        /// Compare <see cref="TitleInfo"/>
        /// by <see cref="TitleInfo.Title"/> field.
        /// </summary>
        [NotNull]
        public static IComparer<TitleInfo> TitleOnly()
        {
            return new TitleOnlyComparer();
        }

        #endregion
    }
}
