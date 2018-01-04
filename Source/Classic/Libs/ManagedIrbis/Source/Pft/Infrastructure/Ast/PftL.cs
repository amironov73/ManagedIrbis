// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftL.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    //
    // Функция L
    // использует текст, полученный в результате вычисления аргумента,
    // в качестве термина доступа для инвертированного файла
    // и возвращает MFN первой ссылки на этот термин, если она есть.
    //
    // Перед поиском в инвертированном файле термин автоматически
    // переводится в прописные буквы. Если термин не найден,
    // то функция принимает значение ноль.
    //
    // Функция L обычно используется вместе с функцией REF .
    //
    // Обратим внимание, что формат, расположенный в аргументе,
    // вычисляется с использованием текущего режима вывода.
    // Это является существенным, так как использование неправильного
    // режима может привести к тому, что термин не будет найден
    // в инвертированном файле. Как правило, следует использовать
    // тот же режим, который применяется в ТВП для инвертированного файла.
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftL
        : PftNumeric
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftL()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftL
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.L);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftL
            (
                params PftNode[] body
            )
        {
            foreach (PftNode node in body)
            {
                Children.Add(node);
            }
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.CompileNodes(Children);

            string actionName = compiler.CompileAction(Children);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("double result = 0.0;");

            if (!string.IsNullOrEmpty(actionName))
            {
                compiler
                    .WriteIndent()
                    .WriteLine("string text = Evaluate({0});", actionName)
                    .WriteIndent()
                    .WriteLine("if (!string.IsNullOrEmpty(text))")
                    .WriteIndent()
                    .WriteLine("{")
                    .IncreaseIndent()
                    .WriteIndent()
                    .WriteLine("int[] found = Context.Provider.Search(text);")
                    .WriteIndent()
                    .WriteLine("if (found.Length != 0)")
                    .WriteIndent()
                    .WriteLine("{")
                    .IncreaseIndent()
                    .WriteIndent()
                    .WriteLine("result = found[0];")
                    .DecreaseIndent()
                    .WriteIndent()
                    .WriteLine("}")
                    .DecreaseIndent()
                    .WriteIndent()
                    .WriteLine("}");
            }

            compiler
                .WriteIndent()
                .WriteLine("return result;");

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            Value = 0;
            string expression = context.Evaluate(Children);
            if (!string.IsNullOrEmpty(expression))
            {
                // TODO get the first found item only

                int[] found = context.Provider.Search(expression);
                if (found.Length != 0)
                {
                    Value = found[0];
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer
                .SingleSpace()
                .Write("l(");
            base.PrettyPrint(printer);
            printer.Write(')');
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        protected internal override bool ShouldSerializeText()
        {
            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("l(");
            PftUtility.NodesToText(result, Children);
            result.Append(')');

            return result.ToString();
        }

        #endregion
    }
}
