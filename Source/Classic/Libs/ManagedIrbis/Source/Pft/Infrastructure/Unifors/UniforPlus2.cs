/* UniforPlus2.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforPlus2
    {
        #region Private members

        #endregion

        #region Public methods

        public static void System
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
#if CLASSIC

            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split(new[] {' ', '\t'}, 2);
                string fileName = parts[0];
                string arguments = (parts.Length == 2)
                    ? parts[1]
                    : string.Empty;

                ProcessStartInfo startInfo = new ProcessStartInfo
                    (
                        fileName,
                        arguments
                    )
                {
                    CreateNoWindow = false,
                    UseShellExecute = false
                };

                Process process = Process.Start(startInfo);
                if (!ReferenceEquals(process, null))
                {
                    process.Dispose();
                }
            }

#endif
        }

        #endregion
    }
}
