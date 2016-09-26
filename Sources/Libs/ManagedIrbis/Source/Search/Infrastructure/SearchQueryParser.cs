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
        public QAstLeaf ParseLeaf
            (
                QTokenList list
            )
        {
            QAstLeaf result = new QAstLeaf();

            QToken token = list.Current;
            if (token.Kind != QTokenKind.Term)
            {
                throw new IrbisException();
            }

            string text = token.Text;
            if (text.EndsWith("$") || text.EndsWith("@"))
            {
                result.Tail = text.Substring(text.Length - 1, 1);
                text = text.Substring(0, text.Length - 1);
            }
            result.Term = text;

            if (!list.MoveNext())
            {
                return result;
            }

            token = list.Current;
            if (token.Kind != QTokenKind.Slash)
            {
                return result;
            }
            list.RequireNext();

            token = list.Current;
            if (token.Kind != QTokenKind.LeftParenthesis)
            {
                throw new IrbisException();
            }
            list.RequireNext();

            List<string> context = new List<string>();
            while (true)
            {
                token = list.Current;

                if (token.Kind == QTokenKind.RightParenthesis)
                {
                    result.Context = context.ToArray();
                    if (result.Context.Length == 0)
                    {
                        result.Context = null;
                    }
                    list.MoveNext();
                    return result;
                }

                if (token.Kind == QTokenKind.Comma)
                {
                    list.RequireNext();
                    continue;
                }

                if (token.Kind != QTokenKind.Term)
                {
                    throw new IrbisException();
                }
                context.Add(token.Text);

                list.RequireNext();
            }
        }

        /// <summary>
        /// token . token
        /// </summary>
        public QAstLevel1 ParseLevel1
            (
                QTokenList list
            )
        {
            QAstLevel1 result = new QAstLevel1
            {
                Left = ParseLeaf(list)
            };
            List<QAstLeaf> right = new List<QAstLeaf>();
            while (!list.IsEof && list.Current.Kind == QTokenKind.Dot)
            {
                list.RequireNext();
                QAstLeaf next = ParseLeaf(list);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                result.Right = right.ToArray();
            }

            return result;
        }

        /// <summary>
        /// token (F) token
        /// </summary>
        public QAstLevel2 ParseLevel2
            (
                QTokenList list
            )
        {
            QAstLevel2 result = new QAstLevel2
            {
                Left = ParseLevel1(list)
            };
            List<QAstLevel1> right = new List<QAstLevel1>();
            while (!list.IsEof && list.Current.Kind == QTokenKind.F)
            {
                list.RequireNext();
                QAstLevel1 next = ParseLevel1(list);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                result.Right = right.ToArray();
            }

            return result;
        }

        /// <summary>
        /// token (G) token
        /// </summary>
        public QAstLevel3 ParseLevel3
            (
                QTokenList list
            )
        {
            QAstLevel3 result = new QAstLevel3
            {
                Left = ParseLevel2(list)
            };
            List<QAstLevel2> right = new List<QAstLevel2>();
            while (!list.IsEof && list.Current.Kind == QTokenKind.G)
            {
                list.RequireNext();
                QAstLevel2 next = ParseLevel2(list);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                result.Right = right.ToArray();
            }

            return result;
        }

        /// <summary>
        /// token * token
        /// </summary>
        public QAstLevel4 ParseLevel4
            (
                QTokenList list
            )
        {
            QAstLevel4 result = new QAstLevel4
            {
                Left = ParseLevel3(list)
            };
            List<QAstLevel3> right = new List<QAstLevel3>();
            while (!list.IsEof && list.Current.Kind == QTokenKind.Star)
            {
                list.RequireNext();
                QAstLevel3 next = ParseLevel3(list);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                result.Right = right.ToArray();
            }

            return result;
        }

        /// <summary>
        /// token ^ token
        /// </summary>
        public QAstLevel5 ParseLevel5
            (
                QTokenList list
            )
        {
            QAstLevel5 result = new QAstLevel5
            {
                Left = ParseLevel4(list)
            };
            List<QAstLevel4> right = new List<QAstLevel4>();
            while (!list.IsEof && list.Current.Kind == QTokenKind.Hat)
            {
                list.RequireNext();
                QAstLevel4 next = ParseLevel4(list);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                result.Right = right.ToArray();
            }

            return result;
        }

        /// <summary>
        /// token + token
        /// </summary>
        public QAstLevel6 ParseLevel6
            (
                QTokenList list
            )
        {
            QAstLevel6 result = new QAstLevel6
            {
                Left = ParseLevel5(list)
            };
            List<QAstLevel5> right = new List<QAstLevel5>();
            while (!list.IsEof && list.Current.Kind == QTokenKind.Plus)
            {
                list.RequireNext();
                QAstLevel5 next = ParseLevel5(list);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                result.Right = right.ToArray();
            }

            return result;
        }

        /// <summary>
        /// ( tokens )
        /// </summary>
        public QAstLevel7 ParseLevel7
            (
                QTokenList list
            )
        {
            QAstLevel7 result = new QAstLevel7();

            if (list.Current.Kind == QTokenKind.LeftParenthesis)
            {
                list.RequireNext();
                QAstLevel6 item = ParseLevel6(list);
                result.Items = new[] {item};
                if (list.IsEof)
                {
                    throw new IrbisException();
                }
                if (list.Current.Kind != QTokenKind.RightParenthesis)
                {
                    throw new IrbisException();
                }
                list.MoveNext();
            }
            else
            {
                QAstLevel6 left = ParseLevel6(list);
                result.Items = new[] { left };
            }

            return result;
        }

        /// <summary>
        /// ( tokens ) * ( tokens )
        /// </summary>
        public QAstLevel8 ParseLevel8
            (
                QTokenList list
            )
        {
            QAstLevel8 result = new QAstLevel8
            {
                Left = ParseLevel7(list)
            };
            List<QAstLevel7> right = new List<QAstLevel7>();
            while (!list.IsEof && list.Current.Kind == QTokenKind.Star)
            {
                list.RequireNext();
                QAstLevel7 next = ParseLevel7(list);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                result.Right = right.ToArray();
            }

            return result;
        }

        /// <summary>
        /// ( tokens ) + ( tokens )
        /// </summary>
        public QAstLevel9 ParseLevel9
            (
                QTokenList list
            )
        {
            QAstLevel9 result = new QAstLevel9
            {
                Left = ParseLevel8(list)
            };
            List<QAstLevel8> right = new List<QAstLevel8>();
            while (!list.IsEof && list.Current.Kind == QTokenKind.Plus)
            {
                list.RequireNext();
                QAstLevel8 next = ParseLevel8(list);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                result.Right = right.ToArray();
            }

            return result;
        }

        /// <summary>
        /// ( tokens ) + ( tokens )
        /// </summary>
        public QAstLevel10 ParseLevel10
            (
                QTokenList list
            )
        {
            QAstLevel10 result = new QAstLevel10
            {
                Left = ParseLevel9(list)
            };
            List<QAstLevel10> right = new List<QAstLevel10>();
            while (!list.IsEof && list.Current.Kind == QTokenKind.Star)
            {
                list.RequireNext();
                QAstLevel10 next = ParseLevel10(list);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                result.Right = right.ToArray();
            }

            return result;
        }

        /// <summary>
        /// ( tokens ) + ( tokens )
        /// </summary>
        public QAstLevel11 ParseLevel11
            (
                QTokenList list
            )
        {
            QAstLevel11 result = new QAstLevel11
            {
                Left = ParseLevel10(list)
            };
            List<QAstLevel11> right = new List<QAstLevel11>();
            while (!list.IsEof && list.Current.Kind == QTokenKind.Plus)
            {
                list.RequireNext();
                QAstLevel11 next = ParseLevel11(list);
                right.Add(next);
            }
            if (right.Count != 0)
            {
                result.Right = right.ToArray();
            }

            return result;
        }

        /// <summary>
        /// Parse the token list.
        /// </summary>
        public QAstRoot Parse
            (
                [NotNull] QTokenList list
            )
        {
            Code.NotNull(list, "list");

            QAstRoot result = new QAstRoot();

            if (list.Length != 0)
            {
                QAstLevel11 level11 = ParseLevel11(list);
                result.Level11 = level11;
            }

            return result;
        }

        /// <summary>
        /// Tokenize the text.
        /// </summary>
        public QTokenList Tokenize
            (
                string text
            )
        {
            List<QToken> result = new List<QToken>();
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
                QTokenKind kind;
                switch (c)
                {
                    case '"':
                        value = navigator.ReadUntil('"').ThrowIfNull();
                        kind = QTokenKind.Term;
                        if (navigator.ReadChar() != '"')
                        {
                            throw new IrbisException();
                        }
                        break;

                    case '#':
                        value = navigator.ReadWhile('0', '1', '2', '3', '4', '5', '6', '7', '8', '9')
                            .ThrowIfNull();
                        kind = QTokenKind.Sharp;
                        break;

                    case '+':
                        value = c.ToString();
                        kind = QTokenKind.Plus;
                        break;

                    case '*':
                        value = c.ToString();
                        kind = QTokenKind.Star;
                        break;

                    case '^':
                        value = c.ToString();
                        kind = QTokenKind.Hat;
                        break;

                    case '.':
                        value = c.ToString();
                        kind = QTokenKind.Dot;
                        break;

                    case '/':
                        value = c.ToString();
                        kind = QTokenKind.Slash;
                        break;

                    case ',':
                        value = c.ToString();
                        kind = QTokenKind.Comma;
                        break;

                    case '(':
                        string preview = c + navigator.PeekString(2);
                        if (preview == "(G)" || preview == "(g)")
                        {
                            value = preview;
                            kind = QTokenKind.G;
                            navigator.ReadChar();
                            navigator.ReadChar();
                        }
                        else if (preview == "(F)" || preview == "(f)")
                        {
                            value = preview;
                            kind = QTokenKind.F;
                            navigator.ReadChar();
                            navigator.ReadChar();
                        }
                        else
                        {
                            value = c.ToString();
                            kind = QTokenKind.LeftParenthesis;
                        }
                        break;

                    case ')':
                        value = c.ToString();
                        kind = QTokenKind.RightParenthesis;
                        break;

                    default:
                        value = c + navigator.ReadUntil('(', '/', '\t', ' ', ',', ')');
                        kind = QTokenKind.Term;
                        break;
                }

                if (kind == QTokenKind.None)
                {
                    throw new IrbisException();
                }

                QToken token = new QToken(kind, position, value);

                result.Add(token);
            }

            return new QTokenList(result);
        }

        #endregion
    }
}
