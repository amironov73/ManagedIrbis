// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RisField.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Metadata.Ris
{
    /// <summary>
    /// RIS record field.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RisField
    {
        #region Properties

        /// <summary>
        /// Tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}
