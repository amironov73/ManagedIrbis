// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IMarcEditor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Worksheet
{
    /// <summary>
    /// Generic editor for field/subfield.
    /// </summary>
    public interface IMarcEditor
    {
        /// <summary>
        /// Perform edit action using specified context.
        /// </summary>
        void PerformEdit
            (
                [NotNull] EditContext context
            );
    }
}
