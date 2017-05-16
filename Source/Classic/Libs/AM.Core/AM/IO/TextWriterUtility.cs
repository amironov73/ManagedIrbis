// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextWriterUtility.cs --
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

namespace AM.IO
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class TextWriterUtility
    {
        #region Public methods

        /// <summary>
        /// Open file for append.
        /// </summary>
        [NotNull]
        public static StreamWriter Append
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
#if WIN81 || PORTABLE

            throw new NotImplementedException();

#else

            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            StreamWriter result = new StreamWriter
                (
                    new FileStream
                        (
                            fileName,
                            FileMode.Append
                        ),
                    encoding
                );

            return result;

#endif
        }

        /// <summary>
        /// Open file for writing.
        /// </summary>
        [NotNull]
        public static StreamWriter Create
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
#if WIN81 || PORTABLE

            throw new NotImplementedException();

#else

            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            StreamWriter result = new StreamWriter
                (
                    new FileStream
                        (
                            fileName, 
                            FileMode.Create
                        ),
                    encoding
                );

            return result;

#endif
        }

        #endregion
    }
}
