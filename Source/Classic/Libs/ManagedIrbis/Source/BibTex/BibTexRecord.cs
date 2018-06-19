// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BibTexRecord.cs --
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

namespace ManagedIrbis.BibTex
{
    /// <summary>
    /// Запись.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BibTexRecord
    {
        #region Properties

        /// <summary>
        /// Поля записи.
        /// </summary>
        [NotNull]
        public FieldCollection Fields { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BibTexRecord()
        {
            Fields = new FieldCollection();
        }

        #endregion
    }
}
