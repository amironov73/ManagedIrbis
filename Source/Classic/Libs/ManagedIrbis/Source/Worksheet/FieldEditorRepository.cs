// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldEditorRepository.cs -- 
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
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Worksheet
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class FieldEditorRepository
    {
        #region Properties

        /// <summary>
        /// Dictionary.
        /// </summary>
        [NotNull]
        public Dictionary<EditMode, IMarcEditor> Dictionary { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldEditorRepository()
        {
            Dictionary = new Dictionary<EditMode, IMarcEditor>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Add editor for specified mode.
        /// </summary>
        public void AddEditor
            (
                EditMode mode,
                [NotNull] IMarcEditor editor
            )
        {
            Code.NotNull(editor, "editor");

            Dictionary.Add(mode, editor);
        }

        /// <summary>
        /// Get editor for specified mode.
        /// </summary>
        [CanBeNull]
        public IMarcEditor GetEditor
            (
                EditMode mode
            )
        {
            IMarcEditor result;

            Dictionary.TryGetValue(mode, out result);

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
