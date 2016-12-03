// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftEditorControl.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Editor for PFT/FST.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftEditorControl
        : TextEditorControl
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftEditorControl()
        {
            PftSyntaxModeProvider provider
                = new PftSyntaxModeProvider();
            HighlightingManager.Manager.AddSyntaxModeFileProvider
                (
                    provider
                );
            SetHighlighting("PFT");
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
