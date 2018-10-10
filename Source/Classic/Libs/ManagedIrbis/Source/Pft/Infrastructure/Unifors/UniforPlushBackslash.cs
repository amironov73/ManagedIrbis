// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusBackslash.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Неописанный UNIFOR, используемый при выводе поля 951
    // в Group951.pft:
    //
    // 'var imgPrefix2=",,',&uf('+\0',if g1^A<>'' then g1^A else g1^I fi),'";',/
    //
    // &uf('+\0C:\Path\File.ext')
    // выводит
    // C:\\Path\\File.ext
    //

    static class UniforPlushBackslash
    {
        #region Public methods

        public static void DoubleBackslashes
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);
            char command = navigator.ReadChar();
            string output = navigator.GetRemainingText();
            if (command == '0')
            {
                if (!string.IsNullOrEmpty(output))
                {
                    output = output.Replace(@"\", @"\\");
                }
            }

            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}