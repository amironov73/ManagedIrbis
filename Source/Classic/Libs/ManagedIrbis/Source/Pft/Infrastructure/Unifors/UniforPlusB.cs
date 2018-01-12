// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusB.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using JetBrains.Annotations;

#endregion

// ReSharper disable ForCanBeConvertedToForeach

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанная функция unifor('+B').
    // Суммирует байты входной строки.
    //

    static class UniforPlusB
    {
        #region Public methods

        /// <summary>
        /// Sum of input string bytes.
        /// </summary>
        public static void ByteSum
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!ReferenceEquals(expression, null))
            {
                byte[] bytes = IrbisEncoding.Utf8.GetBytes(expression);
                long sum = 0;
                unchecked
                {
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        sum += bytes[i];
                    }
                }

                string output = sum.ToInvariantString();
                context.WriteAndSetFlag(node, output);
            }
        }

        #endregion
    }
}
