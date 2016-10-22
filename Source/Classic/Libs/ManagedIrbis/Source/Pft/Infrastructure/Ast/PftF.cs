/* PftF.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    //
    // Official documentation
    //
    // Функция F(выр-1,выр-2,выр-3)
    //
    // Функция F преобразует числовое значение
    // из его внутреннего представления с плавающей
    // точкой в символьную строку.
    //
    // Все три аргумента являются числовыми выражениями.
    // * Первый аргумент - выр-1, является числом,
    // которое необходимо преобразовать.
    // * Второй аргумент  - выр-2, минимальная длина
    // выходной строки, выделяемая для результата,
    // * Третий аргумент  - выр-3, количество десятичных цифр.
    //
    // Второй и третий аргументы необязательны.
    // Отметим, однако, что если присутствует выр-3,
    // то выр-2 не может быть опущено.
    //
    // Выр-2 определяет минимальную длину, т.е.
    // значением функции будет символьная строка длиной
    // как минимум выр-2 символов, и если преобразуемое
    // числовое значение требует выр-2 символов или меньше,
    // оно будет выровнено по правой границе в пределах
    // этой длины. Если количество символов, требуемое
    // для представления значения выр-1, больше данной
    // длины, то используются дополнительные позиции.
    // В этом случае выходная строка будет длиннее,
    // чем выр-2 символов.
    //
    // Выр-3 определяет количество десятичных цифр
    // дробной части Выр-1.
    // Если оно опущено, то результат будет представлен
    // в экспоненциальной форме. Если при этом также
    // опущено выр-2, то по умолчанию длина выходной
    // строки будет равна 16 символам.
    // Если выр-3 присутствует, то результатом будет
    // округленное представление выр-1 с фиксированной
    // точкой с выр-3 цифрами после десятичной точки.
    //
    // Если выр-3 равно нулю, то выр-1 округляется
    // до ближайшего целого числа и результатом будет
    // целое число без десятичной точки.
    //
    // Если при преобразовании целых чисел и чисел
    // с фиксированной точкой оказывается,
    // что целая часть числа слишком большая для
    // ее представления, то выходная строка заменяется
    // последовательностью символов "*".
    //
    // Функция F может использоваться для выравнивания
    // колонки чисел по десятичной точке путем выбора
    // соответствующей длины.
    //
    // Примеры функции F
    //
    // Выражение                       Значение
    // ------------------------------- ----------------
    // f(1)                            1.000000000E+00
    // f(1,10)                         1.000E+00
    // f(-1,10,2)                      -1.00
    // f(1,5,2)                        1.00
    // f(1,8,2)                        1.00
    // f(mfn,1,0)                      4
    // f(mfn,2,0)                      4
    // f(mfn,3,0)                      4
    //


    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftF
        : PftNode
    {
        #region Properties

        /// <summary>
        /// First argument value.
        /// </summary>
        public PftNumeric Argument1 { get; set; }

        /// <summary>
        /// Second argument value.
        /// </summary>
        public PftNumeric Argument2 { get; set; }

        /// <summary>
        /// Third argument value.
        /// </summary>
        public PftNumeric Argument3 { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftF()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftF
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            PftContext clone = context.Push();
            Argument1.Execute(clone);
            context.Pop();
            double value = Argument1.Value;

            int minWidth = -1;
            if (!ReferenceEquals(Argument2, null))
            {
                clone = context.Push();
                Argument2.Execute(clone);
                context.Pop();
                minWidth = (int) Argument2.Value;
            }

            int decimalPoints = -1;
            if (!ReferenceEquals(Argument3, null))
            {
                clone = context.Push();
                Argument3.Execute(clone);
                context.Pop();
                decimalPoints = (int) Argument3.Value;
            }

            string result = PftUtility.FormatLikeF
                (
                    value,
                    minWidth,
                    decimalPoints
                );
            context.Write(this, result);

            // Doesn't touch context.OutputFlag

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override void PrintDebug
            (
                TextWriter writer,
                int level
            )
        {
            for (int i = 0; i < level; i++)
            {
                writer.Write("| ");
            }
            writer.WriteLine("F: ");

            if (!ReferenceEquals(Argument1, null))
            {
                for (int i = 0; i <= level; i++)
                {
                    writer.Write("| ");
                }
                writer.WriteLine("Argument1: ");
                Argument1.PrintDebug(writer, level + 2);
            }

            if (!ReferenceEquals(Argument2, null))
            {
                for (int i = 0; i <= level; i++)
                {
                    writer.Write("| ");
                }
                writer.WriteLine("Argument2: ");
                Argument2.PrintDebug(writer, level + 2);
            }

            if (!ReferenceEquals(Argument3, null))
            {
                for (int i = 0; i <= level; i++)
                {
                    writer.Write("| ");
                }
                writer.WriteLine("Argument3: ");
                Argument3.PrintDebug(writer, level + 2);
            }
        }

        #endregion
    }
}
