/* IrbisEncoding.cs -- работа с кодировками
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Работа с кодировками в ИРБИС.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisEncoding
    {
        #region Properties

        /// <summary>
        /// Однобайтная кодировка по умолчанию.
        /// </summary>
        [NotNull]
        public static Encoding Ansi { get { return _ansi; } }

        #endregion

        #region Private members

        private static Encoding _ansi = Encoding.GetEncoding(1251);

        #endregion

        #region Public methods

        /// <summary>
        /// Установка однобайтной кодировки по умолчанию.
        /// </summary>
        public static void SetAnsiEncoding
            (
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(() => encoding);

            if (!encoding.IsSingleByte)
            {
                throw new ArgumentOutOfRangeException("encoding");
            }

            _ansi = encoding;
        }

        #endregion
    }
}
