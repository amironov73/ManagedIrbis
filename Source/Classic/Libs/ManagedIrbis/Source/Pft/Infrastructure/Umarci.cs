/* Umarci.cs --
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
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    //
    // Unofficial documentation
    //
    // Форматный выход &amp;umarci
    //
    // &umarci('1N1#i#N2') - выбирает N2-е 
    // повторение подполя i поля N1.
    //
    // &umarci('2N1#S') - определяет количество
    // вхождений строки S в поле N1; длина S <= 10 симв.
    //
    // &umarci('3N1#N2#R) - из поля N1 выбирает
    // информацию между (N2-1)-ым и N2-ым разделителями R,
    // если N2<1 и до N2, если N2=1.
    //
    // &umarci('4N1/N2') - выдает содержимое 
    // поля с меткой N2, встроенного в поле N1.
    //
    // &umarci('0a') - когда-то использовалась 
    // для замены разделителей, но теперь замена происходит,
    // если имя fst импорта содержит 'marc' как часть.
    //


    /// <summary>
    /// Umarci.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Umarci
        : IFormatExit
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public static Dictionary<string, Action<PftContext, PftNode, string>> Registry { get; private set; }

        /// <summary>
        /// Throw exception on unknown key.
        /// </summary>
        public static bool ThrowOnUnknown { get; set; }

        #endregion

        #region Construction

        static Umarci()
        {
            ThrowOnUnknown = false;

            Registry = new Dictionary<string, Action<PftContext, PftNode, string>>
                (
#if NETCORE || UAP || WIN81

                    StringComparer.OrdinalIgnoreCase

#else

StringComparer.InvariantCultureIgnoreCase

#endif
);

            RegisterActions();
        }

        #endregion

        #region Private members

        private static void RegisterActions()
        {
            Registry.Add("0", Umarci0);
            Registry.Add("1", Umarci1);
            Registry.Add("2", Umarci2);
            Registry.Add("3", Umarci3);
            Registry.Add("4", Umarci4);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Find action for specified expression.
        /// </summary>
        public static Action<PftContext, PftNode, string> FindAction
            (
                [NotNull] ref string expression
            )
        {
            var keys = Registry.Keys;
            int bestMatch = 0;
            Action<PftContext, PftNode, string> result = null;

            foreach (string key in keys)
            {
                if (key.Length > bestMatch
                    && expression.StartsWith(key))
                {
                    bestMatch = key.Length;
                    result = Registry[key];
                }
            }

            if (bestMatch != 0)
            {
                expression = expression.Substring(bestMatch);
            }

            return result;
        }

        /// <summary>
        /// Handle command 0.
        /// </summary>
        public static void Umarci0
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            // Nothing to do actually
        }

        /// <summary>
        /// Handle command 1.
        /// </summary>
        public static void Umarci1
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            //
            // field#code#repeat
            //
            // e.g.: if &umarci('1101#a#2') <> '' then ...
            //

            if (string.IsNullOrEmpty(expression)
                || ReferenceEquals(context.Record, null))
            {
                return;
            }

            string[] parts = expression.Split('#');
            if (parts.Length != 3)
            {
                return;
            }

            string tag = parts[0];
            string code = parts[1];
            if (string.IsNullOrEmpty(tag)
                || code.Length != 1)
            {
                return;
            }
            int repeat;
            if (!int.TryParse(parts[2], out repeat))
            {
                return;
            }
            repeat--;
            if (repeat < 0)
            {
                return;
            }

            RecordField field = context.Record.Fields
                .GetField(tag, context.Index);
            if (ReferenceEquals(field, null))
            {
                return;
            }
            string text = field.GetSubFieldValue(code[0], repeat);
            if (!string.IsNullOrEmpty(text))
            {
                context.Write(node, text);
            }
        }

        /// <summary>
        /// Handle command 2.
        /// </summary>
        public static void Umarci2
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            //
            // field#substring
            //
            // e.g.: if val(&umarci('2998#^a'))>1 then ...
            //

            if (string.IsNullOrEmpty(expression)
                || ReferenceEquals(context.Record, null))
            {
                return;
            }

            string[] parts = expression.Split('#');
            if (parts.Length != 2)
            {
                return;
            }

            string tag = parts[0];
            string substring = parts[1];
            if (string.IsNullOrEmpty(tag)
                || string.IsNullOrEmpty(substring))
            {
                return;
            }

            RecordField field = context.Record.Fields
                .GetField(tag, context.Index);
            if (ReferenceEquals(field, null))
            {
                return;
            }

            string text = field.ToText();
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            int result = text.CountSubstrings(substring);
            context.Write(node, result.ToInvariantString());
        }

        /// <summary>
        /// Handle command 3.
        /// </summary>
        public static void Umarci3
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            //
            // field#index#separator
            //
            // e.g.: &umarci('391#2#+')
            //

            //
            // sample field: #1: 11|22|33|44
            //
            // &umarci('31#0#|') gives 11|22|33|44
            // &umarci('31#1#|') gives 11
            // &umarci('31#2#|') gives 22
            // &umarci('31#3#|') gives 33
            // &umarci('31#4#|') gives 44
            // &umarci('31#5#|') gives empty string
            //

            if (string.IsNullOrEmpty(expression)
                || ReferenceEquals(context.Record, null))
            {
                return;
            }

            string[] parts = expression.Split('#');
            if (parts.Length != 3)
            {
                return;
            }

            string tag = parts[0];
            string indexText = parts[1];
            string separator = parts[2];
            if (string.IsNullOrEmpty(tag)
                || string.IsNullOrEmpty(indexText)
                || string.IsNullOrEmpty(separator)
                || separator.Length != 1)
            {
                return;
            }

            RecordField field = context.Record.Fields
                .GetField(tag, context.Index);
            if (ReferenceEquals(field, null))
            {
                return;
            }

            string text = field.ToText();
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            int index;
            if (!int.TryParse(indexText, out index))
            {
                return;
            }

            if (index <= 0)
            {
                context.Write(node, text);
                return;
            }
            index--;

            int[] positions = text.GetPositions(separator[0]);

            if (positions.Length == 0)
            {
                if (index <= 0)
                {
                    context.Write(node, text);
                    return;
                }
            }

            if (index == 0)
            {
                text = text.Substring(0, positions[0]);
                context.Write(node, text);
                return;
            }

            int start, end, length;

            if (index < positions.Length)
            {
                start = positions[index - 1] + 1;
                end = positions[index];
                length = end - start;
                text = text.Substring(start, length);
            }
            else if (index == positions.Length)
            {
                start = positions[index - 1] + 1;
                end = text.Length;
                length = end - start - 1;
                text = text.Substring(start, length);
            }
            else
            {
                text = string.Empty;
            }

            context.Write(node, text);
        }

        /// <summary>
        /// Handle command 4.
        /// </summary>
        public static void Umarci4
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            //
            // tag/embed^code
            //
            // e.g.: &umarci('4461/011^а')
            //

            if (string.IsNullOrEmpty(expression)
                || ReferenceEquals(context.Record, null))
            {
                return;
            }

            string[] parts = expression.Split('/');
            if (parts.Length != 2)
            {
                return;
            }

            string tag = parts[0];
            string embed = parts[1];
            if (string.IsNullOrEmpty(tag)
                || string.IsNullOrEmpty(embed))
            {
                return;
            }

            parts = embed.Split('^');
            char code = '\0';
            if (parts.Length == 2)
            {
                embed = parts[0];
                code = parts[1].ToCharArray().GetOccurrence(0);
            }

            RecordField field = context.Record.Fields
                .GetField(tag, context.Index);
            if (ReferenceEquals(field, null))
            {
                return;
            }

            field = field.GetEmbeddedFields()
                .GetField(embed)
                .GetOccurrence(0);
            if (ReferenceEquals(field, null))
            {
                return;
            }

            string text = (code == '\0')
                ? field.ToText()
                : (code == '*')
                ? field.Value
                : field.GetFirstSubFieldValue(code);

            context.Write(node, text);
        }

        #endregion

        #region IFormatExit members

        /// <inheritdoc/>
        public string Name { get { return "umarci"; } }

        /// <inheritdoc/>
        public void Execute
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(node, "node");

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            Action<PftContext, PftNode, string> action
                = FindAction(ref expression);

            if (ReferenceEquals(action, null))
            {
                if (ThrowOnUnknown)
                {
                    throw new PftException("Unknown unifor: " + expression);
                }
            }
            else
            {
                action
                    (
                        context,
                        node,
                        expression
                    );
            }
        }

        #endregion
    }
}
