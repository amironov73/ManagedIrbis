// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FulltextDatabase.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Direct;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Fulltext
{
    /// <summary>
    /// Встроенная база данных TEXT.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class FulltextDatabase
        : IDisposable
    {
        #region Constants

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public const string DatabaseName = "TEXT";

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FulltextDatabase
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            _direct = new DirectAccess64(fileName, DirectAccessMode.ReadOnly);
        }

        #endregion

        #region Private members

        private readonly DirectAccess64 _direct;

        #endregion

        #region Public methods

        /// <summary>
        /// Get pages for main record.
        /// </summary>
        [NotNull]
        public FulltextRecord[] GetPages
            (
                [NotNull] string mainGuid
            )
        {
            Code.NotNullNorEmpty(mainGuid, "mainGuid");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Get specified page for main record.
        /// </summary>
        [CanBeNull]
        public FulltextRecord GetPage
            (
                [NotNull] string mainGuid,
                int pageNumber
            )
        {
            Code.NotNullNorEmpty(mainGuid, "mainGuid");
            Code.Positive(pageNumber, "pageNumber");

            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _direct.Dispose();
        }

        #endregion
    }
}
