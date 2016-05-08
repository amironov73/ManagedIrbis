/* IrbisAlphabetTable.cs -- ISISAC.TAB
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public IrbisAlphabetTable
            (
                Encoding encoding, 
                byte[] table
            )
        {
            _encoding = encoding;
            _table = table;
        }

        public IrbisAlphabetTable
            (
                ManagedClient64 client,
                string fileName
            )
        {
            
        }

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
