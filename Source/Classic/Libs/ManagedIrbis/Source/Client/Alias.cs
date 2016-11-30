// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Alias.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    sealed class Alias
    {
        #region Properties

        /// <summary>
        /// Name of the alias.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Value of the alias.
        /// </summary>
        [CanBeNull]
        public string Value { get; set; }

        #endregion
    }
}
