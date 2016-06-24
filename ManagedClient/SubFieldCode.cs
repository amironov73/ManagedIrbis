/* SubFieldCode.cs -- subfield code
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Subfield code.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class SubFieldCode
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const char DefaultFirstCode = '!';

        /// <summary>
        /// 
        /// </summary>
        public const char DefaultLastCode = '~';

        #endregion

        #region Properties

        /// <summary>
        /// Throw exception on normalization error.
        /// </summary>
        public static bool ThrowOnNormalize { get; set; }

        /// <summary>
        /// Throw exception on verification error.
        /// </summary>
        public static bool ThrowOnVerification { get; set; }

        /// <summary>
        /// List of valid codes.
        /// </summary>
        [NotNull]
        public static CharSet ValidCodes
        {
            get { return _validCodes; }
        }

        #endregion

        #region Construction

        static SubFieldCode()
        {
            _validCodes = new CharSet();
            _validCodes.AddRange(DefaultFirstCode, DefaultLastCode);
        }

        #endregion

        #region Private members

        private static readonly CharSet _validCodes;

        #endregion

        #region Public methods

        /// <summary>
        /// Whether the code valid.
        /// </summary>
        public static bool IsValidCode
            (
                char code
            )
        {
            return ValidCodes.Contains(code);
        }

        /// <summary>
        /// Code normalization.
        /// </summary>
        public static char Normalize
            (
                char code
            )
        {
            return Char.ToLowerInvariant(code);
        }

        /// <summary>
        /// Verify subfield code.
        /// </summary>
        public static bool Verify
            (
                char code,
                bool throwOnError
            )
        {
            bool result = IsValidCode(code);

            if (!result
                && throwOnError)
            {
                throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        /// <summary>
        /// Verify subfield code.
        /// </summary>
        public static bool Verify
            (
                char code
            )
        {
            return Verify
                (
                    code,
                    ThrowOnVerification
                );
        }

        #endregion
    }
}
