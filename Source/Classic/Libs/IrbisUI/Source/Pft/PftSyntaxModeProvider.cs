
/* PftEditorControl.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

using ICSharpCode.TextEditor.Document;

#endregion

namespace IrbisUI
{
    class PftSyntaxModeProvider
        : ISyntaxModeFileProvider
    {
        #region Construction

        public PftSyntaxModeProvider()
        {
            _syntaxModes = new List<SyntaxMode>();

            SyntaxMode pftMode = new SyntaxMode
                (
                    "PftMode.xshd",
                    "PFT",
                    ".pft;.fst"
                );

            _syntaxModes.Add(pftMode);
        }

        #endregion

        #region Private members

        private readonly List<SyntaxMode> _syntaxModes;

        private Stream _GetStream()
        {
            Assembly assembly = typeof(PftEditorControl).Assembly;
            Stream result = assembly.GetManifestResourceStream
                (
                    "IrbisUI.Properties.PftMode.xshd"
                );

            return result;
        }

        #endregion

        #region ISyntaxModeFileProvider members

        public XmlTextReader GetSyntaxModeFile
            (
                SyntaxMode syntaxMode
            )
        {
            Stream stream = _GetStream();
            XmlTextReader result = new XmlTextReader(stream);

            return result;
        }

        public void UpdateSyntaxModeList()
        {
            // Resources don't change during runtime
        }

        public ICollection<SyntaxMode> SyntaxModes
        {
            get { return _syntaxModes; }
        }

        #endregion
    }
}
