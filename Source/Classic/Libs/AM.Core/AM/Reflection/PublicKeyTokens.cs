// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PublicKeyTokens.cs -- common public key tokens
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

#endregion

namespace AM.Reflection
{
    /// <summary>
    /// Common public key tokens.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static class PublicKeyTokens
    {
        #region Private members

        private static readonly byte[] _MicrosoftClr
            = { 0xb7, 0x7a, 0x5c, 0x56, 0x19, 0x34, 0xe0, 0x89 };

        private static readonly byte[] _MicrosoftFX
            = { 0xb0, 0x3f, 0x5f, 0x7f, 0x11, 0xd5, 0x0a, 0x3a };

        private static readonly byte[] _ArsMagna
            = { 0x6d, 0x1a, 0x05, 0xbf, 0x13, 0x4d, 0xec, 0xdf };

        private static readonly byte[] _Istu
            = { 0xdd, 0xab, 0xe0, 0x0d, 0x3f, 0x7e, 0x15, 0xdd };

        #endregion

        #region Properties

        /// <summary>
        /// Microsoft CLR public key token.
        /// </summary>
        public static byte[] MicrosoftClr()
        {
            return (byte[])_MicrosoftClr.Clone();
        }

        /// <summary>
        /// Microsoft .Net FX public key token.
        /// </summary>
        public static byte[] MicrosoftFX()
        {
            return (byte[])_MicrosoftFX.Clone();
        }

        /// <summary>
        /// Ars Magna project public key token.
        /// </summary>
        /// <returns></returns>
        public static byte[] ArsMagna()
        {
            return (byte[])_ArsMagna.Clone();
        }

        /// <summary>
        /// ISTU public key token.
        /// </summary>
        /// <returns></returns>
        public static byte[] Istu()
        {
            return (byte[])_Istu.Clone();
        }

        #endregion
    }
}
