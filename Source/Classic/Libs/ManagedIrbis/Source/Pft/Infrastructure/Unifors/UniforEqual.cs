// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforEqual.cs --
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
    // Форматный выход: Сравнить заданное значение с маской
    // (сравнение по маске)
    // &uf('=!<маска>!<значение>')
    // ! – уникальный символ двухстороннего ограничения
    // (может быть любым символом).
    // Маска может содержать принятые символы маскирования * и ?.
    // В общем случае маска может содержать несколько масок,
    // отделенных друг от друга символом вертикальной черты (|).
    // Форматный выход возвращает: 1 – в случае положительного
    // результата сравнения; 0 – в случае отрицательного.
    //
    // Examples
    //
    // ----------------------------------
    // | Mask      | Value     | Result |
    // | ----------|-----------|--------|
    // | *         |           | 1      |
    // | ?         |           | 0      |
    // | Hello     | Hello     | 1      |
    // | Hello     | hello     | 0      |
    // | Hello*    | Hello     | 1      |
    // | Hello?    | Hello     | 0      |
    // | Hel*      | Hello     | 1      |
    // | He??o     | Hello     | 1      |
    // | He??o     | Hell      | 0      |
    // ----------------------------------
    //

    static class UniforEqual
    {
        #region Public methods

        public static void CompareWithMask
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
            char delimiter = navigator.ReadChar();
            if (delimiter == TextNavigator.EOF)
            {
                return;
            }

            string maskSpecifiaction = navigator.ReadUntil(delimiter);
            if (ReferenceEquals(maskSpecifiaction, null)
                || navigator.ReadChar() == TextNavigator.EOF)
            {
                return;
            }

            string text = navigator.GetRemainingText()
                ?? string.Empty;

            PftMask mask = new PftMask(maskSpecifiaction);
            bool result = mask.Match(text);
            string output = result ? "1" : "0";
            context.Write(node, output);
            context.OutputFlag = true;
        }

        #endregion
    }
}
