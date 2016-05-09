/* IrbisAlphabetTable.cs -- ISISAC.TAB
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// ISISAC.TAB
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class IrbisAlphabetTable
    {
        #region Properties

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisAlphabetTable
            (
                [NotNull] Encoding encoding, 
                [NotNull] byte[] table
            )
        {
            _encoding = encoding;
            _table = table;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisAlphabetTable
            (
                [NotNull] ManagedClient64 client,
                [NotNull] string fileName
            )
        {
            Code.NotNull(client, "client");
            Code.NotNullNorEmpty(fileName, "fileName");

            // TODO
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisAlphabetTable
            (
                ManagedClient64 client
            )
            : this ( client, "ISISAC.TAB" )
        {
            
        }

        #endregion

        #region Private members

        private readonly Encoding _encoding;

        private readonly byte[] _table;

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the specified c is alpha.
        /// </summary>
        public bool IsAlpha
            (
                char c
            )
        {
            return false;
        }

        #endregion
    }
}
