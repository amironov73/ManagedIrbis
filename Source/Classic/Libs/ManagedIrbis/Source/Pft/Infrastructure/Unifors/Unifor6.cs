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

        /// <summary>
        /// Execute format from file.
        /// </summary>
        public static void ExecuteFormat
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            //
            // TODO some caching
            //

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            string fileName = expression;
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

        #endregion
    }
}
