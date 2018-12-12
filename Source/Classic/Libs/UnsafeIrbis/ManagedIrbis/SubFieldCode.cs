// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubFieldCode.cs -- subfield code related routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: good
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

using UnsafeAM.Collections;
using UnsafeAM.Logging;

using JetBrains.Annotations;

#endregion

namespace UnsafeIrbis
{
    /// <summary>
    /// Subfield code related routines.
    /// </summary>
    [PublicAPI]
    public static class SubFieldCode
    {
        #region Constants

        /// <summary>
        /// Begin of valid codes range.
        /// </summary>
        public const char DefaultFirstCode = '!';

        /// <summary>
        /// End of valid codes range (including!).
        /// </summary>
        public const char DefaultLastCode = '~';

        #endregion

        #region Properties

        /// <summary>
        /// Throw exception on verification error.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static bool ThrowOnVerification { get; set; }

        /// <summary>
        /// <see cref="CharSet"/> of valid codes.
        /// </summary>
        [NotNull]
        public static CharSet ValidCodes
        {
            get { return _validCodes; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static SubFieldCode()
        {
            _validCodes = new CharSet();
            _validCodes.AddRange(DefaultFirstCode, DefaultLastCode);
            _validCodes.Remove('^');
        }

        #endregion

        #region Private members

        // ReSharper disable InconsistentNaming
        private static readonly CharSet _validCodes;
        // ReSharper restore InconsistentNaming

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
            return char.ToLowerInvariant(code);
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

            if (!result)
            {
                Log.Error
                    (
                        "SubFieldCode::Verify: "
                        + code
                    );

                if (throwOnError)
                {
                    throw new ArgumentOutOfRangeException();
                }
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
