// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforJ.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using AM;
using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдать библиографическую свертку документа – &uf('B
    // Вид функции: B.
    // Назначение: Выдать библиографическую свертку документа.
    // Формат (передаваемая строка):
    // B
    //
    // Примеры:
    //
    // &unifor('B')
    //

    static class UniforB
    {
        #region Private members

        private static readonly char[] _GoodCharacters =
        {
            '\u0030', '\u0031', '\u0032', '\u0033', '\u0034', '\u0035',
            '\u0036', '\u0037', '\u0038', '\u0039', '\u0041', '\u0042',
            '\u0043', '\u0044', '\u0045', '\u0046', '\u0047', '\u0048',
            '\u0049', '\u004A', '\u004B', '\u004C', '\u004D', '\u004E',
            '\u004F', '\u0050', '\u0051', '\u0052', '\u0053', '\u0054',
            '\u0055', '\u0056', '\u0057', '\u0058', '\u0059', '\u005A',
            '\u0061', '\u0062', '\u0063', '\u0064', '\u0065', '\u0066',
            '\u0067', '\u0068', '\u0069', '\u006A', '\u006B', '\u006C',
            '\u006D', '\u006E', '\u006F', '\u0070', '\u0071', '\u0072',
            '\u0073', '\u0074', '\u0075', '\u0076', '\u0077', '\u0078',
            '\u0079', '\u007A', '\u0402', '\u0403', '\u201A', '\u0453',
            '\u201E', '\u2026', '\u2020', '\u2021', '\u20AC', '\u2030',
            '\u0409', '\u2039', '\u040A', '\u040C', '\u040B', '\u040F',
            '\u0452', '\u2018', '\u2019', '\u201C', '\u201D', '\u2022',
            '\u2013', '\u2014', '\u0098', '\u2122', '\u0459', '\u203A',
            '\u045A', '\u045C', '\u045B', '\u045F', '\u00A0', '\u040E',
            '\u045E', '\u0408', '\u00A4', '\u0490', '\u00A6', '\u00A7',
            '\u0401', '\u00A9', '\u0404', '\u00AB', '\u00AC', '\u00AD',
            '\u00AE', '\u0407', '\u00B0', '\u00B1', '\u0406', '\u0456',
            '\u0491', '\u00B5', '\u00B6', '\u00B7', '\u0451', '\u2116',
            '\u0454', '\u00BB', '\u0458', '\u0405', '\u0455', '\u0457',
            '\u0410', '\u0411', '\u0412', '\u0413', '\u0414', '\u0415',
            '\u0416', '\u0417', '\u0418', '\u0419', '\u041A', '\u041B',
            '\u041C', '\u041D', '\u041E', '\u041F', '\u0420', '\u0421',
            '\u0422', '\u0423', '\u0424', '\u0425', '\u0426', '\u0427',
            '\u0428', '\u0429', '\u042A', '\u042B', '\u042C', '\u042D',
            '\u042E', '\u042F', '\u0430', '\u0431', '\u0432', '\u0433',
            '\u0434', '\u0435', '\u0436', '\u0437', '\u0438', '\u0439',
            '\u043A', '\u043B', '\u043C', '\u043D', '\u043E', '\u043F',
            '\u0440', '\u0441', '\u0442', '\u0443', '\u0444', '\u0445',
            '\u0446', '\u0447', '\u0448', '\u0449', '\u044A', '\u044B',
            '\u044C', '\u044D', '\u044E', '\u044F'
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Convolution of the text.
        /// </summary>
        public static void Convolution
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            // Берётся первая буква последнего слова, затем первая
            // буква предпоследнего слова, и так до первого слова.
            // Затем вторая буква последнего слова (если есть),
            // вторая буква предпоследнего слова, и так снова до
            // первого слова. Продолжается это до тех пор, пока
            // не закончатся буквы в словах, либо не будет достигнут
            // предел в 64 символа.

            const int MaxLength = 64;

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);
            string[] words = navigator.SplitToWords(_GoodCharacters);
            if (words.Length == 0)
            {
                return;
            }

            StringBuilder output = new StringBuilder(MaxLength);

            for (int charOffset = 0; ; charOffset++)
            {
                if (output.Length == MaxLength)
                {
                    break;
                }

                bool flag = false;
                for (
                        int wordIndex = words.Length - 1;
                        wordIndex >= 0;
                        wordIndex--
                    )
                {
                    if (charOffset < words[wordIndex].Length)
                    {
                        flag = true;
                        char c = CharUtility.ToUpperInvariant
                        (
                            words[wordIndex][charOffset]
                        );
                        output.Append(c);
                        if (output.Length == MaxLength)
                        {
                            break;
                        }
                    }
                }

                if (!flag)
                {
                    break;
                }
            }

            if (output.Length != 0)
            {
                context.Write(node, output.ToString());
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
