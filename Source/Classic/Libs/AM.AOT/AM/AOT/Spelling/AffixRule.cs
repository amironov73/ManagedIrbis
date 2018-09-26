// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AffixRule.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.AOT.Spelling
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class AffixRule
    {
        #region Properties

        /// <summary>
        /// Allow combining prefix and suffix.
        /// </summary>
        public bool AllowCombine { get; set; }

        /// <summary>
        /// Collection of text entries that make up this rule.
        /// </summary>
        public List<AffixEntry> AffixEntries { get; set; }

        /// <summary>
        /// Name of the Affix rule
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AffixRule()
        {
            AllowCombine = false;
            AffixEntries = new List<AffixEntry>();
            Name = string.Empty;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
