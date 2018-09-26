// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AffixEntry.cs -- 
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
    public class AffixEntry
    {
        #region Properties

        /// <summary>
        /// The characters to add to the string.
        /// </summary>
        [CanBeNull]
        public string AddCharacters { get; set; }

        /// <summary>
        /// The condition to be met in order to add characters.
        /// </summary>
        public int[] Conditions { get; set; }

        /// <summary>
        /// The characters to remove before adding characters.
        /// </summary>
        [CanBeNull]
        public string StripCharacters { get; set; }

        /// <summary>
        /// The number of conditions that must be met
        /// </summary>
        public int ConditionCount { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AffixEntry()
        {
            AddCharacters = string.Empty;
            Conditions = new int[256];
            StripCharacters = string.Empty;
            ConditionCount = 0;
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
