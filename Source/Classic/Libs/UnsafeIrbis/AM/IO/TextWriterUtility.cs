// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextWriterUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Text;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.IO
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
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
            Code.NotNullNorEmpty(fileName, nameof(fileName));
            Code.NotNull(encoding, nameof(encoding));

            StreamWriter result = new StreamWriter
                (
                    new FileStream(fileName, FileMode.Append),
                    encoding
                );

            return result;
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
            Code.NotNullNorEmpty(fileName, nameof(fileName));
            Code.NotNull(encoding, nameof(encoding));

            StreamWriter result = new StreamWriter
                (
                    new FileStream(fileName, FileMode.Create),
                    encoding
                );

            return result;
        }

        #endregion
    }
}
