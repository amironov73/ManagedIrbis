/* TextReaderUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
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

namespace AM.IO
{
    [PublicAPI]
    [MoonSharpUserData]
    public static class TextReaderUtility
    {
        #region Public methods

        /// <summary>
        /// Обязательное чтение строки.
        /// </summary>
        [NotNull]
        public static string RequireLine
            (
                [NotNull] this TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            string result = reader.ReadLine();
            if (ReferenceEquals(result, null))
            {
                throw new ArsMagnaException
                    (
                        "Unexpected end of stream"
                    );
            }

            return result;
        }

        #endregion
    }
}
