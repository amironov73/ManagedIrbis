/* TokenKind.cs -- тип токена
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

namespace AM.Text.Tokenizer
{
    /// <summary>
    /// Тип токена.
    /// </summary>
    public enum TokenKind
    {
        /// <summary>
        /// Непонятно что.
        /// </summary>
        Unknown,

        /// <summary>
        /// Слово, например, идентификатор.
        /// </summary>
        Word,

        /// <summary>
        /// Число, в том числе с плавающей точкой.
        /// </summary>
        Number,

        /// <summary>
        /// Строка в одинарных или двойных кавычках.
        /// </summary>
        QuotedString,

        /// <summary>
        /// Пробелы.
        /// </summary>
        Whitespace,

        /// <summary>
        /// Символы вроде +, /, = и т. п.
        /// </summary>
        Symbol,

        /// <summary>
        /// Перевод строки.
        /// </summary>
        EOL,

        /// <summary>
        /// Конец текста.
        /// </summary>
        EOF
    }
}
