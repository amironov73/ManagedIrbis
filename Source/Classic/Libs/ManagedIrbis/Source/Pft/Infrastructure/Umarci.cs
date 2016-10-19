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
    // &umarci('0a') - когда-то использовалась 
    // для замены разделителей, но теперь замена происходит,
    // если имя fst импорта содержит 'marc' как часть.
    //
    // &umarci('4N1/N2') - выдает содержимое 
    // поля с меткой N2, встроенного в поле N1.
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
        public static Dictionary<string, Action<PftContext, string>> Registry { get; private set; }

        #endregion

        #region Construction

        static Umarci()
        {
            Registry = new Dictionary<string, Action<PftContext, string>>
                (
#if NETCORE || UAP || WIN81

                    StringComparer.OrdinalIgnoreCase

#else

                    StringComparer.InvariantCultureIgnoreCase

#endif
                );
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

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

            context.Write
                (
                    node,
                    expression
                );
        }

        #endregion
    }
}
