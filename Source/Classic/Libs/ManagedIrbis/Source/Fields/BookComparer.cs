// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BookComparer.cs -- compare books
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
    /// Compare <see cref="BookInfo"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class BookComparer
    {
        #region Nested classes

        class AuthorAndTitleComparer
            : IComparer<BookInfo>
        {
            #region IComparer<T> members

                public int Compare
                    (
                        BookInfo x,
                        BookInfo y
                    )
            {
                Code.NotNull(x, "x");
                Code.NotNull(y, "y");

                AuthorInfo authorX = x.FirstAuthor;
                AuthorInfo authorY = y.FirstAuthor;
                if (ReferenceEquals(authorX, null))
                {
                    if (!ReferenceEquals(authorY, null))
                    {
                        return -1;
                    }
                }
                else
                {
                    if (ReferenceEquals(authorY, null))
                    {
                        return 1;
                    }

                    int result = Author.Compare(authorX, authorY);
                    if (result != 0)
                    {
                        return result;
                    }
                }

                return Title.Compare(x.Title, y.Title);
            }

            #endregion
        }


        #endregion

        #region Properties

        /// <summary>
        /// Compare <see cref="AuthorInfo"/>.
        /// </summary>
        [NotNull]
        public static IComparer<AuthorInfo> Author { get; set; }

        /// <summary>
        /// Compare <see cref="TitleInfo"/>.
        /// </summary>
        [NotNull]
        public static IComparer<TitleInfo> Title { get; set; }

        #endregion

        #region Construction

        static BookComparer()
        {
            Author = AuthorComparer.ByFullName();
            Title = TitleComparer.FullTitle();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compare <see cref="BookInfo"/>
        /// by the authors and titles.
        /// </summary>
        [NotNull]
        public static IComparer<BookInfo> ByAuthorAndTitle()
        {
            return new AuthorAndTitleComparer();
        }

        #endregion
    }
}
