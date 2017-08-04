// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IndexDictionary.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    internal sealed class IndexDictionary
        : Dictionary<string, IndexInfo>
    {
        #region Properties

        public int LastId { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public IndexInfo Create
            (
                IndexSpecification index
            )
        {
            string text = index.ToText();
            IndexInfo result = new IndexInfo
                (
                    index,
                    ++LastId
                );
            Add(text, result);

            return result;
        }

        [CanBeNull]
        public IndexInfo Get
            (
                IndexSpecification specification    
            )
        {
            string text = specification.ToText();
            IndexInfo result;
            TryGetValue(text, out result);

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
