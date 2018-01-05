// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusZ.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Text;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak неописанный unifor('+Z')
    // Преобразование символов и кодовой страницы ANSI в OEM и обратно
    //

    class UniforPlusZ
    {
        public static void AnsiToOem
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            // ibatrak
            // если первый символ 0 вызывает CharToOemA,
            // если первый символ 1 вызывает OemToCharA

            if (!ReferenceEquals(expression, null)
                && expression.Length > 1)
            {
                char command = expression[0];
                string text = expression.Substring(1);

                switch (command)
                {
                    case '0':
                        text = EncodingUtility.ChangeEncoding
                            (
                                text,
                                IrbisEncoding.Ansi,
                                IrbisEncoding.Oem
                            );
                        break;

                    case '1':
                        text = EncodingUtility.ChangeEncoding
                            (
                                text,
                                IrbisEncoding.Oem,
                                IrbisEncoding.Ansi
                            );
                        break;
                }

                context.Write(node, text);
                context.OutputFlag = true;
            }
        }
    }
}
