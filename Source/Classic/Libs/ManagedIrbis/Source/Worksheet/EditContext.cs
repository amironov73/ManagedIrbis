// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EditContext.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Worksheet
{
    /// <summary>
    /// Context for field/subfield editor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class EditContext
    {
        #region Properties

        /// <summary>
        /// Record to be edited.
        /// </summary>
        [CanBeNull]
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Tag of the field.
        /// </summary>
        [CanBeNull]
        public string Tag { get; set; }

        /// <summary>
        /// Code of the subfield (if present).
        /// </summary>
        [CanBeNull]
        public string Code { get; set; }

        /// <summary>
        /// Repeat (0 = all/unknown).
        /// </summary>
        public int Repeat { get; set; }

        /// <summary>
        /// Worksheet item.
        /// </summary>
        [CanBeNull]
        public WorksheetItem Item { get; set; }

        /// <summary>
        /// Source lines (at least one).
        /// </summary>
        [CanBeNull]
        public string[] Source { get; set; }

        /// <summary>
        /// Result lines (at least one).
        /// </summary>
        [CanBeNull]
        public string[] Result { get; set; }

        /// <summary>
        /// Whether user accepted result.
        /// </summary>
        public bool Accept { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        public object UserData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
