// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor6.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using ManagedIrbis.Infrastructure;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class Unifor6
    {
        #region Private members

        #endregion

        #region Public methods

        public static void ExecuteNestedFormat
            (
                PftContext context,
                PftNode node,
                string fileName
            )
        {
            //
            // TODO some caching
            //

            if (!string.IsNullOrEmpty(fileName))
            {

                string ext = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(ext))
                {
                    fileName += ".pft";
                }
                FileSpecification specification
                    = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        context.Environment.Database,
                        fileName
                    );
                string source = context.Environment.ReadFile
                (
                    specification
                );
                if (string.IsNullOrEmpty(source))
                {
                    return;
                }
                PftLexer lexer = new PftLexer();
                PftTokenList tokens = lexer.Tokenize(source);
                PftParser parser = new PftParser(tokens);
                PftProgram program = parser.Parse();
                program.Execute(context);
            }
        }

        #endregion
    }
}
