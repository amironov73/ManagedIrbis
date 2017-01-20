// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftVal.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    //
    // Official documentation
    //
    // Функция VAL возвращает числовое значение своего
    // аргумента.Аргумент - это формат, который может
    // содержать любую допустимую команду форматирования.
    // Сначала вычисляется аргумент, чтобы получить строку
    // текста. Затем эта строка просматривается слева
    // направо до тех пор, пока не будет найдено числовое
    // значение, представленное в текстовом виде (которое
    // может быть представлено в экспоненциальной форме).
    // Функция VAL возвращает это числовое значение,
    // переведенное во внутреннее машинное представление,
    // удобное для выполнения вычислений.
    //
    // Если не найдено ни одно числовое значение,
    // то функция возвращает значение ноль.
    // Если текст содержит более, чем одно числовое
    // значение, возвращается только первое.
    //
    // Ниже приведены примеры функции VAL (при этом
    // предполагается, что v1^a= 10, v1^b= 20 и v2 = 30):
    //
    // Формат                          Значение
    // ------------------------------- ----------------
    // val('15.79')                    15.79
    // val(v1)                         10
    // val(v1^a)                       10
    // val(v2)                         30
    // val("19"v1^b)                   1920
    // val('xxxx7yyy8zzzz')            7
    // val('abs.5.8е-4 ml')            0.00058
    // val('вода')                     0
    // val('Июль-Август 1985')         0
    //
    // В этом примере в последней строке значение 0
    // (а не 1985), так как система рассматривает
    // минус между словами Июль и Август как начало
    // отрицательного числового значения, а букву А
    // от Август как его конец, поэтому выбранное
    // значение получается просто '-' и результатом
    // выполнения функции является 0. В связи с этим,
    // для тех полей или подполей, которые будут
    // использоваться для вычислений, важно с самого
    // начала четко определить правила ввода данных.
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftVal
        : PftNumeric
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVal()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVal
            (
                double value
            )
            : base(value)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVal
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

            string text = context.Evaluate(Children);
            Value = PftUtility.ExtractNumericValue(text);

            OnAfterExecution(context);
        }

        #endregion
    }
}
