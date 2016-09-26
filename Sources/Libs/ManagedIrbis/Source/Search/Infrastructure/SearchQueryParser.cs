/* SearchQueryParser.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    //
    // See official documentation at
    // http://wiki.elnit.org/index.php/%D0%AF%D0%B7%D1%8B%D0%BA_%D0%B7%D0%B0%D0%BF%D1%80%D0%BE%D1%81%D0%BE%D0%B2_%D0%98%D0%A0%D0%91%D0%98%D0%A1
    //
    // Запрос для прямого поиска представляет собой
    // алгебраическое (поисковое) выражение, в котором
    // операндами являются термины словаря, а операторами
    // – логические операторы булевой алгебры. Для изменения
    // порядка выполнения логических операторов в поисковом
    // выражении могут применяться скобки.
    //
    // Термин словаря включает в себя собственно термин словаря
    // и префикс, если таковой используется для данного вида терминов.
    //
    // В общем виде операнд поискового выражения можно
    // представить следующим образом:
    //
    // "<префикс><термин>$"/(tag1, tag2,...tagN)
    //
    // где:
    // <префикс> - префикс, определяющий вид термина(вид словаря).
    // <термин> - собственно термин словаря.
    // $ - признак правого усечения термина.Определяет
    // совокупность терминов, имеющих начальную
    // последовательность символов, совпадающую с указанным
    // термином. Может отсутствовать, в этом случае поиск
    // идет по точному значению указанного термина.
    // " – символ-ограничитель термина (двойные кавычки).
    // Должен использоваться обязательно, если термин включает
    // в себя символы пробел, круглые скобки, решетка (#),
    // а также символы, совпадающие с обозначениями логических
    // операторов (см. ниже).
    // /(tag1, tag2,...tagN) – конструкция квалификации
    // термина.Определяет метки поля, в которых должен
    // находиться указанный термин, или точнее – вторую
    // часть индексной ссылки.Может отсутствовать, что
    // означает отсутствие дополнительных требований
    // в части меток полей.
    //
    // В поисковом выражении могут использоваться следующие
    // логические операторы:
    //
    // + - оператор логического ИЛИ. Соединение двух
    // операндов (терминов) логическим оператором ИЛИ
    // обозначает требование поиска записей, в которых
    // присутствует хотя бы один из терминов.
    // * - оператор логического И.Соединение двух терминов
    // логическим оператором И обозначает требование поиска
    // записей, в которых присутствуют оба термина.
    // ^ - оператор логического НЕ.Соединение двух терминов
    // логическим оператором НЕ обозначает требование поиска
    // записей, в которых присутствует первый термин
    // и отсутствует второй. Оператор НЕ не может быть
    // одноместным (т.е.данному оператору, как и всем другим,
    // должен ОБЯЗАТЕЛЬНО предшествовать термин).
    // (G) – первый оператор контекстного И. Соединение
    // двух терминов таким оператором контекстного И
    // обозначает требование поиска записей, в которых оба
    // термина присутствуют в одном и том же поле
    // (или точнее – когда у терминов совпадают вторые
    // части ссылок).
    // (F) – второй оператор контекстного И. Соединение
    // двух терминов таким оператором контекстного И
    // обозначает требование поиска записей, в которых
    // оба термина присутствуют в одном и том же повторении
    // поля (или точнее – когда у терминов совпадают
    // вторые и третьи части ссылок).
    // . – (точка обрамленная пробелами) третий оператор
    // контекстного И.Соединение двух терминов таким
    // оператором контекстного И обозначает требование
    // поиска записей, в которых оба термина присутствуют
    // в одном и том же повторении поля друг за другом
    // (или точнее – когда у терминов совпадают вторые
    // и третьи части ссылок, а третьи части ссылок
    // отличаются на единицу).
    //
    // Логические операторы имеют приоритеты, которые
    // определяют порядок их выполнения(в пределах одного
    // уровня скобок). Ниже операторы приведены в порядке
    // убывания приоритета:
    // .
    // (F)
    // (G)
    // * и ^
    // +
    // Операторы одного приоритета выполняются слева
    // направо(в пределах одного уровня скобок).
    // Для изменения порядка выполнения логических
    // операторов в поисковом выражении могут применяться
    // круглые скобки.Выражения в скобках могут
    // объединяться только операторами + * ^.
    //
    // Примеры запросов для прямого поиска:
    //
    // ("A=Иванов$" +"A=Петров$") * ("V=03" + "V=05")
    // "K=трактор$" (F) "K=колесн$" + "K=бульдозер$" (F) "K=гусен$"
    // "K=очист$"/(200,922) * "K=вод$"/(200,922)

    /// <summary>
    /// Parse IRBIS query expression.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SearchQueryParser
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Leaf node.
        /// </summary>
        public void ParseLeaf
            (
                QAstTokenList list,
                QAstLeaf leaf
            )
        {
            QAstToken token = list.Current;
            if (token.Kind != QAstTokenKind.Term)
            {
                throw new IrbisException();
            }
            string text = token.Text;
            if (text.EndsWith("$") || text.EndsWith("@"))
            {
                leaf.Tail = text.Substring(text.Length - 1, 1);
                text = text.Substring(0, text.Length - 1);
            }
            leaf.Term = text;

            list.MoveNext();
            if (list.IsEof)
            {
                return;
            }

            token = list.Current;
            if (token.Kind != QAstTokenKind.Slash)
            {
                return;
            }
            list.RequireNext();

            token = list.Current;
            if (token.Kind != QAstTokenKind.LeftParenthesis)
            {
                throw new IrbisException();
            }
            list.RequireNext();

            List<string> context = new List<string>();
            while (true)
            {
                token = list.Current;
                if (token.Kind == QAstTokenKind.RightParenthesis)
                {
                    leaf.Context = context.ToArray();
                    if (leaf.Context.Length == 0)
                    {
                        leaf.Context = null;
                    }
                    list.MoveNext();
                    return;
                }
                if (token.Kind == QAstTokenKind.Comma)
                {
                    list.RequireNext();
                    continue;
                }
                if (token.Kind != QAstTokenKind.Term)
                {
                    throw new IrbisException();
                }
                context.Add(token.Text);
                list.RequireNext();
            }
            throw new IrbisException();
        }

        /// <summary>
        /// token . token
        /// </summary>
        public void ParseLevel1
            (
                QAstTokenList list, QAstLevel1 level1
            )
        {
            QAstLeaf left = new QAstLeaf();
            ParseLeaf(list, left);
            level1.Left = left;
            List<QAstLeaf> right = new List<QAstLeaf>();
            while (!list.IsEof && list.Current.Kind == QAstTokenKind.Dot)
            {
                list.RequireNext();
                QAstLeaf next = new QAstLeaf();
                ParseLeaf(list, next);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                level1.Right = right.ToArray();
            }
        }

        /// <summary>
        /// token (F) token
        /// </summary>
        public void ParseLevel2
            (
                QAstTokenList list, QAstLevel2 level2
            )
        {
            QAstLevel1 left = new QAstLevel1();
            ParseLevel1(list, left);
            level2.Left = left;
            List<QAstLevel1> right = new List<QAstLevel1>();
            while (!list.IsEof && list.Current.Kind == QAstTokenKind.F)
            {
                list.RequireNext();
                QAstLevel1 next = new QAstLevel1();
                ParseLevel1(list, next);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                level2.Right = right.ToArray();
            }
        }

        /// <summary>
        /// token (G) token
        /// </summary>
        public void ParseLevel3
            (
                QAstTokenList list, QAstLevel3 level3
            )
        {
            QAstLevel2 left = new QAstLevel2();
            ParseLevel2(list, left);
            level3.Left = left;
            List<QAstLevel2> right = new List<QAstLevel2>();
            while (!list.IsEof && list.Current.Kind == QAstTokenKind.G)
            {
                list.RequireNext();
                QAstLevel2 next = new QAstLevel2();
                ParseLevel2(list, next);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                level3.Right = right.ToArray();
            }
        }

        /// <summary>
        /// token * token
        /// </summary>
        public void ParseLevel4
            (
                QAstTokenList list, QAstLevel4 level4
            )
        {
            QAstLevel3 left = new QAstLevel3();
            ParseLevel3(list, left);
            level4.Left = left;
            List<QAstLevel3> right = new List<QAstLevel3>();
            while (!list.IsEof && list.Current.Kind == QAstTokenKind.Star)
            {
                list.RequireNext();
                QAstLevel3 next = new QAstLevel3();
                ParseLevel3(list, next);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                level4.Right = right.ToArray();
            }
        }

        /// <summary>
        /// token ^ token
        /// </summary>
        public void ParseLevel5
            (
                QAstTokenList list, QAstLevel5 level5
            )
        {
            QAstLevel4 left = new QAstLevel4();
            ParseLevel4(list, left);
            level5.Left = left;
            List<QAstLevel4> right = new List<QAstLevel4>();
            while (!list.IsEof && list.Current.Kind == QAstTokenKind.Hat)
            {
                list.RequireNext();
                QAstLevel4 next = new QAstLevel4();
                ParseLevel4(list, next);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                level5.Right = right.ToArray();
            }
        }

        /// <summary>
        /// token + token
        /// </summary>
        public void ParseLevel6
            (
                QAstTokenList list, QAstLevel6 level6
            )
        {
            QAstLevel5 left = new QAstLevel5();
            ParseLevel5(list, left);
            level6.Left = left;
            List<QAstLevel5> right = new List<QAstLevel5>();
            while (!list.IsEof && list.Current.Kind == QAstTokenKind.Plus)
            {
                list.RequireNext();
                QAstLevel5 next = new QAstLevel5();
                ParseLevel5(list, next);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                level6.Right = right.ToArray();
            }
        }

        /// <summary>
        /// ( tokens )
        /// </summary>
        public void ParseLevel7
            (
                QAstTokenList list, QAstLevel7 level7
            )
        {
            if (list.Current.Kind == QAstTokenKind.LeftParenthesis)
            {
                list.RequireNext();
                QAstLevel6 item = new QAstLevel6();
                ParseLevel6(list, item);
                level7.Items = new[] {item};
                if (list.IsEof)
                {
                    throw new IrbisException();
                }
                if (list.Current.Kind != QAstTokenKind.RightParenthesis)
                {
                    throw new IrbisException();
                }
                list.MoveNext();
            }
            else
            {
                QAstLevel6 left = new QAstLevel6();
                ParseLevel6(list, left);
                level7.Items = new[] { left };
            }
        }

        /// <summary>
        /// ( tokens ) * ( tokens )
        /// </summary>
        public void ParseLevel8
            (
                QAstTokenList list, QAstLevel8 level8
            )
        {
            QAstLevel7 left = new QAstLevel7();
            ParseLevel7(list, left);
            level8.Left = left;
            List<QAstLevel7> right = new List<QAstLevel7>();
            while (!list.IsEof && list.Current.Kind == QAstTokenKind.Star)
            {
                list.RequireNext();
                QAstLevel7 next = new QAstLevel7();
                ParseLevel7(list, next);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                level8.Right = right.ToArray();
            }
        }

        /// <summary>
        /// ( tokens ) + ( tokens )
        /// </summary>
        public void ParseLevel9
            (
                QAstTokenList list, QAstLevel9 level9
            )
        {
            QAstLevel8 left = new QAstLevel8();
            ParseLevel8(list, left);
            level9.Left = left;
            List<QAstLevel8> right = new List<QAstLevel8>();
            while (!list.IsEof && list.Current.Kind == QAstTokenKind.Plus)
            {
                list.RequireNext();
                QAstLevel8 next = new QAstLevel8();
                ParseLevel8(list, next);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                level9.Right = right.ToArray();
            }
        }

        /// <summary>
        /// ( tokens ) + ( tokens )
        /// </summary>
        public void ParseLevel10
            (
                QAstTokenList list, QAstLevel10 level10
            )
        {
            QAstLevel9 left = new QAstLevel9();
            ParseLevel9(list, left);
            level10.Left = left;
            List<QAstLevel10> right = new List<QAstLevel10>();
            while (!list.IsEof && list.Current.Kind == QAstTokenKind.Star)
            {
                list.RequireNext();
                QAstLevel10 next = new QAstLevel10();
                ParseLevel10(list, next);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                level10.Right = right.ToArray();
            }
        }

        /// <summary>
        /// ( tokens ) + ( tokens )
        /// </summary>
        public void ParseLevel11
            (
                QAstTokenList list, QAstLevel11 level11
            )
        {
            QAstLevel10 left = new QAstLevel10();
            ParseLevel10(list, left);
            level11.Left = left;
            List<QAstLevel11> right = new List<QAstLevel11>();
            while (!list.IsEof && list.Current.Kind == QAstTokenKind.Plus)
            {
                list.RequireNext();
                QAstLevel11 next = new QAstLevel11();
                ParseLevel11(list, next);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                level11.Right = right.ToArray();
            }
        }

        /// <summary>
        /// Parse the token list.
        /// </summary>
        public QAstRoot Parse
            (
                QAstTokenList list
            )
        {
            QAstRoot result = new QAstRoot();

            if (list.Length != 0)
            {
                QAstLevel11 level11 = new QAstLevel11();
                ParseLevel11(list, level11);
                result.Level11 = level11;
            }

            return result;
        }

        /// <summary>
        /// Tokenize the text.
        /// </summary>
        public QAstTokenList Tokenize
            (
                string text
            )
        {
            List<QAstToken> result = new List<QAstToken>();
            TextNavigator navigator = new TextNavigator(text);

            while (!navigator.IsEOF)
            {
                navigator.SkipWhitespace();
                if (navigator.IsEOF)
                {
                    break;
                }

                char c = navigator.ReadChar();
                string value;
                int position = navigator.Position;
                QAstTokenKind kind;
                switch (c)
                {
                    case '"':
                        value = navigator.ReadUntil('"').ThrowIfNull();
                        kind = QAstTokenKind.Term;
                        if (navigator.ReadChar() != '"')
                        {
                            throw new IrbisException();
                        }
                        break;

                    case '#':
                        value = navigator.ReadWhile('0', '1', '2', '3', '4', '5', '6', '7', '8', '9')
                            .ThrowIfNull();
                        kind = QAstTokenKind.Sharp;
                        break;

                    case '+':
                        value = c.ToString();
                        kind = QAstTokenKind.Plus;
                        break;

                    case '*':
                        value = c.ToString();
                        kind = QAstTokenKind.Star;
                        break;

                    case '^':
                        value = c.ToString();
                        kind = QAstTokenKind.Hat;
                        break;

                    case '.':
                        value = c.ToString();
                        kind = QAstTokenKind.Dot;
                        break;

                    case '/':
                        value = c.ToString();
                        kind = QAstTokenKind.Slash;
                        break;

                    case ',':
                        value = c.ToString();
                        kind = QAstTokenKind.Comma;
                        break;

                    case '(':
                        string preview = c + navigator.PeekString(2);
                        if (preview == "(G)" || preview == "(g)")
                        {
                            value = preview;
                            kind = QAstTokenKind.G;
                            navigator.ReadChar();
                            navigator.ReadChar();
                        }
                        else if (preview == "(F)" || preview == "(f)")
                        {
                            value = preview;
                            kind = QAstTokenKind.F;
                            navigator.ReadChar();
                            navigator.ReadChar();
                        }
                        else
                        {
                            value = c.ToString();
                            kind = QAstTokenKind.LeftParenthesis;
                        }
                        break;

                    case ')':
                        value = c.ToString();
                        kind = QAstTokenKind.RightParenthesis;
                        break;

                    default:
                        value = c + navigator.ReadUntil('(', '/', '\t', ' ', ',', ')');
                        kind = QAstTokenKind.Term;
                        break;
                }

                if (kind == QAstTokenKind.None)
                {
                    throw new IrbisException();
                }

                QAstToken token = new QAstToken(kind, position, value);

                result.Add(token);
            }

            return new QAstTokenList(result);
        }

        #endregion
    }
}
