// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusS.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text.RegularExpressions;

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выводит нужную часть текста (до знака "=", или после него)
    // в заголовках, описанных по принципу <NNN=KKK> – &uf('+S')
    // Вид функции: +S.
    // Назначение: Выводит нужную часть текста (до знака "=",
    // или после него) в заголовках, описанных по принципу <NNN=KKK>.
    // Формат (передаваемая строка):
    // +SN[строка]
    // где N может принимать значения 0 для вывода текста после
    // знака "=", и 1 для вывода текста перед знаком "=".
    // Выдержка из документации (см.Файл описания релиза 2002.2
    // и документацию на http://nucleonics.fatal.ru):
    //
    // Для книг, описанных «Под заглавием», которые начинаются
    // с числительного, обозначенного цифрами, начальный элемент
    // сортировки и авторский знак должны формироваться
    // по словесному обозначению числительного. В связи с этим
    // Пользователь должен ввести дополнительную разметку типа
    // <NNN=Текст>, где «Текст», стоящий после знака «=»,
    // — это словесное значение числительного NNN, используемое
    // взамен него для формирования авторского знака и/или
    // в качестве начального элемента сортировки. Например,
    // заглавие 1000 и одна ночь, размеченное как
    // <1000=тысяча> и одна ночь, для формирования авторского знака
    // и сортировки будет представлено как «тысяча и одна ночь»,
    // а для печати как «1000 и одна ночь».
    //
    // Для биографических и биобиблиографических изданий,
    // описанных под заглавием, начинающимся с имени лица,
    // которому оно посвящено, авторский знак и начальный элемент
    // сортировки должны формироваться на фамилию. В связи
    // с этим Пользователь также должен ввести дополнительную
    // разметку типа<AAA=Текст>, где «Текст», стоящий после
    // знака «=», — это фамилия, используемая (взамен части ААА)
    // для формирования авторского знака и/или начального элемента
    // сортировки.Например, заглавие Антон Павлович Чехов может
    // быть размечено как <Антон Павлович Чехов=Чехов Антон Павлович>
    // (в сортировку пойдет «Чехов Антон Павлович»,
    // на печать «Антон Павлович Чехов»). Заметим, что разметка
    // типа<Антон=Чехов> Павлович Чехов даст правильный авторский
    // знак (Чехов Павлович Чехов), но может дать ошибки в сортировке.
    //
    // Примеры:
    //
    // Пример входной строки:
    // <1=Первое> апреля
    // Примеры использования функции:
    // &uf('+s0'v200^a)
    // &uf('+s1'v200^a)
    //
    // Пример расформатирования:
    // Первое апреля
    // 1 апреля
    //

    static class UniforPlusS
    {
        #region Private members

        private static string _FirstEvaluator
            (
                [NotNull] Match match
            )
        {
            return match.Groups["first"].Value;
        }

        private static string _SecondEvaluator
            (
                [NotNull] Match match
            )
        {
            return match.Groups["second"].Value;
        }

        #endregion

        #region Public methods


        /// <summary>
        /// Decode title.
        /// </summary>
        public static void DecodeTitle
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                char index = navigator.ReadChar();
                string input = navigator.GetRemainingText();
                if (!string.IsNullOrEmpty(input))
                {
                    MatchEvaluator evaluator = _SecondEvaluator;
                    if (index != '0')
                    {
                        evaluator = _FirstEvaluator;
                    }
                    string output = Regex.Replace
                        (
                            input,
                            "[<](?<first>.+?)(?:[=](?<second>.+?))?[>]",
                            evaluator
                        );
                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        #endregion
    }
}
