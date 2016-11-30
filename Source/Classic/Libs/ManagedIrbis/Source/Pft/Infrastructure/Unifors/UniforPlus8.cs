// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus8.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Text;

using ManagedIrbis.PlatformSpecific;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforPlus8
    {
        #region Private members

        #endregion

        #region Public methods

        public static void ExecuteNativeMethod
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);
            bool winApi = false;
            if (navigator.PeekChar() == '*')
            {
                winApi = true;
                navigator.ReadChar();
            }
            string dllName = navigator.ReadUntil(',');
            if (string.IsNullOrEmpty(dllName))
            {
                return;
            }
            navigator.ReadChar(); // eat the comma
            string methodName = navigator.ReadUntil(',');
            if (string.IsNullOrEmpty(methodName))
            {
                return;
            }
            navigator.ReadChar(); // eat the comma
            string input = navigator.GetRemainingText()
                ?? string.Empty;

            MethodResult result = MethodRunner.RunMethod
            (
                dllName,
                methodName,
                winApi,
                input
            );

            // TODO: do we need to check return code?
            if (result.ReturnCode == 0)
            {
                context.Write
                (
                    node,
                    result.Output
                );
            }
        }

        #endregion
    }
}
