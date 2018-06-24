// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EndNoteRecord.cs --
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

namespace ManagedIrbis.Metadata.EndNote
{
    //
    // https://en.wikipedia.org/wiki/EndNote
    //
    // EndNote is a commercial reference management software package,
    // used to manage bibliographies and references when writing essays
    // and articles. It is produced by Clarivate Analytics (previously
    // by Thomson Reuters).
    //

    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class EndNoteRecord
    {
        #region Properties

        /// <summary>
        /// Fields.
        /// </summary>
        [NotNull]
        public FieldCollection Fields { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public EndNoteRecord()
        {
            Fields = new FieldCollection();
        }

        #endregion
    }
}
