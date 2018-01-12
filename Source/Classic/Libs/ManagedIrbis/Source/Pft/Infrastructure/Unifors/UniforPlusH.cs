// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusH.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using AM.Logging;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанная функция unifor('+H')
    // Очень странная функция.
    // Перебирает строку как массив однобайтовых символов.
    // Выкидывает каждый четвертый, в начало строки помещает
    // количество таких групп.
    //

    static class UniforPlusH
    {
        #region Public methods

        /// <summary>
        /// Take every 3 of four bytes
        /// </summary>
        public static void Take3Of4
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            Encoding encoding = IrbisEncoding.Utf8;
            byte[] bytes = encoding.GetBytes(expression);
            List<byte> list = new List<byte>();
            int length = bytes.Length < 3
                ? 0
                : unchecked((bytes.Length + 3) / 4);
            list.Add((byte)('0' + length));
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 4 != 3)
                {
                    list.Add(bytes[i]);
                }
            }

            try
            {
                string output = encoding.GetString(list.ToArray());
                context.WriteAndSetFlag(node, output);
            }
            catch (Exception exception)
            {
                Log.TraceException("UniforPlusH::Take3Of4", exception);
            }
        }

        #endregion
    }
}
