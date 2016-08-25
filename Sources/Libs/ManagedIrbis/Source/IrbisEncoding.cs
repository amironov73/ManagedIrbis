/* IrbisEncoding.cs -- encodings used by IRBIS
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * State: moderate
 */

#region Using directives

using System;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Encoding used by IRBIS
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisEncoding
    {
        #region Properties

        /// <summary>
        /// Default single-byte encoding.
        /// </summary>
        [NotNull]
        public static Encoding Ansi { get { return _ansi; } }

        /// <summary>
        /// OEM encoding.
        /// </summary>
        public static Encoding Oem { get { return _oem; } }

        /// <summary>
        /// UTF8 encoding.
        /// </summary>
        public static Encoding Utf8 { get { return Encoding.UTF8; } }

        #endregion

        #region Private members

        private static Encoding _ansi = Encoding.GetEncoding(1251);

        private static Encoding _oem = Encoding.GetEncoding(866);

        #endregion

        #region Public methods

        /// <summary>
        /// Override default single-byte encoding.
        /// </summary>
        public static void SetAnsiEncoding
            (
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");

            if (!encoding.IsSingleByte)
            {
                throw new ArgumentOutOfRangeException("encoding");
            }

            _ansi = encoding;
        }

        /// <summary>
        /// Override OEM encoding.
        /// </summary>
        public static void SetOemEncoding
            (
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");

            if (!encoding.IsSingleByte)
            {
                throw new ArgumentOutOfRangeException("encoding");
            }

            _oem = encoding;
        }

        #endregion
    }
}
